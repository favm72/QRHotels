using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace InRoomWorker.Logic
{
    public class ConfigService
    {
        public string GetHotelsUrl { get; set; }
        public string HasPendingOrdersUrl { get; set; }
        public async Task Read()
        {
            try
            {
#if DEBUG
                string content = await System.IO.File.ReadAllTextAsync("../../../appsettings.json");
#else
            string content = await System.IO.File.ReadAllTextAsync("appsettings.json");
#endif
                var data = JsonConvert.DeserializeObject<Dictionary<string, string>>(content);
                GetHotelsUrl = data.GetValueOrDefault("GetHotelsUrl");
                HasPendingOrdersUrl = data.GetValueOrDefault("HasPendingOrdersUrl");
            }
            catch (Exception ex)
            {
                await Log($"error reading settings {ex.Message}");
            }        
        }
        public async Task Log(string message)
        {
            try
            {
                await System.IO.File.AppendAllTextAsync("log.txt", $"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}\n{message}", Encoding.UTF8);
            }
            catch (Exception)
            {

            }
        }
    }
}
