using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DIP.Models;

namespace DIP.Core.Filters
{
    public class Minimum : IFilter
    {
        /// <summary>
        /// http://www.roborealm.com/help/Min.php
        /// </summary>
        /// <param name="image"></param>
        /// <param name="matrixSize"></param>
        /// <returns></returns>
        public PGMImage Apply(PGMImage image, int matrixSize)
        {
            // The reason of Clone is because the algorithm doesn't process the first rows/columns
            // depending on matrix size and if we don't copy them from the original array,
            // there will be a little black border
            int[] temp = (int[])image.Pixels.Clone();

            int offset = (matrixSize - 1) / 2;
            
            Parallel.For(offset, image.Height - offset, (j) =>
            {
                for (int i = offset; i < image.Width - offset; i++)
                {
                    List<int> neighboursNumbers = (from x in Enumerable.Range(i - offset, matrixSize)
                                                   from y in Enumerable.Range(j - offset, matrixSize)
                                                   where (x >= 0) && (x < image.Width) && (y >= 0) && (y < image.Height)
                                                   select image.GetPixelByPos(x, y)).ToList();

                    neighboursNumbers.Sort();

                    int minIndex = neighboursNumbers[0];
                    temp[j * image.Width + i] = minIndex;
                }
            });

            image.Pixels = temp;

            return image;
        }
    }
}
