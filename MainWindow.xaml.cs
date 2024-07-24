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
using System.IO;
using System.Windows.Controls.Ribbon;
using Microsoft.Win32;
using System.Windows.Controls.Primitives;


namespace app.dox
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            // Установка и проверка //
            FileAddressLabel.Content = app.dox.communication.SharedResources.CurrentAddress;
            AllowsTransparency = true;
            DevTools.IsEnabled = true;

            // Получение данных о шрифтах //
            FontBox.ItemsSource = Fonts.SystemFontFamilies.OrderBy(f => f.Source);
            SizeBox.ItemsSource = new List<double>() { 8, 9, 10, 11, 12, 14, 16, 18, 20, 22, 24, 26, 28, 36, 48, 72 };

            // Если бы Uri работали в XAML корректно, не пришлось бы писать этот ужас //
            message.DoxMessageInitializer Critical;
            try
            {
                File.OpenText($"{Directory.GetCurrentDirectory()}\\app.dox.config.txt");
                _theme = File.ReadAllText($"{Directory.GetCurrentDirectory()}\\app.dox.config.txt");

                Critical = new message.DoxMessageInitializer("Debug", $"config: [{_theme}]", Brushes.Black);
                Critical.ShowDialog();

                try
                {
                    communication.Icons.Install(CloseButton, $"{_icondir}\\{_theme}\\close.png");
                    communication.Icons.Install(MinButton, $"{_icondir}\\{_theme}\\min.png");
                    communication.Icons.Install(MaxButton, $"{_icondir}\\{_theme}\\max.png");
                    communication.Icons.Install(OpenButton, $"{_icondir}\\{_theme}\\open.png");
                    communication.Icons.Install(SaveButton, $"{_icondir}\\{_theme}\\save.png");
                    communication.Icons.Install(XamlWindowInit, $"{_icondir}\\{_theme}\\tabs.png");
                    communication.Icons.Install(btnBold, $"{_icondir}\\{_theme}\\_special\\bold.png");
                    communication.Icons.Install(btnItalic, $"{_icondir}\\{_theme}\\_special\\italic.png");
                    communication.Icons.Install(btnUnderline, $"{_icondir}\\{_theme}\\_special\\UnderLine.png");
                    communication.Icons.Install(_btnAlignCenter, $"{_icondir}\\{_theme}\\_special\\center2.png");
                    communication.Icons.Install(_btnAlignJustify, $"{_icondir}\\{_theme}\\_special\\center.png");
                    communication.Icons.Install(_btnAlignLeft, $"{_icondir}\\{_theme}\\_special\\LeftAlign.png");
                    communication.Icons.Install(_btnAlignRight, $"{_icondir}\\{_theme}\\_special\\RightAlign.png");
                    communication.Icons.Install(_btnBullets, $"{_icondir}\\{_theme}\\_special\\bullets.png");
                    communication.Icons.Install(_btnNumbers, $"{_icondir}\\{_theme}\\_special\\numbered.png");
                    communication.SharedResources.ImageSource = _icondir + _theme +"\\_special\\logo.png";
                }
                catch (Exception ee)
                {
                    Critical = new message.DoxMessageInitializer("Application Error", $"Каталог [{_icondir}\\{_theme}] поврежден или отсутствует. {ee}", Brushes.Black);
                    Critical.ShowDialog();
                    Environment.Exit(0);
                }
            }
            catch
            {
                Critical = new message.DoxMessageInitializer("Application Error", $"Не удалось прочесть файл конфигурации app.dox.config", Brushes.Black);
                Critical.ShowDialog();
                Environment.Exit(0);
            }
        }
        //TODO: изменить на текст из app.dox.theme.t
        public static string _theme;
        public string _icondir = $"{Directory.GetCurrentDirectory()}\\ico\\";
        
        // Движение окна
        private void Window_LeftMouseButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e) => Environment.Exit(0);
        private void OpenFileButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "RTF Документы (*.rtf)|*.rtf|Текстовый файл (*.txt)|*.txt|Открыть как XAML (*.*)|*.*|Все файлы (*.*)|*.*";
            /*
    var result = dlg.ShowDialog();  
    if (result.Value)  
    {  
        TextRange t = new TextRange(Workspace.Document.ContentStart, Workspace.Document.ContentEnd);  
        FileStream file = new FileStream(ofd.FileName, FileMode.Open);  
        t.Load(file, System.Windows.DataFormats.Rtf);  
    }  
             */
            if (ofd.ShowDialog() == true)
            {
                screen.Screen Saver = new screen.Screen();
                message.DoxMessageInitializer warn;
                try
                {
                    TextRange doc = new TextRange(TextFrame.Document.ContentStart, TextFrame.Document.ContentEnd);
                    using (FileStream fs = new FileStream(ofd.FileName, FileMode.Open))
                    {
                        switch (Path.GetExtension(ofd.FileName).ToLower())
                        {
                            case ".rtf":
                                Saver.Show();
                                warn = new message.DoxMessageInitializer("", "Вы можете посмотреть разметку документа, перейдя в меню [Приложение] => XAML", Brushes.DarkCyan);
                                warn.Show();
                                doc.Load(fs, DataFormats.Rtf);
                                TransportToXaml();
                                break;
                            case ".txt":
                                Saver.Show();
                                warn = new message.DoxMessageInitializer("", "Вы открыли неструктурированный документ. вкладка [Приложение] недоступна.", Brushes.DarkCyan);
                                warn.Show();

                                doc.Load(fs, DataFormats.Text);

                                break;
                            default:
                                Saver.Show();
                                warn = new message.DoxMessageInitializer("", "Вы открыли неструктурированный документ. вкладка [Приложение] недоступна.", Brushes.DarkCyan);
                                warn.Show();

                                if (ofd.FilterIndex == 2)
                                    doc.Load(fs, DataFormats.Xaml);
                                else if (ofd.FilterIndex == 3)
                                    doc.Load(fs, DataFormats.Text);
                                
                                break;

                        }
                        Saver.Close();
                    }
                }
                catch (Exception x)
                {
                    message.DoxMessageInitializer ex = new message.DoxMessageInitializer("Document Error", "Нераспознанная структура данных. Документ не поддерживается или поврежден. ", Brushes.DeepSkyBlue);
                    ex.ShowDialog();
                    Saver.Close();
                }
            }
            FileAddressLabel.Content = ofd.FileName;
            app.dox.communication.SharedResources.CurrentAddress = ofd.FileName;
        }
        
        private void SaveFileButton_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Microsoft Office Documents 97-2003 (*.doc)|*.doc|Text Files (*.txt)|*.txt|RichText Files (*.rtf)|*.rtf|XAML Files (*.xaml)|*.xaml|All files (*.*)|*.*";
            if (sfd.ShowDialog() == true)
            {
                TextRange doc = new TextRange(TextFrame.Document.ContentStart, TextFrame.Document.ContentEnd);
                using (FileStream fs = File.Create(sfd.FileName))
                {
                    if (Path.GetExtension(sfd.FileName).ToLower() == ".rtf")
                        doc.Save(fs, DataFormats.Rtf);
                    else if (Path.GetExtension(sfd.FileName).ToLower() == ".txt")
                        doc.Save(fs, DataFormats.Text);
                    else
                        doc.Save(fs, DataFormats.Xaml);
                }
            }
            FileAddressLabel.Content = sfd.FileName;

            app.dox.communication.SharedResources.CurrentAddress = sfd.FileName;
        }
        private void TransportToXaml()
        {
            MemoryStream stream = new MemoryStream();
            using (stream)
            {
                TextRange range = new TextRange(TextFrame.Document.ContentStart,
                    TextFrame.Document.ContentEnd);
                range.Save(stream, DataFormats.Xaml);
                stream.Position = 0;

                // Чтение содержимого из потока и вывод его в текстовом поле. 
                using (StreamReader r = new StreamReader(stream))
                {
                    string line;
                    while ((line = r.ReadLine()) != null)
                        app.dox.communication.SharedResources.Document += line + "\n";
                }
            }
        }
        private void Maximize()
        {
            WindowState = WindowState.Maximized;
            this.Width = SystemParameters.WorkArea.Width;
            this.Height = SystemParameters.WorkArea.Height;
        }
        private void Normalize()
        {
            WindowState = WindowState.Normal;
            Width = 1022;
            Height = 592;
        }
        private void MaxButton_Click(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Normal)
                Maximize();
            else
                Normalize();
        }

        private void DocsControl_MouseLeftButtonClicked(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            XamlWindow xa = new XamlWindow();
            xa.Show();
        }
        // ЫЫЫЫЭЭЭАЭАЭАЭАААААААААА
        private void TextFrame_SelectionChanged(object sender, RoutedEventArgs e)
        {
            object temp = TextFrame.Selection.GetPropertyValue(Inline.FontWeightProperty);
            btnBold.IsChecked = (temp != DependencyProperty.UnsetValue) && (temp.Equals(FontWeights.Bold));
            temp = TextFrame.Selection.GetPropertyValue(Inline.FontStyleProperty);
            btnItalic.IsChecked = (temp != DependencyProperty.UnsetValue) && (temp.Equals(FontStyles.Italic));
            temp = TextFrame.Selection.GetPropertyValue(Inline.TextDecorationsProperty);
            btnUnderline.IsChecked = (temp != DependencyProperty.UnsetValue) && (temp.Equals(TextDecorations.Underline));

            temp = TextFrame.Selection.GetPropertyValue(Inline.FontFamilyProperty);
            FontBox.SelectedItem = temp;
            temp = TextFrame.Selection.GetPropertyValue(Inline.FontSizeProperty);
            SizeBox.Text = temp.ToString();

            UpdateVisualState();
        }

        private void FontBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (FontBox.SelectedItem != null)
                TextFrame.Selection.ApplyPropertyValue(Inline.FontFamilyProperty, FontBox.SelectedItem);
        }

        void UpdateItemCheckedState(ToggleButton button, DependencyProperty formattingProperty, object expectedValue)
        {
            object currentValue = TextFrame.Selection.GetPropertyValue(formattingProperty);
            button.IsChecked = (currentValue == DependencyProperty.UnsetValue) ? false : currentValue != null && currentValue.Equals(expectedValue);
        }

        void ApplyPropertyValueToSelectedText(DependencyProperty formattingProperty, object value)
        {
            if (value == null)
                return;
            TextFrame.Selection.ApplyPropertyValue(formattingProperty, value);
        }

        private void UpdateToggleButtonState()
        {
            UpdateItemCheckedState(_btnAlignLeft, Paragraph.TextAlignmentProperty, TextAlignment.Left);
            UpdateItemCheckedState(_btnAlignCenter, Paragraph.TextAlignmentProperty, TextAlignment.Center);
            UpdateItemCheckedState(_btnAlignRight, Paragraph.TextAlignmentProperty, TextAlignment.Right);
            UpdateItemCheckedState(_btnAlignJustify, Paragraph.TextAlignmentProperty, TextAlignment.Right);
        }

        private void UpdateItemCheckedState(RibbonButton btnAlignLeft, DependencyProperty textAlignmentProperty, TextAlignment left)
        {
            throw new NotImplementedException();
        }

        private void UpdateSelectionListType()
        {
            Paragraph startParagraph = TextFrame.Selection.Start.Paragraph;
            Paragraph endParagraph = TextFrame.Selection.End.Paragraph;
            if (startParagraph != null && endParagraph != null && (startParagraph.Parent is ListItem) && (endParagraph.Parent is ListItem) && object.ReferenceEquals(((ListItem)startParagraph.Parent).List, ((ListItem)endParagraph.Parent).List))
            {
                TextMarkerStyle markerStyle = ((ListItem)startParagraph.Parent).List.MarkerStyle;
                if (markerStyle == TextMarkerStyle.Disc) //bullets  
                {
                    _btnBullets.IsChecked = true;
                }
                else if (markerStyle == TextMarkerStyle.Decimal) //number  
                {
                    _btnNumbers.IsChecked = true;
                }
            }
            else
            {
                _btnBullets.IsChecked = false;
                _btnNumbers.IsChecked = false;
            }
        }
        private void UpdateVisualState()
        {
            UpdateToggleButtonState();
            UpdateSelectionListType();
        }


        // Выбор цвета //
        private void fontcolor(RichTextBox rc)
        {
            var colorDialog = new System.Windows.Forms.ColorDialog(); // вот мы и приплыли блять 
            if (colorDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                var wpfcolor = Color.FromArgb(colorDialog.Color.A, colorDialog.Color.R, colorDialog.Color.G, colorDialog.Color.B);
                TextRange range = new TextRange(rc.Selection.Start, rc.Selection.End);
                range.ApplyPropertyValue(FlowDocument.ForegroundProperty, new SolidColorBrush(wpfcolor));
            }
        }
        

        private void SettingsFontColor_Click(object sender, RoutedEventArgs e)
        {
            fontcolor(TextFrame);
        }

        // AddImage //
        // 1 Выбрать изображение 
        // 2 Вставить изображение, Обновить документ 
        public void selectImg(RichTextBox rc)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Image files (*.jpg, *.jpeg,*.gif, *.png) | *.jpg; *.jpeg; *.gif; *.png";
            var result = dlg.ShowDialog();
            if (result.Value)
            {
                Uri uri = new Uri(dlg.FileName, UriKind.Relative);
                BitmapImage bitmapImg = new BitmapImage(uri);
                Image image = new Image();
                image.Stretch = Stretch.Fill;
                image.Width = 250;
                image.Height = 200;
                image.Source = bitmapImg;
                var tp = rc.CaretPosition.GetInsertionPosition(LogicalDirection.Forward);
                new InlineUIContainer(image, tp);
            }
        }
        private void addImage_Click(object sender, RoutedEventArgs e)
        {
            selectImg(TextFrame);
        }

        private void Msgbutton_Click(object sender, RoutedEventArgs e)
        {
            message.DoxMessageInitializer testmsg = new message.DoxMessageInitializer("bruh button", "bruh title", Brushes.Blue);
            testmsg.Show();
        }
    }
}
