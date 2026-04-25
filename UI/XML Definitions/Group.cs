using AssemblyEngine.Graphics;
using OpenTK.Mathematics;
using System.Xml.Serialization;

namespace AssemblyEngine.UI
{
    [Serializable]
    [XmlType(Namespace = "https://thedevassembly.com")]
    public class Group
    {
        [XmlAttribute("id")]
        public string id;

        [XmlElement("RectTransform")]
        public RectTransform rectTransform;

        [XmlElement("layoutInteraction")]
        public LayoutInteraction layoutInteraction;

        [XmlElement("layoutType")]
        public LayoutType layoutType;

        [XmlIgnore]
        private string _hexColor;

        [XmlElement("color")]
        public string hexColor
        {
            get { return _hexColor; }
            set
            {
                System.Drawing.Color sysColor = System.Drawing.ColorTranslator.FromHtml(value);

                color.R = sysColor.A / 255f;
                color.G = sysColor.R / 255f;
                color.B = sysColor.G / 255f;
                color.A = sysColor.B / 255f;

                _hexColor = value;
            }
        }

        [XmlIgnore]
        public Color4 color;

        [XmlIgnore]
        private string _imagePath;

        [XmlElement("image")]
        public string imagePath
        {
            get { return _imagePath; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    ResourceLoader.LoadResource(out texture, value);
                }
                _imagePath = value;
            }
        }

        [XmlIgnore]
        public Texture2D texture;

        [XmlElement("Group")]
        public List<Group> childGroups = new List<Group>();

        public Group() { }
        public Group(UIVector2I position, UIVector2I size, UIVector2 anchor)
        {
            rectTransform = new RectTransform() { position = position, size = size, anchor = anchor };
        }
    }
}