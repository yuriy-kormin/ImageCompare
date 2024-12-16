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

## ImageComparator DLL


### Overview
The `ImageComparator` library is a .NET DLL designed for pixel-by-pixel image comparison. It accepts three bitmap images as inputs:
1. **Image 1:** First image to compare.
2. **Image 2:** Second image to compare.
3. **Result Image:** Output image to visualize the differences.

### Realization Approach
The library uses unsafe methods to directly access pixel data,
ensuring efficient manipulation of bitmap data.
Images are preprocessed with a Gaussian Blur filter to reduce noise and enhance
meaningful differences.(can be disable in `Settings` module for faster processing)

During comparison, a threshold mechanism determines whether pixel differences within
a square are significant enough to count.

Detected squares are either merged with existing ones or added as new entities to the list.

After processing, the library iterates through the list to overlay red rectangles on the original images, marking the detected areas.



## Parameters
The main parameters for `ImageComparator` can be configured via the `Settings` class. These parameters are:

### Debugging Settings
- **`Debug`** (`bool`):
  - Default: `true`
  - Enables or disables debug mode.

- **`DebugRectShiftPX`** (`int`):
  - Default: `3`
  - Number of pixels to shift the debug rectangles.

- **`DebugRectColor`** (`List<int>`):
  - Default: `[0, 0, 0]` (Black color)
  - RGB color values for the debug rectangles.

### Comparison Settings
- **`applyGaussianBlur`** (`bool`):
  - Default: `false`
  - Applies Gaussian blur to images before comparison.

- **`squareSize`** (`int`):
  - Default: `15`
  - Size of the square blocks used in the comparison process.

- **`PixelCounterPercentageThreshold`** (`int`):
  - Default: `20`
  - Percentage threshold for considering pixel blocks as different.

- **`GaussianSigma`** (`double`):
  - Default: `2.0`
  - Sigma value for Gaussian blur.

- **`PixelBrightPercentageThreshold`** (`int`):
  - Default: `10`
  - Threshold for brightness differences. This value can be overridden by CLI input.

- **`DiffCount`** (`int`):
  - Default: `-1`
  - Number of detected differences. This value can be overridden by CLI input.

### Additional Settings
- **`GrayScaleCompare`** (`bool`):
  - Currently commented out in the source code.
  - Intended for grayscale comparison. Future support may be added.


```csharp
using ImageComparator;

Settings.applyGaussianBlur = true;
Settings.squareSize = 10;
Settings.PixelCounterPercentageThreshold = 15;

// Perform comparison
ImageComparator.Compare(image1, image2, resultImage);
```


