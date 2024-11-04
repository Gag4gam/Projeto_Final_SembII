
using System.Globalization;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;


namespace Projeto_Final_SembII
{

    public partial class MainPage : ContentPage
    {
        private readonly HttpClient _httpClient = new HttpClient();
        private Timer? _timer;
        private int _gyroscopeCount = 0;
        private int _magnetometerCount = 0;

        private double[] _accelerometersensor = { 0.0, 0.0, 0.0 };
        private double[] _gyroscopesensor = { 0.0, 0.0, 0.0 };
        private double[] _magnetometersensor = {0.0,0.0,0.0};

        public MainPage()
        {
            InitializeComponent();
            
            pageBackgroundLayout.BackgroundColor = (Color)Application.Current.Resources["pageBackgroundColor"];

            ExecutarEmThread();
            StartTimer();

        }

        private void StartTimer()
        {
            _timer = new Timer(async (e) =>
            {
                await GetDataAsync();
            }, null, 0, 500); // Chama PostDataAsync a cada 500ms
        }

        public async Task GetDataAsync()
        {
            var data = new
            {
                ax = _accelerometersensor[0],
                ay = _accelerometersensor[1],
                az = _accelerometersensor[2],

                gx = _gyroscopesensor[0],
                gy = _gyroscopesensor[1],
                gz = _gyroscopesensor[2],

                mx = _magnetometersensor[0],
                my = _magnetometersensor[1],
                mz = _magnetometersensor[2]
            };
            _httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "e99b7b66314e4b7abdf478ad0263f785");
            try
            {
                var queryParameters = $"ax={data.ax.ToString(CultureInfo.InvariantCulture)}&ay={data.ay.ToString(CultureInfo.InvariantCulture)}&az={data.az.ToString(CultureInfo.InvariantCulture)}&gx={data.gx.ToString(CultureInfo.InvariantCulture)}&gy={data.gy.ToString(CultureInfo.InvariantCulture)}&gz={data.gz.ToString(CultureInfo.InvariantCulture)}&mx={data.mx.ToString(CultureInfo.InvariantCulture)}&my={data.my.ToString(CultureInfo.InvariantCulture)}&mz={data.mz.ToString(CultureInfo.InvariantCulture)}";
                string url = $"https://webapigerenciamento.azure-api.net/api/retValues?{queryParameters}";

                var json = JsonConvert.SerializeObject(data);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(url,content);
                if (response.IsSuccessStatusCode)
                {
                    var responseData = await response.Content.ReadAsStringAsync();

                }
                else
                {
                    Console.WriteLine($"Request failed with status code: {response.StatusCode}");
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"HttpRequestException: {e.Message}");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception: {e.Message}");
            }

        }
        private void ExecutarEmThread()
        {
            var t1 = new Thread(() =>
            {
                ToggleAccelerometer();
            });

            var t2 = new Thread(() =>
            {
                ToggleGyroscope();
            });

            var t3 = new Thread(() =>
            {
                ToggleMagnetometer();
            });

            t1.Start();
            t2.Start();
            t3.Start();

            t1.Join();
            t2.Join();
            t3.Join();

        }

        private void OnDetailsButtonClicked(object sender, EventArgs e)
        {

            var button = sender as ImageButton;
            var parameter = button?.CommandParameter as string;
            var space = 230;

            switch (parameter)
            {
                case "Accelerometer":
                    DetailsAccelerometer.IsVisible = !DetailsAccelerometer.IsVisible;

                    space = space + (DetailsAccelerometer.IsVisible ? 130 : 0) + (GraphicAccelerometer.IsVisible ? 130 : 0);

                    BoxAccelerometer.HeightRequest = space;

                    break;
                case "Gyroscope":
                    DetailsGyroscope.IsVisible = !DetailsGyroscope.IsVisible;

                    space = space + (DetailsGyroscope.IsVisible ? 130 : 0) + (GraphicGyroscope.IsVisible ? 130 : 0);

                    BoxGyroscope.HeightRequest = space;

                    break;
                case "Magnetometer":
                    DetailsMagnetometer.IsVisible = !DetailsMagnetometer.IsVisible;

                    space = space + (DetailsMagnetometer.IsVisible ? 130 : 0) + (GraphicMagnetometer.IsVisible ? 130 : 0);

                    BoxMagnetometer.HeightRequest = space;

                    break;

            }
        }

        private void OnGraphicsButtonClicked(object sender, EventArgs e)
        {

            var button = sender as ImageButton;
            var parameter = button?.CommandParameter as string;
            var space = 230;

            switch (parameter)
            {
                case "Accelerometer":
                    GraphicAccelerometer.IsVisible = !GraphicAccelerometer.IsVisible;

                    space = space + (DetailsAccelerometer.IsVisible ? 130 : 0) + (GraphicAccelerometer.IsVisible ? 130 : 0);
                    BoxAccelerometer.HeightRequest = space;

                    break;
                case "Gyroscope":
                    GraphicGyroscope.IsVisible = !GraphicGyroscope.IsVisible;

                    space = space + (DetailsGyroscope.IsVisible ? 130 : 0) + (GraphicGyroscope.IsVisible ? 130 : 0);

                    BoxGyroscope.HeightRequest = space;
                    break;
                case "Magnetometer":
                    GraphicMagnetometer.IsVisible = !GraphicMagnetometer.IsVisible;

                    space = space + (DetailsMagnetometer.IsVisible ? 130 : 0) + (GraphicMagnetometer.IsVisible ? 130 : 0);

                    BoxMagnetometer.HeightRequest = space;
                    break;

            }
        }

