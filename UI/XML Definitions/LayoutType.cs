using System.Xml.Serialization;

namespace AssemblyEngine.UI
{
    //[Serializable]
    //[XmlType("layoutType", Namespace = "https://thedevassembly.com")]
    public enum LayoutType
    {
        [XmlEnum("inherit")] Inherit,
        [XmlEnum("center-horizontal")] CenterHorizontal,
        [XmlEnum("center-vertical")] CenterVertical,
        [XmlEnum("top-horizontal")] TopHorizontal,
        [XmlEnum("top-vertical")] TopVertical,
        [XmlEnum("bottom-horizontal")] BottomHorizontal,
        [XmlEnum("bottom-vertical")] BottomVertical,
        [XmlEnum("right-horizontal")] RightHorizontal,
        [XmlEnum("right-vertical")] RightVertical,
        [XmlEnum("left-horizontal")] LeftHorizontal,
        [XmlEnum("left-vertical")] LeftVertical,
        [XmlEnum("top-right-horizontal")] TopRightHorizontal,
        [XmlEnum("top-right-vertical")] TopRightVertical,
        [XmlEnum("top-left-horizontal")] TopLeftHorizontal,
        [XmlEnum("top-left-vertical")] TopLeftVertical,
        [XmlEnum("bottom-right-horizontal")] BottomRightHorizontal,
        [XmlEnum("bottom-right-vertical")] BottomRightVertical,
        [XmlEnum("bottom-left-horizontal")] BottomLeftHorizontal,
        [XmlEnum("bottom-left-vertical")] BottomLeftVertical,
        [XmlEnum("fill-horizontal")] FillHorizontal,
        [XmlEnum("fill-vertical")] FillVertical,
        [XmlEnum("fill-horizontal-right")] FillHorizontalRight,
        [XmlEnum("fill-vertical-right")] FillVerticalRight,
        [XmlEnum("fill-horizontal-left")] FillHorizontalLeft,
        [XmlEnum("fill-vertical-left")] FillVerticalLeft
    }
}