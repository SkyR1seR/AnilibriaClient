using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace anime
{
    public class manager
    {
        public static anime_base.anime_list db { get; set; }
        public static pages.last_anine_page animeList { get; set; }
        public static MainWindow mw { get; set; }

        public static pages.anilib_selected_info_page anilib { get; set; }
        public static Frame frame { get; set; }
    }
}
