
public class Draw
{
    private static unsafe void SetRed(byte* resPtr, int address,List<int>? color= null)
    {
        if (color == null)
        {
            color = new List<int>(){0,0,255};
        }
        resPtr[address] = (byte)color[0];
        resPtr[address+1] = (byte)color[1];
        resPtr[address+2] = (byte)color[2];
    }
    
    public static unsafe void DrawRedBorder(byte* resPtr, int x, int y, int width, int height, int stride, int bytesPerPixel)
    {
        // Top and Bottom borders
        for (int i = 0; i < width; i++)
        {
            int topOffset = (y * stride) + ((x + i) * bytesPerPixel);
            int bottomOffset = ((y + height - 1) * stride) + ((x + i) * bytesPerPixel);
            
            SetRed(resPtr, topOffset);
            SetRed(resPtr, bottomOffset);
        }

        // Left and Right borders
        for (int j = 0; j < height; j++)
        {
            int leftOffset = ((y + j) * stride) + (x * bytesPerPixel);
            int rightOffset = ((y + j) * stride) + ((x + width - 1) * bytesPerPixel);
            
            SetRed(resPtr, leftOffset);
            SetRed(resPtr, rightOffset);
        }
    }
}