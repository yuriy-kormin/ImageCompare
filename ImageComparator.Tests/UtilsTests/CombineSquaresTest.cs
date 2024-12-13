namespace ImageComparator.Tests.UtilsTests;
using ImageComparator.Utils;

public class CombineSquaresTest
{
    [SetUp]
    public void Setup()
    {
        CombineSquares.Squares.Clear();
    }

    [Test]
    public void CombineSquaresTestSimple()
    {
        //   +_1_2_3_4_5_+
        //   1| _        |
        //   2||_|       |   1 square (1.1,2.2)
        //   3|          |
        //   4|          |
        //   5|          |
        //    +----------+
        CombineSquares.Squares.Add((1,1,2,2));
        
        Assert.That(CombineSquares.Squares.Count, Is.EqualTo(1));
        // add square on right(2.1, 3.2)
        
        CombineSquares.AddOrExtend(2,1,3,2);
        Assert.That(CombineSquares.Squares.Count, Is.EqualTo(1));
        Assert.That(CombineSquares.Squares[0], Is.EqualTo((1,1,3,2)));
        //   +_1_2_3_4_5_+
        //   1| __       |
        //   2||__|      |   1 square (1.1,3.2)
        //   3|          |
        //   4|          |
        //   5|          |
        //    +----------+
        
        // add square on bottom(2.2, 3.3)
        CombineSquares.AddOrExtend(2,2,3,3);
        Assert.That(CombineSquares.Squares.Count, Is.EqualTo(1));
        Assert.That(CombineSquares.Squares[0], Is.EqualTo((1,1,3,3)));
        //   +_1_2_3_4_5_+
        //   1| __       |
        //   2||  |      |   
        //   3||__|      |   1 square (1.1,3.3)
        //   4|          |
        //   5|          |
        //    +----------+

    }
    
}