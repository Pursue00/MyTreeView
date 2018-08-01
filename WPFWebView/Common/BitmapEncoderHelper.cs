using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace WPFWebView.Common
{
    public class BitmapEncoderHelper
    {
        public static BitmapImage PathToBitmapThumbImage(string ImagePath)
        {
            if (string.IsNullOrEmpty(ImagePath))
                return null;
            var uri = new Uri(ImagePath);
            if (uri.IsFile && !File.Exists(uri.LocalPath))
                return null;
            return PathToBitmapThumbImage(uri);
        }
        public static BitmapImage PathToBitmapThumbImage(Uri ImagePath)
        {
            BitmapImage bitMapImage = new BitmapImage();
            if (ImagePath.IsFile && File.Exists(ImagePath.LocalPath))
            {
                BinaryReader binReader = new BinaryReader(File.Open(ImagePath.LocalPath, FileMode.Open));
                FileInfo fileInfo = new FileInfo(ImagePath.LocalPath);
                byte[] bytes = binReader.ReadBytes((int)fileInfo.Length);
                binReader.Close();
                bitMapImage.BeginInit();
                bitMapImage.CacheOption = BitmapCacheOption.OnLoad; //增加这一行
                bitMapImage.StreamSource = new MemoryStream(bytes);
                //   bitMapImage.Freeze();
                bitMapImage.EndInit();

            }
            else
            {

                bitMapImage.CreateOptions = BitmapCreateOptions.IgnoreColorProfile;
                bitMapImage.BeginInit();

                bitMapImage.DecodePixelWidth = 200;
                bitMapImage.CacheOption = BitmapCacheOption.None; //增加这一行
                bitMapImage.UriSource = ImagePath;
                //bitMapImage.Freeze();
                bitMapImage.EndInit();
            }

            //bitMapImage.Freeze();
            return bitMapImage;
        }

        public static BitmapImage PathToBitmapImage(string ImagePath)
        {
            //ImagePath = ImagePath.Replace("//", "\\").Replace("/", "\\");
            var uri = new Uri(ImagePath);
            if (uri.IsFile && !File.Exists(uri.LocalPath))
                return null;
            return PathToBitmapImage(uri);
        }
        public static BitmapImage PathToBitmapImage(Uri ImagePath)
        {
            BitmapImage bitMapImage = new BitmapImage();
            if (ImagePath.IsFile && File.Exists(ImagePath.LocalPath))
            {
                BinaryReader binReader = new BinaryReader(File.Open(ImagePath.LocalPath, FileMode.Open));
                FileInfo fileInfo = new FileInfo(ImagePath.LocalPath);
                byte[] bytes = binReader.ReadBytes((int)fileInfo.Length);
                binReader.Close();
                bitMapImage.BeginInit();
                bitMapImage.CacheOption = BitmapCacheOption.OnLoad; //增加这一行
                bitMapImage.StreamSource = new MemoryStream(bytes);
                bitMapImage.EndInit();
            }
            else
            {
                bitMapImage.CreateOptions = BitmapCreateOptions.IgnoreColorProfile;
                bitMapImage.BeginInit();
                bitMapImage.CacheOption = BitmapCacheOption.OnLoad; //增加这一行
                bitMapImage.UriSource = ImagePath;
                bitMapImage.EndInit();
            }


            return bitMapImage;
        }


        public static BitmapImage byteToBitmapImage(byte[] imageData)
        {
            return byteToBitmapImage(imageData, 0, 0);
        }
        public static BitmapImage byteToBitmapImage(byte[] imageData, int Width, int Height)
        {
            if (imageData == null || imageData.Length == 0) return null;
            var image = new BitmapImage();
            using (var mem = new MemoryStream(imageData))
            {
                mem.Position = 0;
                image.BeginInit();
                image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                image.CacheOption = BitmapCacheOption.OnLoad;
                if (Width > 0)
                    image.DecodePixelWidth = Width;
                if (Height > 0)
                    image.DecodePixelHeight = Height;
                image.UriSource = null;
                image.StreamSource = mem;
                image.EndInit();
            }
            image.Freeze();
            return image;
        }
        public static void SaveBitmap(MimeType mineType, BitmapSource source, string savePath)
        {
            SaveBitmap(mineType, BitmapFrame.Create(source), savePath);
        }
        public static void SaveBitmap(MimeType mineType, Stream BitmapStream, string savePath)
        {
            SaveBitmap(mineType, BitmapFrame.Create(BitmapStream), savePath);
        }
        public static void SaveBitmap(MimeType mineType, BitmapFrame bitmapFrame, string savePath)
        {
            string directoryPath = System.IO.Path.GetDirectoryName(savePath);
            if (!Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);
            BitmapEncoder encoder = null;
            if (mineType == MimeType.image_jpeg)
            {
                encoder = new JpegBitmapEncoder();
            }
            else
            {
                encoder = new PngBitmapEncoder();
                (encoder as PngBitmapEncoder).Interlace = PngInterlaceOption.On;
            }
            encoder.Frames.Add(bitmapFrame);
            using (FileStream fileStream = new FileStream(savePath, FileMode.Create, FileAccess.ReadWrite))
            {
                encoder.Save(fileStream);
            }
        }
        public static void CreateBitmapComponent(string BgImagePath, string title, string savePath)
        {
            BitmapImage bgImage = new BitmapImage();
            bgImage.BeginInit();
            bgImage.UriSource = new Uri(BgImagePath);
            bgImage.EndInit();
            //BitmapImage bgImage = null;
            //try
            //{
            //    BinaryReader br = new BinaryReader(File.Open(BgImagePath, FileMode.Open));
            //    FileInfo fi = new FileInfo(BgImagePath);
            //    Byte[] bytes = br.ReadBytes((int)fi.Length);
            //    br.Close();
            //    bgImage = new BitmapImage();
            //    bgImage.BeginInit();
            //    //bgImage.CacheOption = BitmapCacheOption.OnLoad;
            //    bgImage.StreamSource = new MemoryStream(bytes);
            //    bgImage.EndInit();
            //}
            //catch (Exception e)
            //{
            //    MessageBox.Show("读取图片发生错误！");
            //}

            CreateBitmap(bgImage, title, savePath);
        }
        public static void CreateBitmap(string RelativeBgImagePath, string title, string savePath)
        {
            //获取背景图
            BitmapSource bgImage = new BitmapImage(new Uri(RelativeBgImagePath, UriKind.Relative));
            CreateBitmap(bgImage, title, savePath);
        }
        public static void CreateBitmap(BitmapSource bgImage, string title, string savePath)
        {
            //创建一个RenderTargetBitmap 对象，将WPF中的Visual对象输出
            RenderTargetBitmap composeImage = new RenderTargetBitmap(bgImage.PixelWidth, bgImage.PixelHeight, bgImage.DpiX, bgImage.DpiY, PixelFormats.Default);

            FormattedText signatureTxt = new FormattedText(title,
                                                   System.Globalization.CultureInfo.CurrentCulture,
                                                   System.Windows.FlowDirection.LeftToRight,
                                                   new Typeface(System.Windows.SystemFonts.MessageFontFamily, FontStyles.Normal, FontWeights.Normal, FontStretches.Normal),
                                                   14,
                                                   System.Windows.Media.Brushes.Black);
            signatureTxt.MaxTextWidth = 110;

            DrawingVisual drawingVisual = new DrawingVisual();
            DrawingContext drawingContext = drawingVisual.RenderOpen();
            drawingContext.DrawImage(bgImage, new Rect(0, 0, bgImage.Width, bgImage.Height));
            ////计算签名的位置
            //double x2 = (bgImage.Width / 2 - signatureTxt.Width) / 2;
            //double y2 = 10;
            drawingContext.DrawText(signatureTxt, new System.Windows.Point(15, 15));
            drawingContext.Close();
            composeImage.Render(drawingVisual);
            //定义一个JPG编码器
            JpegBitmapEncoder bitmapEncoder = new JpegBitmapEncoder();
            //加入第一帧
            bitmapEncoder.Frames.Add(BitmapFrame.Create(composeImage));
            string directoryPath = System.IO.Path.GetDirectoryName(savePath);
            if (!Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);
            //保存至文件（不会修改源文件，将修改后的图片保存至程序目录下）
            using (FileStream fileStream = new FileStream(savePath, FileMode.Create, FileAccess.ReadWrite))
            {
                bitmapEncoder.Save(fileStream);
            }
        }
        public enum MimeType
        {
            /// <summary>
            /// image/jpeg
            /// </summary>
            image_jpeg = 1,
            /// <summary>
            /// image/png
            /// </summary>
            image_png = 2
        }
    }
}
