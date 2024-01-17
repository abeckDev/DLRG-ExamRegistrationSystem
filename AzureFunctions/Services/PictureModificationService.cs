using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Drawing;

namespace AbeckDev.DLRG.ExamRegistration.Functions.Services
{
    public static class PictureModificationService
    {
        public static Stream RemoveExifData(Stream originalStream)
        {
            originalStream.Position = 0;
            var originalImage = new Bitmap(originalStream);

            // Create new image with the same dimensions
            var newImage = new Bitmap(originalImage.Width, originalImage.Height);

            // Create a graphics object from the new image, and set its properties
            using (var graphics = Graphics.FromImage(newImage))
            {
                // Setting these values ensures higher quality of the resulting image
                graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

                // Draw the original image onto the new image. In this case, we're adjusting the size to be exactly the same as the new image
                graphics.DrawImage(originalImage, 0, 0, newImage.Width, newImage.Height);
            }

            // Save the new image to a stream
            var cleanStream = new MemoryStream();
            newImage.Save(cleanStream, ImageFormat.Jpeg); // Or whichever format you want. 

            // Reset stream position
            cleanStream.Position = 0;

            return cleanStream;
        }
    }

}
