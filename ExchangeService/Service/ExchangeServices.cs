using Entity.Models.DB;
using System.Linq;
using System.Collections.Generic;
using AutoMapper;
using static ExchangeEnum;
using System;

namespace Services.Exchanges
{
    public class ExchangeServices : IExchangeServices
    {
        private readonly BlackJackContext _context;
        private readonly IRedisCacheService _redisCacheManager;
        private readonly IMapper _mapper;
        public ExchangeServices(BlackJackContext context, IMapper mapper, IRedisCacheService redisCacheManager)
        {
            _context = context;
            _mapper = mapper;
            _redisCacheManager = redisCacheManager;
        }
        public ServiceResponse<ExchangeModel> GetAllExchange()
        {
            var response = new ServiceResponse<ExchangeModel>(null);
            //Check Redis
            var cacheKey = "AllExchange";
            var result = _redisCacheManager.Get<List<ExchangeModel>>(cacheKey);
            //-------------------------------  
            if (result != null)
            {
                response.List = result;
                return response;
            }
            else
            {
                var exchangeResult = _context.Exchange.ToList();
                if (exchangeResult != null)
                {
                    var model = _mapper.Map<IList<ExchangeModel>>(exchangeResult);
                    response.List = model;
                    response.IsSuccessful = true;
                    _redisCacheManager.Set(cacheKey, response.List, DateTime.Now.AddMinutes(1));
                }

                return response;
            }
        }

        public ServiceResponse<ExchangeModel> GetExchangeByName(ExchangeType exchangeType)
        {
            var response = new ServiceResponse<ExchangeModel>(null);

            //Check Redis
            var cacheKey = "Exchange:" + (int)exchangeType;
            var result = _redisCacheManager.Get<ExchangeModel>(cacheKey);
            //-------------------------------  
            if (result != null)
            {
                response.Entity = result;
                return response;
            }
            else
            {
                var exchangeResult = _context.Exchange.Where(ex => ex.Name == exchangeType.ToString()).FirstOrDefault();
                if (exchangeResult != null)
                {
                    var model = _mapper.Map<ExchangeModel>(exchangeResult);
                    response.Entity = model;
                    response.IsSuccessful = true;
                    _redisCacheManager.Set(cacheKey, response.Entity, DateTime.Now.AddMinutes(1));
                }
            }
            return response;
        }
        public ServiceResponse<bool> UpdateExchange(ExchangeModel exchangeData)
        {
            var response = new ServiceResponse<bool>(null);
            try
            {
                _redisCacheManager.Publish("Exchange", exchangeData);
                response.IsSuccessful = true;
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                Console.WriteLine(ex.Message);
            }
            return response;
        }
    }
}