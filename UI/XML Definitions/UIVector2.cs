using OpenTK.Mathematics;
using System.Xml.Serialization;

namespace AssemblyEngine.UI
{
    [Serializable]
    [XmlType("Vector2", Namespace = "https://thedevassembly.com")]
    public class UIVector2
    {
        [XmlAttribute("x")]
        public float x;

        [XmlAttribute("y")]
        public float y;

        public UIVector2() { }
        public UIVector2(float x, float y)
        {
            this.x = x;
            this.y = y;
        }
        public static explicit operator Vector2(UIVector2 u) => new Vector2(u.x, u.y);
        public static explicit operator UIVector2(Vector2 v) => new UIVector2(v.X, v.Y);
        public static explicit operator Vector2i(UIVector2 u) => new Vector2i((int)u.x, (int)u.y);
        public static explicit operator UIVector2(Vector2i v) => new UIVector2(v.X, v.Y);
        public override string ToString()
        {
            return $"({x}, {y})";
        }
    }
}