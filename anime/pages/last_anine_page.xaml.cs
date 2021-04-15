using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
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
using System.Threading;

namespace anime.pages
{
    /// <summary>
    /// Логика взаимодействия для last_anine_page.xaml
    /// </summary>
    public partial class last_anine_page : Page
    {
        public List<string> genr_list = new List<string>();
        public last_anine_page()
        {
            InitializeComponent();
            //LoadListAnilib();
            //LoadAnilib(1);

            
        }


        public async void LoadAnilib(int count)
        {
            loadBar.Visibility = Visibility.Visible;
            list.ItemsSource = null;
            genrList.ItemsSource = null;
            genrList.ItemsSource = DataBase.Genres;
            genr_list.Clear();
            isFinished.IsChecked = false;
            nameSearchTB.Text = "";
            loadCount.Content = $"Обновление списка";


            DataBase.Genres = new List<string>();
            WebClient wb = new WebClient();
            wb.Encoding = Encoding.UTF8;
            HttpClient client = new HttpClient();
            var values = new Dictionary<string, string>
            {
                {"query","feed"},
                {"page","1"},
                {"perPage",count.ToString()}
            };

            var content = new FormUrlEncodedContent(values);
            try
            {
                var response = await client.PostAsync("https://www.anilibria.tv/public/api/index.php", content);

                DataBase.db = JsonSerializer.Deserialize<DataBase.Root>(await response.Content.ReadAsStringAsync());

                DataBase.Genres = JsonSerializer.Deserialize<List<string>>(wb.DownloadString("http://api.anilibria.tv/v2/getGenres"));
                genrList.ItemsSource = DataBase.Genres;

                DataBase.db = JsonSerializer.Deserialize<DataBase.Root>(await response.Content.ReadAsStringAsync());
                List<DataBase.Datum> listForDelete = new List<DataBase.Datum>();
                foreach (var item in DataBase.db.data)
                {
                    if (item.release == null)
                    {
                        listForDelete.Add(item);
                    }
                }
                foreach (var item in listForDelete)
                {
                    DataBase.db.data.Remove(item);
                }
                loadCount.Content = $"В базу загружено: {DataBase.db.data.Count} аниме";
                loadBar.Visibility = Visibility.Collapsed;
                list.ItemsSource = DataBase.db.data;
            }
            catch (Exception)
            {
                MessageBox.Show("Не удалось подключиться к серверу");
            }
            
        }


        //public void LoadListAnilib()
        //{
        //    var pageContent = LoadPage(@"https://www.anilibria.tv/pages/catalog.php?""year"":"",""genre"":"",""season"":""");
        //    var document = new HtmlDocument();
        //    document.LoadHtml(pageContent);
        //    //HtmlNode[] name_list = document.DocumentNode.SelectNodes(".//div[contains(@class, 'torrent_block')]/div/span[contains(@class, 'side-runame')]").ToArray();
        //    //HtmlNode[] series_list = document.DocumentNode.SelectNodes(".//div[contains(@class, 'torrent_block')]/div/span[contains(@class, 'side-series')]").ToArray();
        //    //HtmlNode[] img_list = document.DocumentNode.SelectNodes(".//div[contains(@class, 'torrent_block')]/img[contains(@class, 'lasttorpic')]").ToArray();
        //    //HtmlNode[] url_list = document.DocumentNode.SelectNodes(".//div[contains(@class, 'torrent_block')]/div/a").ToArray();
        //    HtmlNode[] name_list = document.DocumentNode.SelectNodes(".//span[contains(@class, 'anime_name')]").ToArray();
        //    HtmlNode[] series_list = document.DocumentNode.SelectNodes(".//td/a/div/span[contains(@class, 'anime_number')]").ToArray();
        //    HtmlNode[] img_list = document.DocumentNode.SelectNodes(".//td/a/img[contains(@class, 'torrent_pic')]").ToArray();
        //    HtmlNode[] url_list = document.DocumentNode.SelectNodes(".//td/a").ToArray();
        //    for (int i = 0; i < name_list.Length; i++)
        //    {
        //        anime_base.anime anime = new anime_base.anime();
        //        anime.name = name_list[i].InnerText;
        //        anime.series = series_list[i].InnerText;
        //        anime.img = img_list[i].GetAttributeValue("src", "");
        //        anime.url = "https://www.anilibria.tv" + url_list[i].GetAttributeValue("href", "");
        //        manager.db.anime.Add(anime);
        //    }

