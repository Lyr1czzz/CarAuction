using CarAuction.Models;
using Microsoft.AspNetCore.SignalR;

namespace CarAuction.Hubs
{
    public class AuctionHub : Hub
    {
        public async Task SendBid(int lotId, double bidAmount)
        {
            // Этот метод будет вызываться из JavaScript на клиенте
            // Он должен только отправить информацию о ставке на сервер

            await Clients.All.SendAsync("BidSubmitted", lotId, bidAmount);
        }

        public async Task StartTimer(int lotId)
        {
            // Этот метод будет вызываться из контроллера, когда нужно запустить таймер

            await Clients.All.SendAsync("TimerStarted", lotId);
        }

        public async Task UpdateTimer(int lotId, int remainingTime)
        {
            // Этот метод будет вызываться из контроллера для обновления таймера на клиентах

            await Clients.All.SendAsync("TimerUpdated", lotId, remainingTime);
        }

        public async Task LotSold(Lot soldLot, Lot nextLot)
        {
            // Этот метод будет вызываться из контроллера, когда лот продан, 
            // чтобы обновить информацию на клиентах

            await Clients.All.SendAsync("LotSold", soldLot, nextLot);
        }
    }
}