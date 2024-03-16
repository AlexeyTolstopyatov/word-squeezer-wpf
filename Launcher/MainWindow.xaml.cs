using Compression;
using System;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;


namespace Launcher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : HandyControl.Controls.Window
    {
        private float _compressedBytes;
        private float _uncompressedBytes;

        public MainWindow()
        {
            InitializeComponent();
            Background = new SolidColorBrush(Color.FromRgb(28 , 28 , 28));
        }

        private void Progress()
        {
            switch (CompressMethod.SelectedIndex)
            {
                case 0:
                if (ReverseCheck.IsChecked == true)
                {
                    char[] text = BeforeProcess.Text.ToCharArray();
                    Array.Reverse(text);
                    AfterProcess.Text = new string(text);

                }
                else
                {
                    AfterProcess.Text = BeforeProcess.Text;
                }
                break;

                case 1:
                if (ReverseCheck.IsChecked == true)
                {
                    char[] text = BeforeProcess.Text.ToCharArray();
                    Array.Reverse(text);

                    Rle rltext = new Rle(new string(text));
                    rltext.Compress();

                    AfterProcess.Text = rltext.Result;
                }
                else
                {
                    Rle rltext = new Rle(BeforeProcess.Text);
                    rltext.Compress();

                    AfterProcess.Text = rltext.Result;
                }
                break;

                case 2:
                if (ReverseCheck.IsChecked == true)
                {
                    char[] text = BeforeProcess.Text.ToCharArray();
                    Array.Reverse(text);

                    Deflate dtext = new Deflate(new string(text));
                    dtext.Compress();

                    AfterProcess.Text = dtext.Result;
                }
                else 
                {
                    Deflate dtext = new Deflate(BeforeProcess.Text);
                    dtext.Compress();

                    AfterProcess.Text = dtext.Result;
                }
                break;
            }
        }

        private void Regress()
        {
            switch (CompressMethod.SelectedIndex)
            {
                case 0:
                if (ReverseCheck.IsChecked == true)
                {
                    char[] text = BeforeProcess.Text.ToCharArray();
                    Array.Reverse(text);
                    AfterProcess.Text = new string(text);
                }
                else
                {
                    AfterProcess.Text = BeforeProcess.Text;
                }
                break;

                case 1:
                if (ReverseCheck.IsChecked == true)
                {

                    Rle rltext = new Rle(BeforeProcess.Text);
                    rltext.Decompress();

                    char[] text = rltext.Result.ToCharArray();

                    Array.Reverse(text);

                    AfterProcess.Text = new string(text);
                }
                else
                {
                    Rle rltext = new Rle(BeforeProcess.Text);
                    rltext.Decompress();

                    AfterProcess.Text = rltext.Result;
                }
                break;

                case 2:
                if (ReverseCheck.IsChecked == true)
                {
                    Deflate dtext = new Deflate(BeforeProcess.Text);
                    dtext.Decompress();

                    char[] text = dtext.Result.ToCharArray();
                    Array.Reverse(text);

                    AfterProcess.Text = new string(text);
                }
                else
                {
                    Deflate dtext = new Deflate(BeforeProcess.Text);
                    dtext.Decompress();

                    AfterProcess.Text = dtext.Result;
                }
                break;
            }
        }

        private void CompressButton_Click(object sender , RoutedEventArgs e)
        {
            Progress();
        }

        private void PropertyButton_Click(object sender , RoutedEventArgs e)
        {
            _compressedBytes = Encoding.UTF8.GetByteCount(AfterProcess.Text);
            _uncompressedBytes = Encoding.UTF8.GetByteCount(BeforeProcess.Text);

            string prop =
                @$"Метод сжатия: {CompressMethod.SelectedValue}
Оригинал: {_uncompressedBytes} Байт
Сжатый текст: {_compressedBytes} Байт";

            HandyControl.Controls.MessageBox.Info(prop , "Свойства текста");
        }

        private void DecompressButton_Click(object sender , RoutedEventArgs e)
        {
            Regress();
        }

        private void BeforeProcess_KeyDown(object sender , KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                Progress();
            else if (e.Key == Key.Escape)
                Regress();
            else if (e.Key == Key.RightCtrl)
            {
                BeforeProcess.Text = AfterProcess.Text;
                AfterProcess.Clear();
            }
        }

        private void ControlHelpButton_Click(object sender , RoutedEventArgs e)
        {
            HandyControl.Controls.MessageBox.Info("[Enter] - Сжать\n[Esc] - Извлечь\n[Right Ctrl] - Обменять поля" , "Управление клавиатурой");
        }
    }
}
