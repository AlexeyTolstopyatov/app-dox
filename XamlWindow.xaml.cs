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
using app.dox;

namespace app.dox
{
    /// <summary>
    /// Логика взаимодействия для XamlWindow.xaml
    /// </summary>
    public partial class XamlWindow : Window
    {
        public XamlWindow()
        {
            
            InitializeComponent();
            XaDocument.Document.Blocks.Clear(); // очищает поле при повторном открытии
            
            XaLabel.Content = app.dox.communication.SharedResources.CurrentAddress;
            XaDocument.AppendText(app.dox.communication.SharedResources.Document);
            if(XaDocument.Document.ToString() != communication.SharedResources.Document)
                app.dox.communication.SharedResources.Document = string.Empty; // сбрасывает разметку предыдущего документа

            
        }

        private void WindowMoving(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        
        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
