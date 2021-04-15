using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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

namespace anime.pages
{
    /// <summary>
    /// Логика взаимодействия для selected_anime_info_page.xaml
    /// </summary>
    public partial class selected_anime_info_page : Page
    {
        public selected_anime_info_page(anime_base.anime anime)
        {
            InitializeComponent();
            LoadInfo(anime.url);
        }

        static string LoadPage(string url)
        {
            WebClient wc = new WebClient();
            wc.Encoding = Encoding.UTF8;
            wc.Proxy = new WebProxy("45.77.77.98:8080");
            string a = wc.DownloadString(url);
            return a;
        }

        public void LoadInfo(string url)
        {
            var pageContent = LoadPage(url);
            var document = new HtmlDocument();
            document.LoadHtml(pageContent);
            HtmlNode[] desc_list = document.DocumentNode.SelectNodes(".//p[contains(@class, 'detail-description')]").ToArray();
            anime_base.anime anime = manager.db.anime.Where(x=>x.url == url).FirstOrDefault();
            anime.desc = desc_list[0].InnerText.Replace("\n"," ");
            DataContext = anime;
        }
    }
}
