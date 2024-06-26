﻿using Diffusion.Database;
using ImageKit.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diffusion.Toolkit.Common
{
    public static class ImageUtil
    {
        /// <summary>
        /// Populate ImageEntry with XmpHelper Data
        /// </summary>
        /// <param name="xd"></param>
        /// <returns></returns>
        public static ImageEntry PopulateImageEntry(XmpHelper? xd, ImageEntry image)
        {
            if (xd != null)
            {
                if (xd.Rating != null)
                {
                    if (xd.Rating == -1)
                    {
                        image.ForDeletion = true; //rejects
                    }
                    else
                    {
                        image.Rating = xd.Rating;
                    }
                }
                if (xd.Label != null)
                {
                    image.Label = xd.Label;
                }
            }
            return image;
        }

        public static Image? PopulateImage(XmpHelper? xd, Image? image)
        {
            if (xd != null && image != null)
            {
                if (xd.Rating != null)
                {
                    if (xd.Rating == -1)
                    {
                        image.ForDeletion = true; //rejects
                    }
                    else
                    {
                        image.Rating = xd.Rating;
                    }
                }
                //if (xd.Label != null)
                //{
                image.CustomTags = xd.Label;
                //}
            }
            return image;
        }



    }
}