        //}



        //static string LoadPage(string url)
        //{
        //    WebClient wc = new WebClient();
        //    wc.Encoding = Encoding.UTF8;
        //    wc.Proxy = new WebProxy("187.44.1.172:8080");
        //    string a = wc.DownloadString(url);
        //    StreamWriter sw = new StreamWriter($"{Environment.CurrentDirectory}/site.txt", false, System.Text.Encoding.Default);
        //    sw.WriteLine(a);
        //    return a;
        //}

        private void list_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (list.SelectedItem != null)
            {
                manager.anilib = new pages.anilib_selected_info_page((DataBase.Datum)list.SelectedItem);
                manager.frame.Navigate(manager.anilib);
                manager.mw.animeName.Content = ((DataBase.Datum)list.SelectedItem).release.name;
                list.SelectedItem = null;
            }
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
           SuperSort(sender);

        }


        private void nameSearchTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            SuperSort(sender);
        }

        
        
        public void SuperSort(object sender)
        {
            try
            {
                CheckBox box = (CheckBox)sender;
                if (box.Content.ToString() == "")
                {
                    if ((bool)isFinished.IsChecked)
                    {
                        list.ItemsSource = WinCool(genr_list).Where(x => x.release.names[0].ToLower().Contains(nameSearchTB.Text.ToLower()) && x.release.status == "Завершен");
                    }
                    else
                    {
                        list.ItemsSource = WinCool(genr_list).Where(x => x.release.names[0].ToLower().Contains(nameSearchTB.Text.ToLower()));
                    }
                }
                else
                {
                    if ((bool)box.IsChecked)
                    {
                        genr_list.Add(box.Content.ToString().ToLower());
                    }
                    else
                    {
                        genr_list.Remove(box.Content.ToString().ToLower());
                    }
                    if ((bool)isFinished.IsChecked)
                    {
                        list.ItemsSource = WinCool(genr_list).Where(x => x.release.names[0].ToLower().Contains(nameSearchTB.Text.ToLower()) && x.release.status == "Завершен");
                    }
                    else
                    {
                        list.ItemsSource = WinCool(genr_list).Where(x => x.release.names[0].ToLower().Contains(nameSearchTB.Text.ToLower()));
                    }
                }
                
            }
            catch (Exception)
            {
                if ((bool)isFinished.IsChecked)
                {
                    list.ItemsSource = WinCool(genr_list).Where(x => x.release.names[0].ToLower().Contains(nameSearchTB.Text.ToLower()) && x.release.status == "Завершен");
                }
                else
                {
                    list.ItemsSource = WinCool(genr_list).Where(x => x.release.names[0].ToLower().Contains(nameSearchTB.Text.ToLower()));
                }
            }
            
        }
        

        public  List<DataBase.Datum> SortList(List<DataBase.Datum> baseAnim, string janr)
        {
           return baseAnim.Where(x => x.release.genres.Contains(janr)).ToList();
        }

        public List<DataBase.Datum> WinCool(List<string> genres)
        {

            var listItem = DataBase.db.data;
            List<string> janrList = genres;
            foreach (var item in janrList)
            {
                listItem =  SortList(listItem, item);
            }
            return listItem;
        }




        private void genresClear_Click(object sender, RoutedEventArgs e)
        {
            //action.IsChecked = false;
            //fantasy.IsChecked = false;
            //comedy.IsChecked = false;
            //adventure.IsChecked = false;
            //romance.IsChecked = false;
            //senen.IsChecked = false;
            //drama.IsChecked = false;
            //school.IsChecked = false;
            //etti.IsChecked = false;
            //magic.IsChecked = false;
            genrList.ItemsSource = null;
            genrList.ItemsSource = DataBase.Genres;
            genr_list.Clear();
            list.ItemsSource = DataBase.db.data;
            isFinished.IsChecked = false;
            nameSearchTB.Text = "";
        }
    }
}
