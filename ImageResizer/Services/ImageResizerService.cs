using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageResizer.Services
{
    public class ImageResizerService : IImageResizerService
    {

        public Stream ResizeImage(Stream stream, int width, int height)
        {
            Image img = Image.FromStream(stream);
            Bitmap b = new Bitmap(width, height);
            using (Graphics g = Graphics.FromImage((Image)b))
            {
                g.DrawImage(img, 0, 0, width, height);
            }

            var output = new MemoryStream();
            b.Save(output, ImageFormat.Jpeg);
            return output;
        }


    }
}
