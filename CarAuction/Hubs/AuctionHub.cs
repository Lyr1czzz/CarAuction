using CarAuction.Data;
using CarAuction.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Timers;

public class AuctionHub : Hub
{

    private readonly AppDbContext _db;
    // ... другие методы хаба ...

    public async Task PlaceBid(int lotId, double amount)
    {
        // 1. Получение лота и проверка
        var lot = _db.Lots.Find(lotId);
        if (lot == null)
        {
            await Clients.Caller.SendAsync("Error", "Lot not found.");
            return;
        }

        // 2. Создание ставки
        var bid = new Bid
        {
            Amount = amount,
            AuctionDate = DateTime.Now.ToString(),
            Lot = lot
        };
        _db.Bids.Add(bid);
        await _db.SaveChangesAsync();

        // 3. Отправка обновления клиентам
        await Clients.All.SendAsync("UpdateBid", lotId, amount, bid.AuctionDate);
        await Clients.All.SendAsync("ResetTimer");

        // 4. Логика таймера
        var timer = new System.Timers.Timer(10000); // 10 секунд
        timer.Elapsed += async (sender, e) =>
        {
            // Остановка таймера
            timer.Stop();
            timer.Dispose();

            // Помечаем лот как проданный
            lot.isSaled = true;
            await _db.SaveChangesAsync();

            // Определение следующего лота (адаптируйте под вашу логику)
            var nextLot = _db.Lots
                .Include(l => l.Bids)
                .Include(l => l.Vehicle)
                    .ThenInclude(v => v.Make) // Связь один к одному или один ко многим
                .Include(l => l.Vehicle)
                    .ThenInclude(v => v.Model) // Связь один к одному или один ко многим
                .Include(l => l.Vehicle)
                    .ThenInclude(v => v.Series) // Связь один к одному или один ко многим
                .Include(l => l.Vehicle)
                    .ThenInclude(v => v.Engine) // Связь один к одному или один ко многим
                .Include(l => l.Vehicle)
                    .ThenInclude(v => v.Images) // Связь один ко многим
                .Include(l => l.Auction)
                // ... другие Include для связанных сущностей ...
                .FirstOrDefault(l => l.AuctionId == lot.AuctionId && !l.isSaled);

            // Отправка информации о новом лоте
            await Clients.All.SendAsync("NewLot", nextLot);
        };
        timer.Start();
    }

    // ... другие методы хаба ...
}