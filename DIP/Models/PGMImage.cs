using DIP.Common;
using System.Drawing;
using System.Drawing.Imaging;

namespace DIP.Models
{
    public class PGMImage
    {
        public string Format { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int MaxGrayLevel { get; set; }
        public int[] Pixels { get; set; }

        public bool HasValidMetadata
        {
            get
            {
                return Width > 0 && Height > 0 && MaxGrayLevel > 0 && !string.IsNullOrWhiteSpace(Format);
            }
        }

        public int GetPixelByPos(int x, int y) 
            => Pixels[y * Width + x];

        public void SetPixelByPos(int x, int y, int pixel) 
            => Pixels[y * Width + x] = pixel;
        
        /// <summary>
        /// Converts PGM image to Bitmap.
        /// </summary>
        /// <returns></returns>
        public unsafe Bitmap ToBitmap()
        {
            Bitmap image = new Bitmap(Width, Height);
            BitmapData bitmapData = image.LockBits(new Rectangle(0, 0, Width, Height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            ColorARGB* startingPosition = (ColorARGB*)bitmapData.Scan0;

            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    double color = GetPixelByPos(x, y);
                    byte rgb = (byte)(color * 255 / MaxGrayLevel);

                    ColorARGB* position = startingPosition + x + y * Width;
                    position->A = 255;
                    position->R = rgb;
                    position->G = rgb;
                    position->B = rgb;
                }
            }

            image.UnlockBits(bitmapData);

            return image;
        }
    }
}
