using System.Drawing;
namespace ImageComparator.utils;

public interface IConverter
{
    static Bitmap Process(Bitmap image){return image;}
}