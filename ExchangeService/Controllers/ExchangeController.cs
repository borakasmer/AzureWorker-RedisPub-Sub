using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Services.Exchanges;
using Swashbuckle.AspNetCore.Annotations;
using static ExchangeEnum;

namespace ExchangeService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ExchangeController : ControllerBase
    {
        private readonly IExchangeServices _service;
        public ExchangeController(IExchangeServices service)
        {
            _service = service;
        }

        [HttpGet]
        //Amaç Swagger'da açıklama olarak girilir.
        [SwaggerOperation(Summary = "Kayıtlı tüm Kur bilgilerinin çekilmesi.", Description = "<b>Azure SqlDB üzerinden:</b> </br>Tüm Kur Bilgileri Listelenir.")]
        public ServiceResponse<ExchangeModel> Get()
        {
            return _service.GetAllExchange();
            //return "Hello Exchange World..";
        }
        [HttpGet("GetExchangeByName/{exchangeType}")]
        //Amaç Swagger'da açıklama olarak girilir.
        [SwaggerOperation(Summary = "İstenen tek bir Kur bilgisinin çekilmesi.", Description = "<b>Azure SqlDB üzerinden:</b> </br>istenen Kur bilgisi adından filitrelenerek çekilir. </br>1-) Dolar</br>2-) Euro</br>3-) Pound")]
        public ServiceResponse<ExchangeModel> GetExchangeByName(ExchangeType exchangeType)
        {
            return _service.GetExchangeByName(exchangeType);
        }

        [HttpPost("UpdateExchange")]
        //Amaç Swagger'da açıklama olarak girilir.
        [SwaggerOperation(Summary = "Amaç Gönderilen Kur'un güncellenmesidir.", Description = "<b>Azure SqlDB üzerinde:</b> </br>gönderilen Kur bilgisi güncellenir. </br>1-) Dolar</br>2-) Euro</br>3-) Pound")]
        public ServiceResponse<bool> UpdateExchange(ExchangeModel exchangeData)
        {
            return _service.UpdateExchange(exchangeData);
        }
    }
}
