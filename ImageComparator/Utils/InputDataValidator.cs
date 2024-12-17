using System.Drawing;

namespace ImageComparator.Utils
{
    /// <summary>
    /// A utility class to validate input data related to image comparisons.
    /// </summary>
    public class InputDataValidator
    {
        /// <summary>
        /// Checks if two bitmaps have the same dimensions (width and height).
        /// </summary>
        /// <param name="bitmap1">The first bitmap to compare.</param>
        /// <param name="bitmap2">The second bitmap to compare.</param>
        /// <exception cref="Exception">Thrown when the dimensions of the two bitmaps do not match.</exception>
        /// <remarks>
        /// This method compares the width and height of the two bitmaps. 
        /// If they differ, an exception is thrown to indicate that the bitmaps cannot be compared.
        /// </remarks>
        public static void CheckSameDimensions(Bitmap bitmap1, Bitmap bitmap2)
        {
            // Check if the dimensions of the two bitmaps are the same
            if (bitmap1.Width != bitmap2.Width || bitmap1.Height != bitmap2.Height)
                throw new Exception("Bitmaps are different.");
        }
    }    
}