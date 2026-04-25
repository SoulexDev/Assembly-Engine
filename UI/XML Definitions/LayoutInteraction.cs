using System.Xml.Serialization;

namespace AssemblyEngine.UI
{
    //[Serializable]
    //[XmlType("layoutInteraction", Namespace = "https://thedevassembly.com")]
    public enum LayoutInteraction
    {
        [XmlEnum("fit")] Fit,
        [XmlEnum("stack")] Stack,
        [XmlEnum("stack-horizontal")] StackHorizontal,
        [XmlEnum("stack-vertical")] StackVertical,
        [XmlEnum("ignore")] Ignore
    }
}