using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace anime
{
    public class DataBase
    {
        public static Root db { get; set; }
        public static List<string> Genres { get; set; }

        public class BlockedInfo
        {
            public bool blocked { get; set; }
            public object reason { get; set; }
            public bool bakanim { get; set; }
        }

        public class Playlist
        {
            public int id { get; set; }
            public string title { get; set; }
            public string sd { get; set; }
            public string hd { get; set; }
            public string fullhd { get; set; }
            public string srcHd { get; set; }
        }

        public class Torrent
        {
            public int id { get; set; }
            public string hash { get; set; }
            public int leechers { get; set; }
            public int seeders { get; set; }
            public int completed { get; set; }
            public string quality { get; set; }
            public string series { get; set; }
            public object size { get; set; }
            public string url { get; set; }
            public string urlString { get { return "https://www.anilibria.tv/" + url; } }
            public int ctime { get; set; }
        }

        public class Favorite
        {
            public int rating { get; set; }
            public bool added { get; set; }
        }

        public class Release
        {
            public int id { get; set; }
            public string code { get; set; }
            public List<string> names { get; set; }
            public string name { get
                {
                    if (names[0].Length > 30)
                    {
                        return names[0].Substring(0, 29) + "...";
                    }
                    else
                    {
                        return names[0];
                    }
                } }
            public string series { get; set; }
            public string seriesText { get { return "Серия: " + series; } }
            public string poster { get; set; }
            public string img { get
                {
                    return "https://www.anilibria.tv/" + poster;
                } 
            }
            public string last { get; set; }
            public string moon { get; set; }
            public string announce { get; set; }
            public string status { get; set; }
            public string statusCode { get; set; }
            public string type { get; set; }
            public List<string> genres { get; set; }
            public List<string> voices { get; set; }
            public string year { get; set; }
            public string season { get; set; }
            public string day { get; set; }
            public string description { get; set; }
            public string desc { get {
                    string a = description;
                    string b;
                    try
                    {
                        b = description.Remove(description.IndexOf('<'), description.Length - description.IndexOf('<'));
                    }
                    catch
                    {
                        b = a;
                    }
                    try
                    {
                        a = b.Replace("&quot;", "");
                    }
                    catch (Exception)
                    {
                        a = b;
                    }
                    return a;
                    
                } }
            public BlockedInfo blockedInfo { get; set; }
            public List<Playlist> playlist { get; set; }
            public List<Torrent> torrents { get; set; }
            public Favorite favorite { get; set; }
        }

        public class Datum
        {
            public Release release { get; set; }
        }

        public class Root
        {
            public bool status { get; set; }
            public List<Datum> data { get; set; }
            public object error { get; set; }
        }
    }
}
