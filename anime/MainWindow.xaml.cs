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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Text.Json;
using System.Net.Http;
using System.Net;
using System.IO;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Threading;
using HtmlAgilityPack;
using Microsoft.Win32;
using System.Diagnostics;

namespace anime
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            manager.mw = this;
            manager.db = new anime_base.anime_list();
            List<anime_base.anime> listAnime = new List<anime_base.anime>();
            manager.db.anime = listAnime;
            manager.frame = mainFrame;
            manager.animeList = new pages.last_anine_page();
            manager.frame.Navigate(manager.animeList);
            animeName.Content = "-";


            animeCountSelectWin seriaSelectWin = new animeCountSelectWin();
            seriaSelectWin.Show();
        }

        private void backBtn_Click(object sender, RoutedEventArgs e)
        {
            if (manager.frame.CanGoBack)
            {
                manager.frame.GoBack();
            }
        }

        private void watchBtn_Click(object sender, RoutedEventArgs e)
        {
            
            if (manager.anilib != null)
            {
                seriaSelectWin seriaSelectWin = new seriaSelectWin();
                seriaSelectWin.Show();
                //Process.Start("wmplayer.exe", ((DataBase.Release)manager.anilib.DataContext).playlist[0].hd);
            }
            
        }

        private void downloadBtn_Click(object sender, RoutedEventArgs e)
        {
            if (manager.anilib != null)
            {
                WebClient wb = new WebClient();
                byte[] torent = wb.DownloadData(((DataBase.Release)manager.anilib.DataContext).torrents.FirstOrDefault().urlString);
                SaveFileDialog sw = new SaveFileDialog();
                sw.FileName = ((DataBase.Release)manager.anilib.DataContext).code + ".torrent";
                sw.Filter = ".torrent | *.torrent";
                sw.InitialDirectory = "Downloads";
                
                if (sw.ShowDialog().Value)
                {
                    File.WriteAllBytes(System.IO.Path.GetFullPath(sw.FileName), torent);
                }
                
            }
           
        }

        private void refreshBtn_Click(object sender, RoutedEventArgs e)
        {
            animeCountSelectWin seriaSelectWin = new animeCountSelectWin();

            seriaSelectWin.Show();
        }
    }
}
