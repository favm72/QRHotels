using InRoomWorker.Logic;
using NAudio.Wave;
using System;
using System.Threading.Tasks;
using System.Windows;
using static InRoomWorker.Logic.QRService;

namespace InRoomWorker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        QRService service;
        ConfigService config;
        bool serviceActive = false;


        public MainWindow()
        {
            config = new ConfigService();
            service = new QRService(config);

            InitializeComponent();
        }

        public async Task LoadHotels()
        {
            await config.Read();
            ddlHotel.Items.Clear();
            var data = await service.GetHotels();
            foreach (var item in data)
            {
                ddlHotel.Items.Add(item);
            }
        }

        public async Task PlaySound()
        {
            try
            {
                if (!System.IO.File.Exists(txtPath.Text))
                {
                    await config.Log("sound file not found");
                    return;
                }

                var reader = new Mp3FileReader(txtPath.Text);
                var waveOut = new WaveOut();
                waveOut.Init(reader);
                waveOut.Play();
            }
            catch (Exception ex)
            {
                await config.Log(ex.Message);
            }
        }

        private async Task DoProcess()
        {
            while (serviceActive)
            {
                try
                {
                    int delay;
                    //bool parsed = int.TryParse(txtDelay.Text, out delay);
                    //if (!parsed)
                    delay = 10000;
                    await Task.Delay(delay);

                    await config.Read();
                    if (string.IsNullOrWhiteSpace(config.HasPendingOrdersUrl))
                    {
                        await config.Log("Pending orders api no está configurado");
                        continue;
                    }
                    if (ddlHotel.SelectedItem == null)
                    {
                        await config.Log("No se ha seleccionado un Hotel");
                        continue;
                    }
                    if (string.IsNullOrWhiteSpace(txtPath.Text))
                    {
                        await config.Log("No se configuro la ruta del archivo mp3");
                        continue;
                    }
                    if (!System.IO.File.Exists(txtPath.Text))
                    {
                        await config.Log($"El archivo mp3 no existe en la ruta {txtPath.Text}");
                        continue;
                    }

                    string hotelCode = (ddlHotel.SelectedItem as HotelItem).HotelCode;
                    bool playSound = await service.HasPendingOrders(hotelCode);
                    if (playSound)
                    {
                        await PlaySound();
                    }
                }
                catch (Exception ex)
                {
                    await config.Log(ex.Message);
                }
            }
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            btnStart.Content = "Start";
            await LoadHotels();
        }

        private async void btnStart_Click(object sender, RoutedEventArgs e)
        {
            if (serviceActive)
            {
                serviceActive = false;
                btnStart.Content = "Start";
                await config.Log("Service stopped");
            }
            else
            {
                await config.Read();
                if (string.IsNullOrWhiteSpace(config.HasPendingOrdersUrl))
                {
                    MessageBox.Show("Error al leer el arhivo appsettings.json", "Alerta");
                    return;
                }
                if (ddlHotel.SelectedItem == null)
                {
                    MessageBox.Show("No se ha seleccionado un Hotel", "Alerta");
                    return;
                }
                if (string.IsNullOrWhiteSpace(txtPath.Text))
                {
                    MessageBox.Show("No se configuro la ruta del archivo mp3", "Alerta");
                    return;
                }
                if (!System.IO.File.Exists(txtPath.Text))
                {
                    MessageBox.Show($"El archivo mp3 no existe en la ruta {txtPath.Text}", "Alerta");
                    return;
                }
                btnStart.Content = "Stop";
                serviceActive = true;
                await DoProcess();
            }
        }
    }
}
