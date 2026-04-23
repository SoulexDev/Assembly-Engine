using System.Xml.Serialization;

namespace AssemblyEngine.UI
{
    public enum LayoutInteraction
    {
        [XmlEnum("fit")] Fit,
        [XmlEnum("stack")] Stack,
        [XmlEnum("stack-horizontal")] StackHorizontal,
        [XmlEnum("stack-vertical")] StackVertical,
        [XmlEnum("ignore")] Ignore
    }
}