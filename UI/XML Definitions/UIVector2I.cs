using OpenTK.Mathematics;
using System.Xml.Serialization;

namespace AssemblyEngine.UI
{
    [Serializable]
    [XmlType("Vector2I", Namespace = "https://thedevassembly.com")]
    public class UIVector2I
    {
        [XmlAttribute("x")]
        public int x;

        [XmlAttribute("y")]
        public int y;

        public UIVector2I() { }
        public UIVector2I(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public static explicit operator Vector2i(UIVector2I u) => new Vector2i(u.x, u.y);
        public static explicit operator UIVector2I(Vector2i v) => new UIVector2I(v.X, v.Y);
        public static explicit operator Vector2(UIVector2I u) => new Vector2(u.x, u.y);
        public static explicit operator UIVector2I(Vector2 v) => new UIVector2I((int)v.X, (int)v.Y);

        public override string ToString()
        {
            return $"({x}, {y})";
        }
    }
}