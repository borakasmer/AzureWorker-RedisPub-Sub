using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

public class ServiceResponse<T> : IServiceResponse<T>
    {

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string ExceptionMessage { get; set; }

        public IList<T> List { get; set; }

        [JsonProperty]
        public T Entity { get; set; }

        public int Count { get; set; }

        public bool IsSuccessful { get; set; }
        public ServiceResponse(HttpContext context)
        {
            
        }
    }