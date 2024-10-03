using Microsoft.Maui.Controls;
using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;


namespace Projeto_Final_SembII
{
    public partial class MainPage : ContentPage
    {

        public MainPage()
        {
            InitializeComponent();

            pageBackgroundLayout.BackgroundColor = (Color)Application.Current.Resources["pageBackgroundColor"];
            
            ToggleAccelerometer();
            ToggleGyroscope();
            ToggleMagnetometer();

        }

        private void OnDetailsButtonClicked(object sender, EventArgs e)
        {
            System.Console.WriteLine("Chegou");
            var button = sender as ImageButton;
            var parameter = button?.CommandParameter as string;
            System.Console.WriteLine(parameter);
            switch (parameter)
            {
                case "Accelerometer":
                    DetailsAccelerometer.IsVisible = !DetailsAccelerometer.IsVisible;
                    BoxAccelerometer.HeightRequest = DetailsAccelerometer.IsVisible ? 360 : 230;
                    break;
                case "Gyroscope":
                    DetailsGyroscope.IsVisible = !DetailsGyroscope.IsVisible;
                    BoxGyroscope.HeightRequest = DetailsGyroscope.IsVisible ? 360 : 230;
                    break;
                case "Magnetometer":
                    DetailsMagnetometer.IsVisible = !DetailsMagnetometer.IsVisible;
                    BoxMagnetometer.HeightRequest = DetailsMagnetometer.IsVisible ? 360 : 230;
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
                    Accelerometer.Default.Start(SensorSpeed.UI);
                }
                else
                {
                    // Turn off accelerometer
                    Accelerometer.Default.Stop();
                    Accelerometer.Default.ReadingChanged -= Accelerometer_ReadingChanged;
                }
            }
        }

        private void Accelerometer_ReadingChanged(object sender, AccelerometerChangedEventArgs e)
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
            else
            {
                Console.WriteLine("Formato de dados inválido.");
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
                    Gyroscope.Default.Start(SensorSpeed.UI);
                }
                else
                {
                    // Turn off gyroscope
                    Gyroscope.Default.Stop();
                    Gyroscope.Default.ReadingChanged -= Gyroscope_ReadingChanged;
                }
            }
        }

        private void Gyroscope_ReadingChanged(object sender, GyroscopeChangedEventArgs e)
        {


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
            else
            {
                Console.WriteLine("Formato de dados inválido.");
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
                    Magnetometer.Default.Start(SensorSpeed.UI);
                }
                else
                {
                    // Turn off magnetometer
                    Magnetometer.Default.Stop();
                    Magnetometer.Default.ReadingChanged -= Magnetometer_ReadingChanged;
                }
            }
        }

        private void Magnetometer_ReadingChanged(object sender, MagnetometerChangedEventArgs e)
        {


            var match = Regex.Match(e.Reading.ToString(), @"X:\s*([-+]?[0-9]*,[0-9]+),\s*Y:\s*([-+]?[0-9]*,[0-9]+),\s*Z:\s*([-+]?[0-9]*,[0-9]+)");

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
            else
            {
                Console.WriteLine("Formato de dados inválido.");
            }

        }

    }
}
