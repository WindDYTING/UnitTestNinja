using System;
using System.Threading;
using System.Threading.Tasks;

namespace MyTestNinja.Difficulty
{
    // 可能自己用 interface 把 RtAdditionInfo 假掉，或 Fleck(package)，不然就暫時不測
    public class AdditionInfoSample
    {
        private readonly RtAdditionInfo<RTComm> _additionSv;

        public AdditionInfoSample(RtAdditionInfo<RTComm> additionSv)
        {
            _additionSv = additionSv;
        }

        public async Task<SubResult> SubscribeStockDataAsync()
        {
            var param = new AdditionInfoWebSocketParam();
            return await _additionSv.SubAdditionInfoDataAsync<StockData>(param, OnStockData);
        }

        private Task OnStockData(string key, IMessage<StockData> message, string csv, SubResult result, CancellationToken token)
        {
            return Task.CompletedTask;
        }
    }

    public class StockData : BaseDataDefinition
    {
        public double Price { get; set; }
    }

//  ------------------------------------------------------------------------------------------------------

    public class BaseDataDefinition
    {

    }

    public class RtAdditionInfo<T>
    {
        public event Action<BaseDataDefinition> OnDataReceived;

        public Task<SubResult> SubAdditionInfoDataAsync<TMessage>(AdditionInfoWebSocketParam param,
            IMessageHandler<TMessage> onMessage)
        {
            // ...
            return Task.FromResult(new SubResult());
        }
    }

    public delegate Task IMessageHandler<in TMessage>(string subKey, IMessage<TMessage> message, string csv,
        SubResult subResult, CancellationToken cancellationToken);

    public interface IMessage<out T> : IMessage
    {
        T Body { get; }
    }

    public interface IMessage
    {
        object GetBody();
        Type MessageType { get; }
    }

    public class RTComm
    {
        public string Commodity { get; set; }
        public int CommodityId { get; set; }
    }

    public class AdditionInfoWebSocketParam
    {
        public string Action => PrivateAction;

        public string DataType { get; set; }

        public string ProviderType { get; set; }

        public string[] KeyNamePath { get; set; }

        public string[] ColumnNames { get; set; }

        public string Value { get; set; }

        internal string PrivateAction { get; set; }

        public string SubKey => this.DataType + string.Join(",", this.ColumnNames) + this.Value;
    }


    public class SubResult
    {

    }
}
