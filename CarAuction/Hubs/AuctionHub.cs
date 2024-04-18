using CarAuction.Data;
using CarAuction.Models;
using CarAuction.Models.ViewModels;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Timers;
using static CarAuction.Controllers.AuctionController;

public class AuctionHub : Hub
{
    private static System.Timers.Timer _timer;
    private const int TimerInterval = 10000; // Интервал 10 секунд
    private static int _remainingTime = TimerInterval / 1000;
    private static IHubContext<AuctionHub> _hubContext;
    private readonly AppDbContext _db;

    public AuctionHub(IHubContext<AuctionHub> hubContext, AppDbContext db)
    {
        _db = db;
        _hubContext = hubContext;
        if (_timer == null)
        {
            _timer = new System.Timers.Timer(1000); // Интервал в 1 секунду
            _timer.AutoReset = true;
            _timer.Elapsed += OnTimerElapsed;
        }
    }

    public Task StartOrResetTimer()
    {
        _remainingTime = TimerInterval / 1000;
        if (!_timer.Enabled)
        {
            _timer.Start();
        }
        else
        {
            _timer.Stop();
            _timer.Start(); // Перезапускаем таймер
        }

        return Clients.All.SendAsync("TimerUpdated", _remainingTime);
    }

    public Lot GetCurrentLot(int AuctionId)
    {
        return _db.Lots
            .Include(l => l.Bids)
            .Include(l => l.Vehicle)
                .ThenInclude(v => v.Make) 
            .Include(l => l.Vehicle)
                .ThenInclude(v => v.Model)
            .Include(l => l.Vehicle)
                .ThenInclude(v => v.Series)
            .Include(l => l.Vehicle)
                .ThenInclude(v => v.Engine)
            .Include(l => l.Vehicle)
                .ThenInclude(v => v.Images)
        .Include(l => l.Auction)
            .FirstOrDefault(l => l.Auction.isActive && !l.isSaled && l.AuctionId == AuctionId);
    }

    private async void OnTimerElapsed(object sender, ElapsedEventArgs e)
    {
         _remainingTime--;

        if (_remainingTime <= 0)
        {
            _timer.Stop();
           

            await _hubContext.Clients.All.SendAsync("TimerEnded");
            
        }
        else
        {
            await _hubContext.Clients.All.SendAsync("TimerUpdated", _remainingTime);
        }
    }

    public async Task NextLot(string AuctionId)
    {
        var nextLots = _db.Lots
            .Include(l => l.Bids)
            .Include(l => l.Vehicle)
                .ThenInclude(v => v.Make)
            .Include(l => l.Vehicle)
                .ThenInclude(v => v.Model)
            .Include(l => l.Vehicle)
                .ThenInclude(v => v.Series)
            .Include(l => l.Vehicle)
                .ThenInclude(v => v.Engine)
            .Include(l => l.Vehicle)
                .ThenInclude(v => v.Images)
            .Include(l => l.Auction).
            Where(l => l.AuctionId == int.Parse(AuctionId) && l.isSaled == false).ToList();

        var newVM = new AuctionViewModel
        {
            CurrentLot = GetCurrentLot(int.Parse(AuctionId)),
            NextLots = nextLots
        };

        await Clients.All.SendAsync("ChangeLot", newVM);
    }

    public async Task Send(string lotId)
    {
        var lot = _db.Lots.Where(l => l.Id == int.Parse(lotId)).FirstOrDefault();
        lot.FinalCost += 50;
        lot.isSaled = false;
        await _db.SaveChangesAsync();
        await Clients.All.SendAsync("Receive", lot.FinalCost);
    }
}