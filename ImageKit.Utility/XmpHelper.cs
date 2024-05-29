using XmpCore;

namespace ImageKit.Utility
{
    public class XmpHelper
    {
        public static int? GetRating(string filePath,int? defaultRating=null)
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
                if (property.Path != null && (property.Path == "xmp:Rating"))
                {
                    // Attempt to parse the property value as an integer
                    if (int.TryParse(property.Value, out int rating))
                    {
                        return rating; // Return the parsed integer value
                    }
                    else
                    {
                        break; // Break the loop if parsing fails (assuming there's only one "Rating" property)
                    }
                }
            }

            // Return a default value or message if the "Rating" property is not found
            return defaultRating;
        }
    }
}
