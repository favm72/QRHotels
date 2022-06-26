using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NAudio.Wave;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace QRWorker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> logger;
        private readonly WorkerOptions options;
        private readonly QRService service;
        public Worker(ILogger<Worker> logger, WorkerOptions options)
        {
            this.logger = logger;
            this.options = options;
            service = new QRService(options, logger);
        }

        public void PlaySound()
        {
            try
            {
                if (!System.IO.File.Exists(options.SoundPath))
                {
                    logger.LogInformation("sound file not found");
                    return;
                }

                var wave = new AudioFileReader(options.SoundPath);              
                var outputSound = new WaveOut();
                outputSound.Init(wave);
                outputSound.Play();                
            }
            catch (Exception ex)
            {
                logger.LogInformation("error playing file", ex.Message);
            }            
        }


        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await Task.Delay(options.Delay, stoppingToken);                 
                    if (string.IsNullOrWhiteSpace(options.HasPendingOrdersUrl))
                    {
                        logger.LogInformation("HasPendingOrdersUrl is empty");
                        continue;
                    }
                    if (string.IsNullOrWhiteSpace(options.HotelCode))
                    {
                        logger.LogInformation("HotelCode is empty");
                        continue;
                    }
                    if (string.IsNullOrWhiteSpace(options.SoundPath))
                    {
                        logger.LogInformation("SoundPath is empty");
                        continue;
                    }
                    if (!System.IO.File.Exists(options.SoundPath))
                    {
                        logger.LogInformation($"mp3 file does not exists in path {options.SoundPath}");
                        continue;
                    }
                    
                    bool playSound = await service.HasPendingOrders(options.HotelCode);
                    if (playSound)
                        PlaySound();
                }
                catch (Exception ex)
                {
                    logger.LogInformation(ex.Message);
                }                        
            }
        }
    }
}
