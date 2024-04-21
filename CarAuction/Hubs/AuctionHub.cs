using CarAuction.Data;
using CarAuction.Models;
using CarAuction.Models.ViewModels;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Timers;
using Microsoft.AspNetCore.Authorization;
using static CarAuction.Controllers.AuctionController;

[Authorize]
public class AuctionHub : Hub
{
    private static System.Timers.Timer _timer;
    private const int TimerInterval = 10000; // Интервал 10 секунд
    private static int _remainingTime = TimerInterval / 1000;
    private static int _lotId;
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

        var lot = _db.Lots.Where(l => l.Id == _lotId).FirstOrDefault();
        lot.isSaled = true;
        await _db.SaveChangesAsync();

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
        _lotId = int.Parse(lotId);
        var userId = Context.User.FindFirstValue(ClaimTypes.NameIdentifier);
        var FinalOwner = _db.ApplicationUsers.FirstOrDefault(u => u.Id == userId);
        lot.FinalCost += 200;

        _db.Bids.Add(new Bid { Amount = lot.FinalCost, User = FinalOwner, AuctionDate = DateTime.Now.ToString(), Lot = lot });
        await _db.SaveChangesAsync();
        await Clients.All.SendAsync("Receive", lot.FinalCost, FinalOwner.FullName);
    }
}