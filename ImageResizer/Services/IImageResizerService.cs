using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageResizer.Services
{
    public interface IImageResizerService
    {
        Stream ResizeImage(Stream stream, int width, int height);
    }
}
