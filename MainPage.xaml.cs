using Microsoft.Maui.Controls;
using System;
using System.Globalization;
using LiveCharts;
using System.Text.RegularExpressions;
using System.Timers;


namespace Projeto_Final_SembII
{

    public partial class MainPage : ContentPage
    {

        private int _gyroscopeCount = 0;
        private int _magnetometerCount = 0;
        public MainPage()
        {
            InitializeComponent();

            pageBackgroundLayout.BackgroundColor = (Color)Application.Current.Resources["pageBackgroundColor"];

            ExecutarEmThread();


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
                    MagnetometerX.Text = $"X: {x} µT";
                    MagnetometerY.Text = $"Y: {y} µT";
                    MagnetometerZ.Text = $"Z: {z} µT";
                }
            }

        }

        

    }
}
