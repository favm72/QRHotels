using System;
using System.Collections.Generic;
using System.Text;

namespace QRWorker
{
    public class WorkerOptions
    {
        public string Connection { get; set; }
        public string HotelCode { get; set; }
        public string SoundPath { get; set; }
        public int Delay { get; set; }
        public string GetHotelsUrl { get; set; }
        public string HasPendingOrdersUrl { get; set; }
    }
}
