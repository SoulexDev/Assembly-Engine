using System.Xml.Serialization;

namespace AssemblyEngine.UI
{
    [XmlType("Vector2")]
    public class UIVector2
    {
        [XmlElement("x")]
        public int x;

        [XmlElement("y")]
        public int y;
    }
}