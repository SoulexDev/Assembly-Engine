using System.Xml;
using System.Xml.Schema;

namespace AssemblyEngine.UI
{
    public class ASXMLCanvas
    {
        private Group rootGroup;

        public void LoadFromXML(string xmlPath)
        {
            Console.WriteLine($"Loading ASXML: {xmlPath}");

            XmlReaderSettings settings = new XmlReaderSettings();

            using (XmlReader XSDReader = XmlReader.Create("ui/ASXML.xsd"))
            {
                XmlSchema schema = XmlSchema.Read(XSDReader, ValidationCallback);
                settings.Schemas.Add(schema);

                using (XmlReader ASXMLReader = XmlReader.Create(xmlPath))
                {
                    if (ASXMLReader.Read())
                    {
                        
                    }
                }
            }
        }
        private static void ValidationCallback(object sender, ValidationEventArgs args)
        {
            if (args.Severity == XmlSeverityType.Warning)
                Console.Write("WARNING: ");
            else if (args.Severity == XmlSeverityType.Error)
                Console.Write("ERROR: ");

            Console.WriteLine(args.Message);
        }
        public void Draw()
        {

        }
    }
}
