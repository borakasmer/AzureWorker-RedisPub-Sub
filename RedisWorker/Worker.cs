using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Entity.Models.DB;
using ServiceStack.Redis;
using Newtonsoft.Json;

namespace RedisWorker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            await Task.Delay(1000, stoppingToken);

            var conf = new RedisEndpoint() { Host = "****.redis.cache.windows.net", Port = 6379, Password = "*******" };
            Console.WriteLine("Services Start...");
            using (IRedisClient client = new RedisClient(conf))
            {
                IRedisSubscription sub = null;
                using (sub = client.CreateSubscription())
                {
                    sub.OnMessage += (channel, exchange) =>
                    {
                        try
                        {
                            ExchangeModel _exchange = JsonConvert.DeserializeObject<ExchangeModel>(exchange);
                            Console.WriteLine(_exchange.Name + ": " + _exchange.Value);

                            //Redis UPDATE
                            using (IRedisClient clientServer = new RedisClient(conf))
                            {
                                string redisKey = "Exchange:" + _exchange.ID;
                                clientServer.Set<ExchangeModel>(redisKey, _exchange);
                            }

                            //Sql UPDATE
                            using (BlackJackContext context = new BlackJackContext())
                            {
                                var exchangeModel = context.Exchange.FirstOrDefault(ex => ex.Id == _exchange.ID);
                                exchangeModel.Value = (decimal)_exchange.Value;
                                exchangeModel.UpdateDate = DateTime.Now;
                                context.SaveChanges();
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    };
                    sub.SubscribeToChannels(new string[] { "Exchange" });
                }
            }           
        }
    }
    public class ExchangeModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public double Value { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public System.DateTime? UpdatedDate { get; set; }
    }
}
