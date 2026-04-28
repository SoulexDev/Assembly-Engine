using AssemblyEngine.Graphics;
using OpenTK.Mathematics;
using System.Xml.Serialization;

namespace AssemblyEngine.UI
{
    [Serializable]
    [XmlType(Namespace = "https://thedevassembly.com")]
    public class Group
    {
        //[XmlIgnore]
        //private bool isDirty = true;

        [XmlAttribute("id")]
        public string id;

        [XmlElement("position")]
        public UIVector2I internalPosition;

        [XmlElement("size")]
        public UIVector2I internalSize;

        //[XmlIgnore]
        //private UIVector2 _anchor;

        [XmlElement("anchor")]
        public UIVector2 anchor;

        //[XmlIgnore]
        //private Vector2i _position;

        [XmlIgnore]
        public Vector2i position;

        //[XmlIgnore]
        //private Vector2i _size;

        [XmlIgnore]
        public Vector2i size;

        //[XmlIgnore]
        //private LayoutInteraction _layoutInteraction;

        [XmlElement("layoutInteraction")]
        public LayoutInteraction layoutInteraction;

        //[XmlIgnore]
        //private LayoutType _layoutType;

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
        public List<Group> children = new List<Group>();

        public Group() { }

        //public void SetDirty()
        //{
        //    isDirty = true;
        //}
    }
}