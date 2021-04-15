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

namespace anime
{
    /// <summary>
    /// Логика взаимодействия для animeCountSelectWin.xaml
    /// </summary>
    public partial class animeCountSelectWin : Window
    {
        public animeCountSelectWin()
        {
            InitializeComponent();
        }

        private void genresClear_Click(object sender, RoutedEventArgs e)
        {
            if ((bool)fast.IsChecked || (bool)normal.IsChecked || (bool)high.IsChecked)
            {
                if ((bool)fast.IsChecked)
                {
                    manager.animeList.LoadAnilib(40);
                }
                if ((bool)normal.IsChecked)
                {
                    manager.animeList.LoadAnilib(600);
                }
                if ((bool)high.IsChecked)
                {
                    manager.animeList.LoadAnilib(2000000);
                }
                this.Close();
            }
        }
    }
}
