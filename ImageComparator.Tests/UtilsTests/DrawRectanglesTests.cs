using System.Drawing;
using System.Drawing.Imaging;
using ImageComparator.Utils;

namespace ImageComparator.Tests.UtilsTests;


public class DrawRectanglesTests
{


    private unsafe void isValid(List<List<int>> matrix, int bytesPerPixel, BitmapData image, List<int> color)
    {
        /// Accept matrix like < <0,0,1>, <0,1,0>, <1,0,0> > which represent
        /// +--x+
        /// |-x-|
        /// +x--| to check filled with color
        
        byte* ptr = (byte*)image.Scan0;
        var stride =image.Stride;
        for (int y = 0; y < matrix.Count; y++)
        {
            for (int x = 0; x < matrix[y].Count; x++)
            {
                if (matrix[y][x] == 1)
                {
                    var point = ptr + y * stride + x * bytesPerPixel;
                    for (int c = 0; c < color.Count; c++)
                    {
                        Assert.That(point[c], Is.EqualTo(color[2-c]));
                    }
                }
            }
        }
    }
    
    [Test]
    public unsafe void DrawBorder_DefaultColor_DrawsBorder()
    {
        var Bm=BitmapFixture.CreateSolidColorBitmap(5, 5, Color.Black);
        int bpp = Image.GetPixelFormatSize(Bm.PixelFormat) / 8;
        BitmapData? data = null; 
        try
        {
            data =BitmapLockBits.Lock(Bm, ImageLockMode.ReadWrite);
            DrawRectangles.DrawBorder((byte*)data.Scan0.ToPointer(), 1, 1, 3, 3, data.Stride, bpp);
            
            var matrix = new List<List<int>>()
            {
                new List<int> { 0, 0, 0, 0, 0 },
                new List<int> { 0, 1, 1, 1, 0 },
                new List<int> { 0, 1, 0, 1, 0 },
                new List<int> { 0, 1, 1, 1, 0 },
                new List<int> { 0, 0, 0, 0, 0 },
            };
            isValid(matrix,bpp,data,new List<int>(){255,0,0});   
        }
        finally
        {
            if (data != null) {Bm.UnlockBits(data);}
        }
    }
}