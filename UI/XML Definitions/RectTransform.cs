using System.Xml.Serialization;

namespace AssemblyEngine.UI
{
    [XmlType("RectTransform")]
    public class RectTransform
    {
        [XmlElement("position")]
        public UIVector2 position;

        [XmlElement("size")]
        public UIVector2 size;

        [XmlElement("anchor")]
        public UIVector2 anchor;
    }
}