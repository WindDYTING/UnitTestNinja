using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

namespace MyTestNinja.Difficulty
{
    public class SecuritiesServiceSample : Form
    {
        private Buttom _btnEnterOrder;
        private readonly PocketUser _user;

        public OrderStatusPackets OrderStatus { get; set; }
        public OrderUpdateResult OrderUpdate { get; set; }
        public TradeUpdateResult TradeUpdate { get; set; }

        public SecuritiesServiceSample()
        {
            InitializeComponents();

            _user = new PocketUser();
            _user.OnReceivedOrderUpdate += OnReceivedOrderUpdate;
            _user.OnReceivedTradeUpdate += OnReceivedTradeUpdate;
        }

        private void InitializeComponents()
        {
            _btnEnterOrder = new Buttom();
            _btnEnterOrder.Clicked += OnButtonClick;
        }

        private void OnReceivedTradeUpdate(TradeUpdateResult ret)
        {
            TradeUpdate = ret;
        }

        private void OnReceivedOrderUpdate(OrderUpdateResult ret)
        {
            OrderUpdate = ret;
        }

        // by button trigger，如果我想測試按鈕按下去的行為怎辦呢？
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

    public class Form : IComponent
    {
        public void Dispose()
        {
            
        }

        public ISite Site { get; set; }
        public event EventHandler Disposed;
    }

    public class Buttom : IComponent
    {
        public void Dispose()
        {
            
        }

        public event EventHandler Clicked;

        public ISite Site { get; set; }
        public event EventHandler Disposed;
    }
}
