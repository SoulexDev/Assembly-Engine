using AssemblyEngine.Graphics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace AssemblyEngine.UI
{
    [Serializable]
    [XmlRoot("ASXML", Namespace = "https://thedevassembly.com")]
    public class ASXMLCanvas
    {
        [XmlIgnore]
        public RectTransform canvasRect;

        [XmlElement("Group")]
        public List<Group> rootGroups = new List<Group>();

        public ASXMLCanvas()
        {
            canvasRect = new RectTransform()
            {
                position = new UIVector2I(0, 0),
                size = new UIVector2I(1920, 1080),
                anchor = new UIVector2(0, 0)
            };
        }
        public static ASXMLCanvas LoadFromXML(string xmlPath)
        {
            xmlPath = Path.Combine(Core.AssetsPath, xmlPath);
            string xsdPath = Path.Combine(Core.AssetsPath, "ui/ASXML.xsd");

            Console.WriteLine($"Loading ASXML: {xmlPath}");

            ASXMLCanvas canvas;

            XmlSerializer serializer = new XmlSerializer(typeof(ASXMLCanvas));

            using (XmlReader xsdReader = XmlReader.Create(xsdPath))
            {
                XmlSchema schema = XmlSchema.Read(xsdReader, ValidationCallback);

                XmlReaderSettings settings = new XmlReaderSettings();
                settings.ValidationType = ValidationType.Schema;
                settings.ValidationFlags = XmlSchemaValidationFlags.ReportValidationWarnings;
                settings.Schemas.Add(schema);

                using (XmlReader xmlReader = XmlReader.Create(xmlPath, settings))
                {
                    try
                    {
                        canvas = (ASXMLCanvas)serializer.Deserialize(xmlReader);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"Error loading XML: {e}");
                        canvas = null;
                    }
                }
            }
            return canvas;
        }
        private static void ValidationCallback(object sender, ValidationEventArgs args)
        {
            if (args.Severity == XmlSeverityType.Warning)
                Console.Write("WARNING: ");
            else if (args.Severity == XmlSeverityType.Error)
                Console.Write("ERROR: ");

            Console.WriteLine(args.Message);
        }
        public void Draw()
        {
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

            Core.defaultUIShader.Use();
            Core.defaultUIShader.SetVector("uCanvasSize", (Vector2i)canvasRect.size);

            GL.BindVertexArray(FullscreenQuadMesh.mesh.vao);

            foreach (Group rootGroup in rootGroups)
            {
                RecursiveDrawGroup(rootGroup, canvasRect, 0);
            }

            GL.BindVertexArray(0);
            GL.Disable(EnableCap.Blend);
        }
        private void RecursiveDrawGroup(Group group, RectTransform transformedParentRect, int index)
        {
            RectTransform transformedRect = GetTransformedChildRect(index, LayoutType.Inherit, group.layoutInteraction, transformedParentRect, group.rectTransform);

            if (!string.IsNullOrEmpty(group.imagePath))
            {
                Core.defaultUIShader.SetVector("uPosition", (Vector2i)transformedRect.position);
                Core.defaultUIShader.SetVector("uSize", (Vector2i)transformedRect.size);
                Core.defaultUIShader.SetVector("uAnchor", (Vector2i)transformedRect.anchor);
                Core.defaultUIShader.SetColor("uColor", group.color);

                GL.ActiveTexture(TextureUnit.Texture0);
                GL.BindTexture(TextureTarget.Texture2D, group.texture);
                Core.defaultUIShader.SetTexture("uMainTex", 0);

                GL.DrawArrays(PrimitiveType.Triangles, 0, 6);
                GL.BindTexture(TextureTarget.Texture2D, 0);
            }

            float centerX = 0;
            float widthX = 0;

            for (int i = 0; i < group.childGroups.Count; i++)
            {
                Group childGroup = group.childGroups[i];
                if (group.layoutType == LayoutType.CenterHorizontal)
                {
                    widthX += childGroup.rectTransform.size.x;
                }
                //RecursiveDrawGroup(group.childGroups[i], transformedRect, i);
            }
            centerX = widthX / group.childGroups.Count;
        }
        private RectTransform GetTransformedChildRect(int index, LayoutType layoutType, LayoutInteraction layoutInteraction, RectTransform parent, RectTransform child)
        {
            RectTransform returnRect = new RectTransform();

            Vector2i pPosition = (Vector2i)parent.position;
            //Vector2i pSize = (Vector2i)parent.size;
            //Vector2 pAnchor = (Vector2)parent.anchor;

            Vector2i cPosition = (Vector2i)child.position;
            Vector2i cSize = (Vector2i)child.size;
            Vector2 cAnchor = (Vector2)child.anchor;

            Vector2 childPosition = cPosition - cAnchor * cSize;

            returnRect.position = (UIVector2I)(pPosition + childPosition);
            returnRect.size = child.size;
            returnRect.anchor = child.anchor;

            switch (layoutType)
            {
                case LayoutType.Inherit:
                    break;
                case LayoutType.CenterHorizontal:
                    break;
                default:
                    break;
            }

            return returnRect;
        }
        //private Vector2 CanvasSpaceToNDCSpace(Vector2i canvasSize, Vector2i point)
        //{
        //    Vector2 floatPoint = (Vector2)point;
        //    floatPoint /= canvasSize;

        //    floatPoint *= 2;
        //    floatPoint -= Vector2.One;

        //    return floatPoint;
        //}
    }
}