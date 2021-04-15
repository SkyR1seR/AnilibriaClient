using LibVLCSharp.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using M3USharp;
using System.Net;
using WPFMediaKit;
using MaterialDesignColors;
using System.Threading;
using System.Windows.Threading;

namespace anime
{
    /// <summary>
    /// Логика взаимодействия для player.xaml
    /// </summary>
    public partial class player : Window
    {
        public int selectedSeria = 0;
        List<string> urls = new List<string>();
        bool change = true;
        LibVLCSharp.Shared.MediaPlayer mediaPlayer;
        DispatcherTimer timer = new DispatcherTimer();
        int UItime = 2;
        public player(List<DataBase.Playlist> urlList, string name, int seria, int quality)
        {
            InitializeComponent();
            Core.Initialize();

            LibVLC _LibVLC = new LibVLC();
            string url = "";
            switch (quality)
            {
                case 1:
                    url = urlList[seria].hd;
                    foreach (var item in urlList)
                    {
                        urls.Add(item.hd);
                    }
                    break;
                case 2:
                    url = urlList[seria].fullhd;
                    foreach (var item in urlList)
                    {
                        urls.Add(item.fullhd);
                    }
                    break;
                default:
                    url = urlList[seria].sd;
                    foreach (var item in urlList)
                    {
                        urls.Add(item.sd);
                    }
                    break;
            }
            urls.Reverse();

            selectedSeria = urlList.Count - seria;
            var media = new Media(_LibVLC, url, FromType.FromLocation);
            mediaPlayer = new LibVLCSharp.Shared.MediaPlayer(media) { EnableHardwareDecoding = true };
            Player.MediaPlayer = mediaPlayer;
            mediaPlayer.Buffering += MediaPlayer_Buffering;
            Player.MediaPlayer.Play();
            Player.MediaPlayer.TimeChanged += MediaPlayer_TimeChanged;
            serName.Content = mediaPlayer.Title;
            animeName.Text = name;
            this.Title = name.Substring(0,15)+"...";
            serName.Content = "Серия: " + (selectedSeria);
            timer.Interval = TimeSpan.FromSeconds(5);
            timer.Tick += Timer_Tick;
            timer.Start();
            soundSlider.ValueChanged += soundSlider_ValueChanged;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (UItime <= 0)
            {
                ui1.Visibility = Visibility.Collapsed;
                ui2.Visibility = Visibility.Collapsed;
                ui3.Visibility = Visibility.Collapsed;
                UIvis.Visibility = Visibility.Visible;
                timer.Stop();
            }
            UItime--;
        }

        private void MediaPlayer_TimeChanged(object sender, MediaPlayerTimeChangedEventArgs e)
        {
            if (change)
            {
                changeTime();
            }
            
        }

        private async void MediaPlayer_Buffering(object sender, MediaPlayerBufferingEventArgs e)
        {
            if (e.Cache >=100)
            {
                
                Set();
                
            }
            
        }

        void changeTime()
        {
            if (!Player.CheckAccess())
            {
                Player.Dispatcher.Invoke(new Action(changeTime));
            }
            else
            {
                float lenght = (float)(Player.MediaPlayer.Time/1000);
                float tim = lenght / 60;
                slider.Value = lenght;
                //TimeSpan timespan = TimeSpan.FromMinutes(tim);
                //timeLeft.Content = timespan.ToString("mm\\:ss");
            }
            
        }

        void Set()
        {
            if (!Player.CheckAccess())
            {
                Player.Dispatcher.Invoke(new Action(Set));
            }
            else
            {
                pauseBtn.Visibility = Visibility.Visible;
                playBtn.Visibility = Visibility.Collapsed;
                loadBar.Visibility = Visibility.Collapsed;
                double lenght = ((double)Player.MediaPlayer.Length/1000/60);
                TimeSpan timespan = TimeSpan.FromMinutes(lenght);
                slider.Maximum = lenght * 60;
                time.Content = timespan.ToString("mm\\:ss");
            }
        }

