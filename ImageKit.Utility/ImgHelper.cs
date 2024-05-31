using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageKit.Utility
{
    public class ImgHelper
    {
        /// <summary>
        /// Move extended files like .txt and .xmp
        /// </summary>
        /// <param name="oldPath"></param>
        /// <param name="newPath"></param>
        public static void MoveExtFiles(string oldPath, string newPath)
        {
            MoveExtFile(oldPath, newPath, ".txt");
            MoveExtFile(oldPath, newPath, ".xmp");
        }

        public static void DelExtFiles(string oldPath)
        {
            DelExtFile(oldPath, ".txt");
            DelExtFile(oldPath, ".xmp");
        }

        private static void MoveExtFile(string oldPath, string newPath, string ext)
        {
            oldPath = Path.ChangeExtension(oldPath, ext);
            if (File.Exists(oldPath))
            {
                newPath = Path.ChangeExtension(newPath, ext);
                File.Move(oldPath, newPath);
            }
        }


        private static void DelExtFile(string oldPath, string ext)
        {
            oldPath = Path.ChangeExtension(oldPath, ext);
            if (File.Exists(oldPath))
            {
                File.Delete(oldPath);
            }
        }
    }
}
