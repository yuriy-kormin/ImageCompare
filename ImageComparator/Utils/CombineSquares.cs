namespace ImageComparator.Utils
{
    /// <summary>
    /// A utility class for managing a collection of rectangles (squares) and combining them if they are touching.
    /// </summary>
    public unsafe class CombineSquares
    {
        /// <summary>
        /// A list to store the squares as tuples representing the coordinates of each square (x1, y1, x2, y2).
        /// </summary>
        public static List<(int x1, int y1, int x2, int y2)> Squares { get; private set; } =
            new List<(int, int, int, int)>();

        /// <summary>
        /// Adds a new square or extends an existing one if the given square overlaps or touches any existing square.
        /// </summary>
        /// <param name="x1">The x-coordinate of the top-left corner of the square.</param>
        /// <param name="y1">The y-coordinate of the top-left corner of the square.</param>
        /// <param name="x2">The x-coordinate of the bottom-right corner of the square.</param>
        /// <param name="y2">The y-coordinate of the bottom-right corner of the square.</param>
        /// <remarks>
        /// This method checks if the given square overlaps or touches any existing squares in the list.
        /// If there are any such squares, they will be combined into one larger square and added back to the list.
        /// If no such squares are found, the given square will simply be added to the list.
        /// </remarks>
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
