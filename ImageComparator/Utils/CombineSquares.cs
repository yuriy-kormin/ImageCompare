
namespace ImageComparator.Utils
{
    public unsafe class CombineSquares
    {
        public static List<(int x1, int y1, int x2, int y2)> Squares { get; private set; } =
            new List<(int, int, int, int)>();
    
    
        public static void AddOrExtend(int x1, int y1, int x2, int y2)
        {
            var horizontalConnect = Squares.FirstOrDefault(es => es.x2 == x1 && es.y1 <= y1 && es.y2 >= y2);
            var verticalConnect = Squares.FirstOrDefault(es => es.y2 ==y1 && es.x1 <= x1 && es.x2 >= x2);

            if (horizontalConnect == default && verticalConnect == default)
            {
                Squares.Add((x1, y1, x2, y2));
                return;
            }
            if (horizontalConnect != default)
            {
                x1=horizontalConnect.x1;
                y1=horizontalConnect.y1;
                y2 = Math.Max(horizontalConnect.y2, y2);
                Squares.Remove(horizontalConnect);
            }
            else if (verticalConnect != default)
            {
                x1 = Math.Min(x1, verticalConnect.x1);
                y1 = verticalConnect.y1;
                x2 = Math.Max(x2, horizontalConnect.x2);
                Squares.Remove(verticalConnect);
            }
            Squares.Add((x1, y1, x2, y2));
        }
    }    
}
