using DIP.Models;
using System;
using System.IO;
using System.Linq;

namespace DIP.Common
{
    public static class PGMParser
    {
        /// <summary>
        /// Loads PGM image from a file.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static PGMImage LoadFromFile(string path)
        {
            PGMImage image = new PGMImage();

            // http://cc.davelozinski.com/c-sharp/fastest-way-to-read-text-files
            using (StreamReader reader = File.OpenText(path))
            {
                string line = string.Empty;

                while ((line = reader.ReadLine()) != null)
                {
                    if (string.IsNullOrEmpty(line) || line.StartsWith("#"))
                    {
                        continue;
                    }

                    // Read format
                    if (image.Format == null)
                    {
                        if (line.Trim() != "P2")
                        {
                            throw new PGMFormatException("Unsupported file format.");
                        }

                        image.Format = line;
                        continue;
                    }

                    // Read width and height
                    string[] widthAndHeight = line.Split(new char[0], StringSplitOptions.RemoveEmptyEntries);
                    if (image.Width == 0 || image.Height == 0)
                    {
                        image.Width = Convert.ToInt32(widthAndHeight[0]);
                        image.Height = Convert.ToInt32(widthAndHeight[1]);
                        continue;
                    }

                    // Read max gray value
                    if (image.MaxGrayLevel == 0)
                    {
                        image.MaxGrayLevel = Convert.ToInt32(line);
                        break;
                    }
                }

                // Validate metadata
                if (!image.HasValidMetadata)
                {
                    throw new PGMFormatException("Metadata could not be read or it is invalid.");
                }

                // Read pixel data
                image.Pixels = reader.ReadToEnd()
                    .Split(new char[0], StringSplitOptions.RemoveEmptyEntries) // split by whitespace and remove empty entries
                    .Select(e => Convert.ToInt32(e))
                    .ToArray();
            }
            
            return image;
        }
        
        /// <summary>
        /// Saves PGM image.
        /// </summary>
        /// <param name="image"></param>
        /// <param name="path"></param>
        public static void Save(this PGMImage image, string path)
        {
            using (StreamWriter writer = new StreamWriter(path))
            {
                // Writing format
                writer.WriteLine(image.Format);

                // Writing width and height
                writer.WriteLine($"{image.Width} {image.Height}");

                // Writing max gray level
                writer.WriteLine(image.MaxGrayLevel);

                // Writing pixel data
                string pixelLine = string.Empty;

                for (int i = 0; i < image.Pixels.Length; i++)
                {
                    string currentPixel = $"{image.Pixels[i]} ";

                    if (pixelLine.Length + currentPixel.Length >= 70) // 70 is line's max length
                    {
                        writer.WriteLine(pixelLine);
                        pixelLine = currentPixel;
                    }
                    else
                    {
                        pixelLine += currentPixel;
                    }
                }

                if (pixelLine.Length > 0)
                {
                    writer.WriteLine(pixelLine);
                }
            }
        }
    }
}
