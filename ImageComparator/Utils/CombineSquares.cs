
using System.Drawing;

namespace ImageComparator.Utils
{
    public unsafe class CombineSquares
    {
        public static List<Rectangle> Squares { get; private set; }
    
        public static void AddOrExtend(Rectangle rect)
        {
            var touchingSquares = Squares.Where(s => 
                    s.Right >= rect.Left && s.Left <= rect.Right 
                    && s.Bottom >= rect.Top && s.Top <= rect.Bottom 
            ).ToList();
            
            if (touchingSquares.Any())
            {
                x1 = Math.Min(rect.Left, touchingSquares.Min(s => s.Left));
                y1 = Math.Min(rect.Top, touchingSquares.Min(s => s.Top));
                x2 = Math.Max(rect.Right, touchingSquares.Max(s => s.Right));
                y2 = Math.Max(rect.Bottom, touchingSquares.Max(s => s.Bottom));
            
                Squares.RemoveAll(s => touchingSquares.Contains(s));
            }
            Squares.Add((x1, y1, x2, y2));
            
        }
    }    
}
