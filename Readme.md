[![CI](https://github.com/yuriy-kormin/ImageCompare/actions/workflows/ci.yml/badge.svg)](https://github.com/yuriy-kormin/ImageCompare/actions/workflows/ci.yml)
[![codecov](https://codecov.io/gh/yuriy-kormin/ImageCompare/graph/badge.svg?token=45FLY0KKSY)](https://codecov.io/gh/yuriy-kormin/ImageCompare)

# ImageComparator
This C# solution provides a library and a CLI utility for recognizing and 
marking differences between two images. The library's functionality includes:
- ##### Image Loading and Processing:
    Images are loaded as bitmap data and preprocessed with a Gaussian Blur filter
to smooth differences and equalize pixel intensities.

- ##### Comparison Logic:
    The images are divided into small squares (default 15x15 pixels). 

    The library compares color brightness between corresponding pixels in the squares.
    
    Squares exceeding a defined difference threshold are added to a list for further processing.

- ##### Result Visualization:
    After detecting differences, red rectangles are drawn on the images to visually highlight the areas of discrepancy.

## Realization Approach
The library uses unsafe methods to directly access pixel data, 
ensuring efficient manipulation of bitmap data. 
Images are preprocessed with a Gaussian Blur filter to reduce noise and enhance 
meaningful differences.

During comparison, a threshold mechanism determines whether pixel differences within 
a square are significant enough to count.

Detected squares are either merged with existing ones or added as new entities to the list. 

After processing, the library iterates through the list to overlay red rectangles on the original images, marking the detected areas.


## CLI Usage Parameters
The CLI utility accepts the following parameters. If not provided, default values are used:

```plaintext

--file1=filename1        // Path to the first image (default: "1.jpg")
--file1=filename2        // Path to the second image (default: "2.jpg")
--threshold=<value>      // Percentage difference threshold [0-100] (default: 10)
--diffcount=<value>      // Maximum difference count per square [-1 (no limit) to 100] (default: -1)
    
--file1, --file2
   Specify the paths of the images to compare. 
      
      Defaults are 1.jpg and 2.jpg.

--threshold<int>[0-100]:
   Sets the percentage threshold for recognizing differences between pixels.
    
      Defaults to 10.

--diffcount<int>[-1-100]:
   Limits the number of difference pixels per square. 
     
     -1 (default) means no limit.
```






