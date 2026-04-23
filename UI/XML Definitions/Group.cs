using OpenTK.Mathematics;
using System.Xml.Serialization;

namespace AssemblyEngine.UI
{
    [XmlType("Group")]
    public class Group
    {
        //metadata
        [XmlAttribute("id")]
        public string id { get; private set; }

        //elements
        [XmlElement("layoutType", DataType = "string")]
        public LayoutType layoutType;
        public RectTransform rectTransform;
        public Color4 color;
        public List<Group> childGroups = new List<Group>();
        public List<Image> childImages = new List<Image>();
    }
}