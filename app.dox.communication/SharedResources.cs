using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app.dox.communication
{
    /// <summary>
    /// Передача информации между окнами 
    /// </summary>
    public class SharedResources
    {
        /// <summary name="CurrentAddress">
        /// Сдержит адрес изменяемого документа.
        /// </summary>
        public static string CurrentAddress = "";

        /// <summary name="Document">
        /// Содержит разметку XAML изменяемого документа. Необходим для XAML Window
        /// </summary>
        public static string Document;

        /// <summary name="ImageSource">
        /// Содержит путь до передаваемого изображения
        /// </summary>
        public static string ImageSource;
    }
}
