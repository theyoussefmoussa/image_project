# Image Processing Application (OpenCV C#)

## Project Overview
This is a WinForms-based image processing application using a C# wrapper for **OpenCV**. It allows users to perform basic image manipulation, color channel analysis, and filter applications in a multi-window environment.

## Core Features

### 1. File Management
* **Open:** Load JPEG and BMP images into the workspace.
* **Clear:** Wipes the current workspace and releases system memory (RAM) used by the image pointers.

### 2. Image Filters
* **Brightness:** Adjust image lighting via a TrackBar. Includes **Confirm/Cancel** logic to preview changes before applying them to the main image.
* **Contrast:** Enhance or reduce the color difference in the image.
* **Negative:** Inverts image colors.
* **Grey Scale:** Converts BGR images to 8-bit grayscale in a separate window.
* **Histogram Equalization:** Improves image contrast by stretching the intensity range.

### 3. Color Analysis (RGB Separation)
* **RGB Split:** Launches a dedicated form (**RGBForm**) that uses `unsafe` C# code to manipulate image memory. It extracts the Red, Green, and Blue channels and displays them in individual picture boxes.

## Technical Implementation Details
* **Memory Management:** Uses `cvlib.CvReleaseImage` to prevent memory leaks when clearing or swapping images.
* **Inter-Form Communication:** Pass `IplImage` pointers between the main dashboard and sub-forms (Brightness, RGB, etc.) using custom constructors.
* **Pointer Manipulation:** Employs `unsafe` blocks and `IntPtr` arithmetic to access raw pixel data for high-performance processing.
* **UI Stability:** Implements parameterless constructors and `DialogResult` logic to ensure compatibility with the Visual Studio WinForms Designer.

## How to Run
1. Ensure the OpenCV DLLs are in the project's build directory.
2. Set the solution platform to **x86** to match the OpenCV wrapper architecture.
3. Build and run the solution.
