using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyTestNinja.Difficulty
{
    public class SecuritiesServiceSample
    {
        private PocketUser _user;

        public OrderStatusPackets OrderStatus { get; set; }
        public OrderUpdateResult OrderUpdate { get; set; }
        public TradeUpdateResult TradeUpdate { get; set; }

        public SecuritiesServiceSample()
        {
            _user = new PocketUser();
            _user.OnReceivedOrderUpdate += OnReceivedOrderUpdate;
            _user.OnReceivedTradeUpdate += OnReceivedTradeUpdate;
        }

        private void OnReceivedTradeUpdate(TradeUpdateResult ret)
        {
            TradeUpdate = ret;
        }

        private void OnReceivedOrderUpdate(OrderUpdateResult ret)
        {
            OrderUpdate = ret;
        }

        // by button trigger
        private async void OnButtonClick(object sender, EventArgs e)
        {
            OrderStatus = await _user.EnterOrderAsync("2330", 1, 1);
        }
    }

    //   -------------------------------------
    public class PocketUser
    {

        public PocketUser()
        {
        }

        public Action<OrderUpdateResult> OnReceivedOrderUpdate { get; set; }

        public Action<TradeUpdateResult> OnReceivedTradeUpdate { get; set; }

        private void ReceivedPackets()
        {
            OnReceivedOrderUpdate?.Invoke(new OrderUpdateResult());
            OnReceivedTradeUpdate?.Invoke(new TradeUpdateResult());
        }
        public Task<OrderStatusPackets> EnterOrderAsync(string commodities
            , int side
            , int vol
            , double? price = null)
        {
            return Task.FromResult(new OrderStatusPackets())
                .ContinueWith(task =>
                {
                    ReceivedPackets();
                    return task.Result;
                });
        }
    }

    public class OrderStatusPackets
    {
    }

    public class PacketEventArgs : EventArgs
    {
        public PacketEventArgs(IReadOnlyDictionary<string, object> packets)
        {
            Packets = packets;
        }

        public IReadOnlyDictionary<string, object> Packets { get; }

    }

    public class TradeUpdateResult
    {
    }

    public class OrderUpdateResult
    {
    }

    public interface ISecuritiesConnection
    {
    }
    
}
