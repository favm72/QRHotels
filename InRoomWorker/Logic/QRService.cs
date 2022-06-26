using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace InRoomWorker.Logic
{
    public class QRService
    {
        ConfigService config;
        public class HotelItem
        {
            public string HotelCode { get; set; }
            public string Description { get; set; }
            public override string ToString()
            {
                return Description;
            }
        }
        public QRService(ConfigService config)
        {
            this.config = config;
        }
        public async Task<List<HotelItem>> GetHotels()
        {
            try
            {
                var uri = config.GetHotelsUrl;
                using var client = new HttpClient();              
                var response = await client.GetAsync(uri);
                if (!response.IsSuccessStatusCode)
                    throw new Exception($"Error al consultar hoteles: {response.StatusCode}");
                var responseJson = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<HotelItem>>(responseJson);
            }
            catch (Exception ex)
            {
                await config.Log(ex.Message);
                return new List<HotelItem>();
            }
        }

        public async Task<bool> HasPendingOrders(string hotelCode)
        {
            try
            {
                var uri = $"{config.HasPendingOrdersUrl}/{hotelCode}";
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
                await config.Log(ex.Message);
                return false;
            }
        }
    }
}
