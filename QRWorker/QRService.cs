using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace QRWorker
{
    public class QRService
    {  
        public class HotelItem
        {
            public string HotelCode { get; set; }
            public string Description { get; set; }
            public override string ToString()
            {
                return Description;
            }
        }
        ILogger<Worker> logger;
        WorkerOptions options;
        public QRService(WorkerOptions options, ILogger<Worker> logger)
        {
            this.options = options;
            this.logger = logger;
        }
        public async Task<List<HotelItem>> GetHotels()
        {
            try
            {
                var uri = options.GetHotelsUrl;
                using var client = new HttpClient();              
                var response = await client.GetAsync(uri);
                if (!response.IsSuccessStatusCode)
                    throw new Exception($"Error al consultar hoteles: {response.StatusCode}");
                var responseJson = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<HotelItem>>(responseJson);
            }
            catch (Exception ex)
            {
                logger.LogInformation(ex.Message);
                return new List<HotelItem>();
            }
        }

        public async Task<bool> HasPendingOrders(string hotelCode)
        {
            try
            {
                var uri = $"{options.HasPendingOrdersUrl}/{hotelCode}";
                using var client = new HttpClient();
                var response = await client.GetAsync(uri);
                if (!response.IsSuccessStatusCode)
                    throw new Exception($"Error al consultar ordenes {response.StatusCode}");
                var responseJson = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<Dictionary<string, bool>>(responseJson);
                return data.GetValueOrDefault("result");
            }
            catch (Exception ex)
            {
                logger.LogInformation(ex.Message);
                return false;
            }
        }
    }
}
