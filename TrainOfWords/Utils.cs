using System;
using System.Drawing;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

namespace TrainOfWords
{
    public static class Utils
    {
        public static BitmapSource ConvertBitmapToBitmapSource(Bitmap bm)
        {
            var bitmap = bm;
            var bitmapSource = Imaging.CreateBitmapSourceFromHBitmap(bitmap.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            bitmap.Dispose();
            return bitmapSource;
        } 
    }
}