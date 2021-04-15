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

namespace anime.pages
{
    /// <summary>
    /// Логика взаимодействия для anilib_selected_info_page.xaml
    /// </summary>
    public partial class anilib_selected_info_page : Page
    {
        public anilib_selected_info_page(DataBase.Datum release)
        {
            InitializeComponent();
            DataContext = release.release;
            for (int i = 0; i < release.release.genres.Count; i++)
            {
                if (i == release.release.genres.Count-1)
                {
                    genres.Text += release.release.genres[i];
                }
                else
                {
                    genres.Text += release.release.genres[i] + ", ";
                }
            }
            
        }
    }
}
