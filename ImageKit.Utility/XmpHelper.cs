using XmpCore;

namespace ImageKit.Utility
{
    public class XmpHelper
    {
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        public int? Rating { get; set; } = null;
        public DateTime? Date { get; set; } = null;
        public XmpHelper? GetXmpData(string filePath) //, int? defaultRating = null
        {
            // Ensure the file exists
            filePath = Path.ChangeExtension(filePath, ".xmp");

            if (!File.Exists(filePath))
            {
                //throw new FileNotFoundException("File not found.", filePath);
                return null;
            }

            // Open the .xmp file and parse its content
            IXmpMeta xmp;
            using (var stream = File.OpenRead(filePath))
                xmp = XmpMetaFactory.Parse(stream);

            // Iterate through the properties to find the "Rating" property
            foreach (var property in xmp.Properties)
            {
                if (property.Path != null)
                {
                    switch (property.Path)
                    {
                        case "xmp:Rating":
                            // Attempt to parse the property value as an integer
                            if (int.TryParse(property.Value, out int rating))
                            {
                                Rating = rating;
                                //defaultRating = rating; // Return the parsed integer value
                            }
                            break;
                        case "dc:title[1]":
                            Title = property.Value;
                            break;
                        case "dc:description[1]":
                            Description = property.Value;
                            break;
                        case "xmp:MetadataDate":
                            DateTime.TryParse(property.Value, out DateTime date);
                            Date = date;
                            break;
                    }
                }

                // Return a default value or message if the "Rating" property is not found

            }
            return this;
        }

        public static int? GetXmpRating(string filePath, int? defaultRating  = null)
        {
            // Ensure the file exists
            filePath = Path.ChangeExtension(filePath, ".xmp");

            if (!File.Exists(filePath))
            {
                //throw new FileNotFoundException("File not found.", filePath);
                return defaultRating;
            }

            // Open the .xmp file and parse its content
            IXmpMeta xmp;
            using (var stream = File.OpenRead(filePath))
                xmp = XmpMetaFactory.Parse(stream);

            // Iterate through the properties to find the "Rating" property
            foreach (var property in xmp.Properties)
            {
                if (property.Path != null && property.Path == "xmp:Rating")
                {
                    // Attempt to parse the property value as an integer
                    if (int.TryParse(property.Value, out int rating))
                    {
                        return rating;
                    }
                    break;
                }
            }
            return defaultRating;
        }

    }
}