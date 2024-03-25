using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Skia;
using Kurs_Proj.test;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.WebSockets;
using Tmds.DBus.Protocol;

namespace Kurs_Proj
{
    public partial class MainWindow : Window
    {
        private bool isConnected = false;
        private bool isStreamOn = false;
        private bool isUpped = false;
        public StreamWriter sw;

        public string? _network;

        public string [] nets;

        public MainWindow()
        {
            InitializeComponent();

            Loh.Text += "\n Programm run";

            using (ApplicationContext db = new ApplicationContext())
            {
                nets = db.Networks.Where(n => n.NetStatus.Equals(true)).Select(n => n.Ip).ToArray();
                Loh.Text += "\n AVAILABLE Networks:";
                foreach (var net in nets) { Loh.Text += "\n"+net; }
            }
        }

        public void Connect(object sender, RoutedEventArgs args)
        {
            _network = ip_input.Text;
            if (!ip_input.IsEnabled)
            {
                isConnected = false;
                ip_input.IsEnabled = true;
                connectButton.IsEnabled = true;
                
                if (isStreamOn)
                {
                    OffSream();

                }
                if (isUpped)
                {
                    isUpped = isStreamOn;
                    UpButton.IsEnabled = isStreamOn;

                    UpButton.Background = new SolidColorBrush(Colors.LightGreen);
                    UpButtonText.Text = "UP";

                    GetLohDB( "Land");
                    GetLog("Dron is landed");
                }

                connectButton.Background = new SolidColorBrush(Colors.LightGreen);
                connectButtonText.Text = "Connect";

                ActivateButtons(isConnected);

                GetLog($"Disconnect from \"{_network}\"");
                sw.Close();
                GetLohDB( "Disconnect");
            }
            else if (nets.Contains(_network))
            {
                sw = new StreamWriter("Lohs/" + $"{DateTime.Now.ToString("dd.MM.yyyy__HH.mm.ss")}__{ip_input.Text}__LOH.txt", true);
                
                GetLohDB( "Connect");
                
                isConnected = true;
                ip_input.IsEnabled = false;
                GetLog($"Connect to \"{_network}\"");
                connectButton.Background = new SolidColorBrush(Colors.Pink);
                connectButtonText.Text = "Disconnect";

                ActivateButtons(isConnected);                
            }
            else
            {
                Loh.Text+="\n Wrong Input IP!!!";
            }
        }
        public void StreamOn(object sender, RoutedEventArgs args)
        {
            if (isStreamOn)
            {
                OffSream();
                return;
            }
            if (isConnected)
            {
                OnStream();
            }
        }
        public void Photo(object sender, RoutedEventArgs args)
        {
            GetLohDB( "Photo");
            GetLog("Getting photo");
        }
        public void Reload(object sender, RoutedEventArgs args)
        {
            GetLohDB("Reload");
            GetLog("Stream was reloaded");
        }
        public void Forward(object sender, RoutedEventArgs args)
        {
            GetLohDB("Forward");
            if (isUpped)
                GetLog($"Dron move FORWARD with forward power {Math.Round(Convert.ToDouble(ForwardSlider.Value))} and up power {Math.Round(Convert.ToDouble(UpForceSlider.Value))}");
            else
                GetLog($"Dron move FORWARD with forward power {Math.Round(Convert.ToDouble(ForwardSlider.Value))} without up power");
        }
        public void Left(object sender, RoutedEventArgs args)
        {
            GetLohDB( "Left");
            if (isUpped)
                GetLog($"Dron move LEFT with forward power {Math.Round(Convert.ToDouble(ForwardSlider.Value))} and up power {Math.Round(Convert.ToDouble(UpForceSlider.Value))}");
            else 
                GetLog($"Dron move LEFT with forward power {Math.Round(Convert.ToDouble(ForwardSlider.Value))} without up power");
        }
        public void Right(object sender, RoutedEventArgs args)
        {
            GetLohDB( "Right");
            if (isUpped)
                GetLog($"Dron move RIGHT with forward power {Math.Round(Convert.ToDouble(ForwardSlider.Value))} and up power {Math.Round(Convert.ToDouble(UpForceSlider.Value))}");
            else
                GetLog($"Dron move RIGHT with forward power {Math.Round(Convert.ToDouble(ForwardSlider.Value))} without up power");
        }
        public void UP(object sender, RoutedEventArgs args)
        {
            if (isUpped) 
            {
                isUpped = false;
                UpButton.Background = new SolidColorBrush(Colors.LightGreen);
                UpButtonText.Text = "UP";
                GetLog("Dron is landed");
                GetLohDB( "Land");
                return;
            }
            isUpped = true;
            UpButton.Background = new SolidColorBrush(Colors.Pink);
            UpButtonText.Text = "X";
            GetLog("Dron is upped");
            GetLohDB( "Up"); 
        }

        private void GetLog(string log)
        {
            Loh.Text += $"\n {DateTime.Now.ToString()} |    " + log;
            sw.Write($"\n {DateTime.Now.ToString()} |    " + log);
        }
        private void GetLohDB(string comand) {
            using (ApplicationContext db = new ApplicationContext())
            {
                var inputComand = db.Comand.Where(n => n.ComandName == comand).First();
                var inputNetwork = db.Networks.Where(n => n.Ip == _network).First();

                var loh = new Loh
                {
                    StatusResponseComand = true,
                    Date = DateTime.Now.ToString()
                };

                inputComand.Lohs.Add(loh);
                inputNetwork.Lohs.Add(loh);

                db.SaveChanges();
            }
        }
        
        private void OffSream()
        {
            StreamText.Text = "Stream is no Live";
            backgroundSream.Background = new SolidColorBrush(Colors.Pink);

            StreamOnButtonText.Text = "ON Stream";
            StreamOnButton.Background = new SolidColorBrush(Colors.LightGreen);
            
            isStreamOn = false;
            PhotoButton.IsEnabled = false;
            ReloadButton.IsEnabled = false;
            
            GetLog("Stream Off");
            GetLohDB( "StreamOff");
        }
        private void OnStream()
        {
            StreamText.Text = "Stream is Live";
            backgroundSream.Background = new SolidColorBrush(Colors.LightGreen);

            StreamOnButtonText.Text = "OFF Stream";
            StreamOnButton.Background = new SolidColorBrush(Colors.Pink);

            isStreamOn = true;
            PhotoButton.IsEnabled = true;
            ReloadButton.IsEnabled = true;

            GetLog("Stream On");
            GetLohDB("StreamOn");
        }
        private void ActivateButtons(bool isActivated) 
        {
            StreamOnButton.IsEnabled = isActivated;
            ForwardButton.IsEnabled = isActivated;
            UpButton.IsEnabled = isActivated;
            LeftButton.IsEnabled = isActivated;
            RightButton.IsEnabled = isActivated;
            ForwardSlider.IsEnabled = isActivated;
            UpForceSlider.IsEnabled = isActivated;
            LightSlider.IsEnabled = isActivated;
            QualitySlider.IsEnabled = isActivated;

            if (!isActivated) 
            {
                ForwardSlider.Value = ForwardSlider.Minimum;
                UpForceSlider.Value = UpForceSlider.Minimum;
                LightSlider.Value = LightSlider.Minimum;
                QualitySlider.Value = QualitySlider.Minimum;
            }

        }
        private void TextBlock_PropertyChanged(object? sender, Avalonia.AvaloniaPropertyChangedEventArgs e)
        {
            if (e.Property.Name == "Text")
            {
                LogSV.ScrollToEnd();
            }
        }
    }
}