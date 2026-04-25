using System.Xml.Serialization;

namespace AssemblyEngine.UI
{
    [Serializable]
    [XmlType(Namespace = "https://thedevassembly.com")]
    public class RectTransform
    {
        [XmlElement("position")]
        public UIVector2I position;

        [XmlElement("size")]
        public UIVector2I size;

        [XmlElement("anchor")]
        public UIVector2 anchor;

        public RectTransform() { }
    }
}