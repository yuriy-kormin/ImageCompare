
namespace ImageComparator.Utils
{
    public unsafe class CombineSquares
    {
        public static List<(int x1, int y1, int x2, int y2)> Squares { get; private set; } =
            new List<(int, int, int, int)>();
    
    
        public static void AddOrExtend(int x1, int y1, int x2, int y2)
        {
            var touchingSquares = Squares.Where(s => 
                    s.x2 >= x1 && s.x1 <= x2 
                    && s.y2 >= y1 && s.y1 <= y2 
            ).ToList();
            
            if (touchingSquares.Any())
            {
                x1 = Math.Min(x1, touchingSquares.Min(s => s.x1));
                y1 = Math.Min(y1, touchingSquares.Min(s => s.y1));
                x2 = Math.Max(x2, touchingSquares.Max(s => s.x2));
                y2 = Math.Max(y2, touchingSquares.Max(s => s.y2));
            
                Squares.RemoveAll(s => touchingSquares.Contains(s));
            }
            Squares.Add((x1, y1, x2, y2));
            
        }
    }    
}
