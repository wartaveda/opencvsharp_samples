﻿using OpenCvSharp;
using SampleBase;

namespace SamplesLegacy
{
    /// <summary>
    /// Histogram sample
    /// http://book.mynavi.jp/support/pc/opencv2/c3/opencv_img.html
    /// </summary>
    class HistSample : ConsoleTestBase
    {
        public override void RunTest()
        {
            using var src = Cv2.ImRead(ImagePath.Lenna, ImreadModes.Grayscale);

            // Histogram view
            const int Width = 260, Height = 200;
            using var render = new Mat(new Size(Width, Height), MatType.CV_8UC3, Scalar.All(255));

            // Calculate histogram
            var hist = new Mat();
            int[] hdims = {256}; // Histogram size for each dimension
            Rangef[] ranges = { new Rangef(0,256), }; // min/max 
            Cv2.CalcHist(
                new Mat[]{src}, 
                new int[]{0}, 
                null,
                hist, 
                1, 
                hdims, 
                ranges);
  
            // Get the max value of histogram
            Cv2.MinMaxLoc(hist, out _, out double maxVal);

            var color = Scalar.All(100);
            // Scales and draws histogram
            hist = hist * (maxVal != 0 ? Height / maxVal : 0.0);
            for (int j = 0; j < hdims[0]; ++j)
            {
                int binW = (int)((double)Width / hdims[0]);
                render.Rectangle(
                    new Point(j * binW, render.Rows - (int)hist.Get<float>(j)),
                    new Point((j + 1) * binW, render.Rows),
                    color, 
                    -1);
            }

            using (new Window("Image", src, WindowFlags.AutoSize | WindowFlags.FreeRatio))
            using (new Window("Histogram", render, WindowFlags.AutoSize | WindowFlags.FreeRatio))
            {
                Cv2.WaitKey();
            }
        }
    }
}