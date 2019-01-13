using DIP.Models;

namespace DIP.Core
{
    public interface IFilter
    {
        PGMImage Apply(PGMImage image, int matrixSize);
    }
}