        public void ToggleAccelerometer()
        {
            if (Accelerometer.Default.IsSupported)
            {
                if (!Accelerometer.Default.IsMonitoring)
                {
                    // Turn on accelerometer
                    Accelerometer.Default.ReadingChanged += Accelerometer_ReadingChanged;
                    Accelerometer.Default.Start(SensorSpeed.Default);


                }
                else
                {
                    // Turn off accelerometer
                    Accelerometer.Default.Stop();
                    Accelerometer.Default.ReadingChanged -= Accelerometer_ReadingChanged;


                }
            }
        }

        private void Accelerometer_ReadingChanged(object? sender, AccelerometerChangedEventArgs e)
        {


            var match = Regex.Match(e.Reading.ToString(), @"X:\s*([-+]?[0-9]*,[0-9]+),\s*Y:\s*([-+]?[0-9]*,[0-9]+),\s*Z:\s*([-+]?[0-9]*,[0-9]+)");

            if (match.Success)
            {
                // Convertendo os valores para double
                double x = double.Parse(match.Groups[1].Value, CultureInfo.GetCultureInfo("pt-BR"));
                double y = double.Parse(match.Groups[2].Value, CultureInfo.GetCultureInfo("pt-BR"));
                double z = double.Parse(match.Groups[3].Value, CultureInfo.GetCultureInfo("pt-BR"));
                _accelerometersensor[0] = double.Parse(match.Groups[1].Value);
                _accelerometersensor[1] = double.Parse(match.Groups[2].Value);
                _accelerometersensor[2] = double.Parse(match.Groups[3].Value);
                // Exibindo os valores
                AccelerometerX.Text = $"X: {x} m/s^2";
                AccelerometerY.Text = $"Y: {y} m/s^2";
                AccelerometerZ.Text = $"Z: {z} m/s^2";
            }
            
        }

        private void ToggleGyroscope()
        {
            if (Gyroscope.Default.IsSupported)
            {
                if (!Gyroscope.Default.IsMonitoring)
                {
                    // Turn on gyroscope
                    Gyroscope.Default.ReadingChanged += Gyroscope_ReadingChanged;
                    Gyroscope.Default.Start(SensorSpeed.Default);


                }
                else
                {
                    // Turn off gyroscope
                    Gyroscope.Default.Stop();
                    Gyroscope.Default.ReadingChanged -= Gyroscope_ReadingChanged;


                }
            }
        }

        private void Gyroscope_ReadingChanged(object? sender, GyroscopeChangedEventArgs e)
        {
            _gyroscopeCount++;
            if (_gyroscopeCount == 3)
            {
                _gyroscopeCount = 0;
                var match = Regex.Match(e.Reading.ToString(), @"X:\s*([-+]?[0-9]*,[0-9]+),\s*Y:\s*([-+]?[0-9]*,[0-9]+),\s*Z:\s*([-+]?[0-9]*,[0-9]+)");
                if (match.Success)
                {
                    // Convertendo os valores para double
                    double x = double.Parse(match.Groups[1].Value, CultureInfo.GetCultureInfo("pt-BR"));
                    double y = double.Parse(match.Groups[2].Value, CultureInfo.GetCultureInfo("pt-BR"));
                    double z = double.Parse(match.Groups[3].Value, CultureInfo.GetCultureInfo("pt-BR"));
                    // Exibindo os valores
                    _gyroscopesensor[0] = x;
                    _gyroscopesensor[1] = y;
                    _gyroscopesensor[2] = z;


                    GyroscopeX.Text = $"X: {x} rad/s";
                    GyroscopeY.Text = $"Y: {y} rad/s";
                    GyroscopeZ.Text = $"Z: {z} rad/s";
                }
            }
        }
        private void ToggleMagnetometer()
        {
            if (Magnetometer.Default.IsSupported)
            {
                if (!Magnetometer.Default.IsMonitoring)
                {
                    // Turn on magnetometer
                    Magnetometer.Default.ReadingChanged += Magnetometer_ReadingChanged;
                    Magnetometer.Default.Start(SensorSpeed.Default);


                }
                else
                {
                    // Turn off magnetometer
                    Magnetometer.Default.Stop();
                    Magnetometer.Default.ReadingChanged -= Magnetometer_ReadingChanged;

                }
            }
        }

        private void Magnetometer_ReadingChanged(object? sender, MagnetometerChangedEventArgs e)
        {
            _magnetometerCount++;
            
            var match = Regex.Match(e.Reading.ToString(), @"X:\s*([-+]?[0-9]*,[0-9]+),\s*Y:\s*([-+]?[0-9]*,[0-9]+),\s*Z:\s*([-+]?[0-9]*,[0-9]+)");
            if (_magnetometerCount == 5)
            {
                _magnetometerCount = 0;
                if (match.Success)
                {
                    // Convertendo os valores para double
                    double x = double.Parse(match.Groups[1].Value, CultureInfo.GetCultureInfo("pt-BR"));
                    double y = double.Parse(match.Groups[2].Value, CultureInfo.GetCultureInfo("pt-BR"));
                    double z = double.Parse(match.Groups[3].Value, CultureInfo.GetCultureInfo("pt-BR"));
                    // Exibindo os valores

                    _magnetometersensor[0] = x;
                    _magnetometersensor[1] = y;
                    _magnetometersensor[2] = z;

                    MagnetometerX.Text = $"X: {x} µT";
                    MagnetometerY.Text = $"Y: {y} µT";
                    MagnetometerZ.Text = $"Z: {z} µT";
                }
            }

        }

        

    }
}