        private void playBtn_Click(object sender, RoutedEventArgs e)
        {
            if (((Button)sender).Name == "pauseBtn")
            {
                Player.MediaPlayer.Pause();
                pauseBtn.Visibility = Visibility.Collapsed;
                playBtn.Visibility = Visibility.Visible;
            }
            else
            {
                Player.MediaPlayer.Play();
                pauseBtn.Visibility = Visibility.Visible;
                playBtn.Visibility = Visibility.Collapsed;
            }
        }

        private void slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

            //Player.MediaPlayer.Time=(long)slider.Value*1000;
            double lenght = (double)(slider.Value/60);
            TimeSpan timespan = TimeSpan.FromMinutes(lenght);
            timeLeft.Content = timespan.ToString("mm\\:ss");
            //loadBar.Visibility = Visibility.Visible;
        }

        private void slider_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            change = true;

            Player.MediaPlayer.Time = (long)slider.Value * 1000;
            loadBar.Visibility = Visibility.Visible;
        }

        private void slider_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            change = false;
        }

        private void fsBtn_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Maximized;
            WindowStyle = WindowStyle.None;
            fsBackBtn.Visibility = Visibility.Visible;
            fsBtn.Visibility = Visibility.Collapsed;
        }

        void Fullscreen()
        {
            if (!Player.CheckAccess())
            {
                Player.Dispatcher.Invoke(new Action(Fullscreen));
            }
            else
            {
                mediaPlayer.Fullscreen = true;
            }
        }

        private void fsBackBtn_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Normal;
            WindowStyle = WindowStyle.SingleBorderWindow;
            fsBackBtn.Visibility = Visibility.Collapsed;
            fsBtn.Visibility = Visibility.Visible;
        }

        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            UItime = 2;
            timer.Start();
            UIvis.Visibility = Visibility.Collapsed;
            ui1.Visibility = Visibility.Visible;
            ui2.Visibility = Visibility.Visible;
            ui3.Visibility = Visibility.Visible;
        }

        private void previewBtn_Click(object sender, RoutedEventArgs e)
        {
            changeSeries(false);
        }

        private void nextBtn_Click(object sender, RoutedEventArgs e)
        {
            changeSeries(true);
        }

        void changeSeries(bool next)
        {
            if (next)
            {
                if (urls.Count > selectedSeria)
                {
                    LibVLC _LibVLC = new LibVLC();

                    var media = new Media(_LibVLC, urls[selectedSeria], FromType.FromLocation);
                    //mediaPlayer = new LibVLCSharp.Shared.MediaPlayer(media) { EnableHardwareDecoding = true };
                    Player.MediaPlayer.Media = media;
                    mediaPlayer.Buffering += MediaPlayer_Buffering;
                    Player.MediaPlayer.Play();
                    selectedSeria++;
                    serName.Content = "Серия: " + (selectedSeria);
                    //MessageBox.Show(selectedSeria.ToString());
                    loadBar.Visibility = Visibility.Visible;
                }
                
            }
            else
            {
                if (selectedSeria > 1)
                {
                    LibVLC _LibVLC = new LibVLC();

                    selectedSeria--;
                    var media = new Media(_LibVLC, urls[selectedSeria-1], FromType.FromLocation);
                    //mediaPlayer = new LibVLCSharp.Shared.MediaPlayer(media) { EnableHardwareDecoding = true };
                    Player.MediaPlayer.Media = media;
                    mediaPlayer.Buffering += MediaPlayer_Buffering;
                    Player.MediaPlayer.Play();
                    serName.Content = "Серия: " + (selectedSeria);
                    //MessageBox.Show(selectedSeria.ToString());
                    loadBar.Visibility = Visibility.Visible;
                }
            }
            
        }

        private void volumeBtn_Click(object sender, RoutedEventArgs e)
        {
            if (soundSlider.Value != 0)
            {
                soundSlider.Value = 0;
            }
            else
            {
                soundSlider.Value = 50;
            }
        }

        private void soundSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            soundChange();
        }

        void soundChange()
        {
            if (!Player.CheckAccess())
            {
                Player.Dispatcher.Invoke(new Action(soundChange));
            }
            else
            {
                Player.MediaPlayer.Volume = (int)Math.Truncate(soundSlider.Value);
            }
        }
    }
}
