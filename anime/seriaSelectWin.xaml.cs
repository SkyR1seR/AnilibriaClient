using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    /// Логика взаимодействия для seriaSelectWin.xaml
    /// </summary>
    public partial class seriaSelectWin : Window
    {
        public seriaSelectWin()
        {
            InitializeComponent();
            animeName.Text = ((DataBase.Release)manager.anilib.DataContext).names[0];
            for (int i = 0; i < ((DataBase.Release)manager.anilib.DataContext).playlist.Count; i++)
            {
                seriaSelect.Items.Add($"Серия: {((DataBase.Release)manager.anilib.DataContext).playlist.Count  - i}");
            }
            quality.Items.Add("480p (sd)");
            if (((DataBase.Release)manager.anilib.DataContext).playlist[0].hd != null)
            {
                quality.Items.Add("720p (HD)");
            }
            if (((DataBase.Release)manager.anilib.DataContext).playlist[0].fullhd != null)
            {
                quality.Items.Add("1080p (FullHD)");
            }

        }

        private void watchBtn_Click(object sender, RoutedEventArgs e)
        {
            switch (quality.SelectedIndex)
            {
                case 0:
                    player player = new player(((DataBase.Release)manager.anilib.DataContext).playlist, ((DataBase.Release)manager.anilib.DataContext).names[0], seriaSelect.SelectedIndex,0);
                    player.Show();
                    return;
                case 1:
                    player player1 = new player(((DataBase.Release)manager.anilib.DataContext).playlist, ((DataBase.Release)manager.anilib.DataContext).names[0], seriaSelect.SelectedIndex,1);
                    player1.Show();
                    //Process.Start("wmplayer.exe", ((DataBase.Release)manager.anilib.DataContext).playlist[seriaSelect.SelectedIndex].hd);
                    return;
                case 2:
                    player player2 = new player(((DataBase.Release)manager.anilib.DataContext).playlist, ((DataBase.Release)manager.anilib.DataContext).names[0], seriaSelect.SelectedIndex,2);
                    player2.Show();
                    return;
                default:
                    break;
            }
            
        }

        private void cancBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
