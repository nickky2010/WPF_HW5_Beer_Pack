using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using WPF_HW5_Beer_Pack.Interfaces;
using WPF_HW5_Beer_Pack.Models;
using WPF_HW5_Beer_Pack.Readers;
using System.Drawing;
using System.Drawing.Imaging;
using WPF_HW5_Beer_Pack.Painters;

namespace WPF_HW5_Beer_Pack
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MainModel model;
        OpenFileDialog openFileDialog;
        SaveFileDialog saveFileDialog;
        Bitmap bmp;
        IPainter painter;
        public MainWindow()
        {
            InitializeComponent();
            string programDirectory = Directory.GetParent(Directory.GetParent(Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).FullName).FullName).FullName; ;

            openFileDialog = new OpenFileDialog
            {
                InitialDirectory = programDirectory + @"\Data",
                FileName = "",
                Filter = "xml file|*.xml",
                DefaultExt = "xml"
            };
            saveFileDialog = new SaveFileDialog
            {
                InitialDirectory = programDirectory,
                Filter = "png file|*.png",
                DefaultExt = "png"
            };
        }

        private void MenuItemSaveToPNG_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (saveFileDialog.ShowDialog() == true)
                {
                    SaveToPng(imageBox, saveFileDialog.FileName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Сould not save file!!! " + ex.Message);
            }
        }

        private void MenuItemFromXML_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (openFileDialog.ShowDialog() == true)
                {
                    IReader<MainModel> reader = new ReaderXML();
                    model = reader.Read(openFileDialog.FileName);
                    imageBox.Width = model.OriginalDocumentWidth;
                    imageBox.Height = model.OriginalDocumentHeight;
                    bmp = new Bitmap((int)imageBox.Width, (int)imageBox.Height);
                    painter = new Painter2D(model, Graphics.FromImage(bmp), new System.Drawing.Pen(System.Drawing.Color.Black, 3));
                    painter.Paint();
                    imageBox.Source = BitmapToImageSource(bmp);
                    menuItemSaveToPNG.IsEnabled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Сould not read file!!! " + ex.Message);
            }
        }

        private void MenuItemExit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        BitmapSource BitmapToImageSource(Bitmap bitmap)
        {
            if (bitmap == null)
                throw new ArgumentNullException("bitmap");
            var rect = new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height);
            var bitmapData = bitmap.LockBits(
                rect,
                ImageLockMode.ReadWrite,
                System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            try
            {
                var size = (rect.Width * rect.Height) * 4;
                return BitmapSource.Create(
                    bitmap.Width,
                    bitmap.Height,
                    bitmap.HorizontalResolution,
                    bitmap.VerticalResolution,
                    PixelFormats.Bgra32,
                    null,
                    bitmapData.Scan0,
                    size,
                    bitmapData.Stride);
            }
            finally
            {
                bitmap.UnlockBits(bitmapData);
            }
        }
        void SaveToPng(FrameworkElement visual, string fileName)
        {
            var encoder = new PngBitmapEncoder();
            SaveUsingEncoder(visual, fileName, encoder);
        }

        void SaveUsingEncoder(FrameworkElement visual, string fileName, BitmapEncoder encoder)
        {
            RenderTargetBitmap bitmap = new RenderTargetBitmap(
                (int)visual.ActualWidth,
                (int)visual.ActualHeight,
                96,
                96,
                PixelFormats.Pbgra32);
            bitmap.Render(visual);
            BitmapFrame frame = BitmapFrame.Create(bitmap);
            encoder.Frames.Add(frame);

            using (var stream = File.Create(fileName))
            {
                encoder.Save(stream);
            }
        }
    }
}
