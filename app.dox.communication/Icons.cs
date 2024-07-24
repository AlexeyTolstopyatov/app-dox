using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
//using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.IO;
using System.Windows.Controls.Ribbon;
using System.Drawing;

namespace app.dox.communication
{
    public class Icons
    {
        /// <summary>
        /// Устанавливает значки на указанный элемент
        /// </summary>
        /// <param name="rButton"></param>
        /// <param name="newUri"></param>
        public static void Install(RibbonButton rButton, string newUri)
        {
            rButton.Background              = new ImageBrush(new BitmapImage(new Uri(newUri)));
            rButton.FocusedBackground       = new ImageBrush(new BitmapImage(new Uri(newUri)));
            rButton.MouseOverBackground     = new ImageBrush(new BitmapImage(new Uri(newUri)));
        }
        public static void Install(Button sButton, string newUri)
        {
            sButton.Background = new ImageBrush(new BitmapImage(new Uri(newUri)));
        }
        public static void Install(RibbonRadioButton rrButton, string newUri)
        {
            rrButton.SmallImageSource = new BitmapImage(new Uri(newUri));
        }
        public static void Install(RibbonToggleButton tButton, string newUri)
        {
            tButton.SmallImageSource = new BitmapImage(new Uri(newUri));
        }
    }
}
