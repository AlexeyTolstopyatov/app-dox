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

namespace app.dox.message
{
    /// <summary>
    /// Логика взаимодействия для DoxMessageInitializer.xaml
    /// </summary>
    public partial class DoxMessageInitializer : Window
    {
        public DoxMessageInitializer(string Title, string Message, SolidColorBrush CColor)
        {
            InitializeComponent();
            TitleLabel.Content = Title;
            MessageLabel.Text = Message;
            ColoredRectangle.Fill = CColor;

            if (MessageLabel.Height >= Height)
                for (int i = 0; i < MessageLabel.Height + 30; i++)
                    Height += i;
            

        }

        private void DoneButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void MessageMoveEnd(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}
