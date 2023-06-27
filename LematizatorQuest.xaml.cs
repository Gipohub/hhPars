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

namespace WpfApp1Tech
{
    /// <summary>
    /// Логика взаимодействия для LematizatorQuest.xaml
    /// </summary>
    public partial class LematizatorQuest : Window
    {
        public LematizatorQuest()
        {
            InitializeComponent();
        }

        private void YesOneButton_Click(object sender, RoutedEventArgs e)
        {
            Lemmizator1.Tech = true;
            //Close();
            this.DialogResult = false;
        }

        private void YesTwoButton_Click(object sender, RoutedEventArgs e)
        {
            Lemmizator1.Tech = true;
            Lemmizator1.Pair = true;
            //Close();
            this.DialogResult = false;
        }

        private void YesThreeButton_Click(object sender, RoutedEventArgs e)
        {
            Lemmizator1.Tech = true;
            Lemmizator1.Triple = true;
            //Close();
            this.DialogResult = false;
        }

        private void NoButton_Click(object sender, RoutedEventArgs e)
        {
            //Close();
            this.DialogResult = false;
        }

        private void RichTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
