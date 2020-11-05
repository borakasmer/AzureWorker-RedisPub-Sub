using System.Collections.Generic;
using static ExchangeEnum;

namespace Services.Exchanges
{
    public interface IExchangeServices
    {
        ServiceResponse<ExchangeModel> GetAllExchange();
        ServiceResponse<ExchangeModel> GetExchangeByName(ExchangeType exchangeType);
        ServiceResponse<bool> UpdateExchange(ExchangeModel exchangeData);
    }
}