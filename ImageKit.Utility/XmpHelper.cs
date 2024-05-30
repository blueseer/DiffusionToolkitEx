using System.Xml.Linq;
using XmpCore;
using XmpCore.Options;

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

        public static int? GetXmpRating(string filePath, int? defaultRating = null)
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

        public static void SetXmpRating(string filePath, int? rating)
        {
            IXmpMeta xmp;
            // Load existing XMP metadata from the file
            filePath = Path.ChangeExtension(filePath, ".xmp");
            if (File.Exists(filePath))
            {
                // Load existing XMP metadata from the file
                using (var stream = File.OpenRead(filePath))
                {
                    xmp = XmpMetaFactory.Parse(stream);
                }
            }
            else
            {
                // Create new XMP metadata
                xmp = XmpMetaFactory.Create();
            }

            // Define the XMP namespace URI for XMP Basic (where the Rating property is usually found)
            string xmpBasicNamespace = "http://ns.adobe.com/xap/1.0/";
            // Define the property name for the Rating (adjust as needed based on your XMP structure)
            string ratingPropertyName = "Rating";


            UpdateOrRemoveProperty(xmp, xmpBasicNamespace, ratingPropertyName, rating?.ToString());

            //string dcNamespace = "http://purl.org/dc/elements/1.1/";
            //UpdateOrRemoveText(xmp, dcNamespace, "title", "Test title"); //Working, testing
            //UpdateOrRemoveText(xmp, dcNamespace, "description", "Test my desc\nHello, how are you?"); //Working, testing

            // Serialize the updated XMP metadata to a string
            var serializeOptions = new SerializeOptions { UseCompactFormat = false };
            string updatedXmpString = XmpMetaFactory.SerializeToString(xmp, serializeOptions);

            // Write the updated XMP metadata back to the file
            // IMPORTANT: This example overwrites the original file. Consider creating a backup or working on a copy of the file.
            File.WriteAllText(filePath, updatedXmpString);
        }

        public static void SetXmpRatings(IEnumerable<string> paths, int? rating)
        {
            foreach (var path in paths)
            {
                SetXmpRating((string)path, rating);
            }
        }

        private static void UpdateOrRemoveProperty(IXmpMeta xmp, string namespaceUri, string propertyName, string? value)
        {
            if (value != null)
            {
                xmp.SetProperty(namespaceUri, propertyName, value, null);
            }
            else if (xmp.DoesPropertyExist(namespaceUri, propertyName))
            {
                xmp.DeleteProperty(namespaceUri, propertyName);
            }
        }

        private static void UpdateOrRemoveText(IXmpMeta xmp, string dcNamespace, string propertyName, string? value)
        {
            if (value != null)
            {
                xmp.AppendArrayItem(dcNamespace, propertyName, new PropertyOptions { IsArrayAltText = true }, value, null);
                xmp.SetQualifier(dcNamespace, propertyName + "[1]", XmpConstants.NsXml, "lang", "x-default", null);
            }
            else if (xmp.DoesPropertyExist(dcNamespace, propertyName))
            {
                xmp.DeleteProperty(dcNamespace, propertyName);
            }
        }


#if false // FutureExpansionForWritingTitleAndDescription
        public static void CreateOrUpdateXmpFile(string filePath, int? rating, string title, string description)
        {
            IXmpMeta xmp;

            if (File.Exists(filePath))
            {
                using (var stream = File.OpenRead(filePath))
                {
                    xmp = XmpMetaFactory.Parse(stream);
                }
            }
            else
            {
                xmp = XmpMetaFactory.Create();
            }

            string xmpBasicNamespace = "http://ns.adobe.com/xap/1.0/";

            UpdateOrRemoveProperty(xmp, xmpBasicNamespace, "Rating", rating?.ToString());
            UpdateOrRemoveProperty(xmp, xmpBasicNamespace, "Title", title);
            UpdateOrRemoveProperty(xmp, xmpBasicNamespace, "Description", description);

            var serializeOptions = new SerializeOptions { UseCompactFormat = true };
            string xmpString = XmpMetaFactory.SerializeToString(xmp, serializeOptions);

            File.WriteAllText(filePath, xmpString);
        }
#endif


    }
}