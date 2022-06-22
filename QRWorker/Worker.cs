using DataLayer;
using LogicLayer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace QRWorker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly WorkerOptions options;
        public Worker(ILogger<Worker> logger, WorkerOptions options)
        {
            _logger = logger;
            this.options = options;
        }

        public MyContext UseContext()
        {
            var storage = this.options.Connection;
            var optionsBuilder = new DbContextOptionsBuilder<MyContext>();
            optionsBuilder.UseSqlServer(storage);
            //optionsBuilder.LogTo(filter, (s) => { System.IO.File.AppendAllText("test.txt", "\n" + s); });
            var options = optionsBuilder.Options;
            return new MyContext(options);
        }

        public void PlaySound()
        {
            try
            {
                if (!System.IO.File.Exists(options.SoundPath))
                {
                    _logger.LogInformation("sound file not found");
                    return;
                }

                var wave = new AudioFileReader(options.SoundPath);
                OffsetSampleProvider offsetSampleProvider = new OffsetSampleProvider(wave);
                //offsetSampleProvider.SkipOver = TimeSpan.FromSeconds(timeFrom);
                //offsetSampleProvider.Take = TimeSpan.FromSeconds(timeTo - timeFrom);
                var outputSound = new WaveOut();
                outputSound.Init(offsetSampleProvider);
                outputSound.Play();
                _logger.LogInformation("sound played");
            }
            catch (Exception ex)
            {
                _logger.LogInformation("error playing file", ex.Message);
            }            
        }


        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(options.Delay, stoppingToken);
                using (var context = UseContext())
                {
                    var orderBL = new OrderBL(context);
                    var hasPendingOrders = await orderBL.HasPendingOrders(options.HotelCode);
                    _logger.LogInformation("has pending orders", hasPendingOrders);
                    if (hasPendingOrders)
                    {
                        PlaySound();                        
                    }
                }

            }
        }
    }
}
