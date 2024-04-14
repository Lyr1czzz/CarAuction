using CarAuction.Data;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Timers;

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

    public async Task Send(string lotId)
    {
        var lot = _db.Lots.Where(l => l.Id == int.Parse(lotId)).FirstOrDefault();
        lot.FinalCost += 50;
        _db.Lots.Update(lot);
        await _db.SaveChangesAsync();
        await Clients.All.SendAsync("Receive", lot.FinalCost);
    }
}