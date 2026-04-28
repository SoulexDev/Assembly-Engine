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
        private bool isDirty = true;

        [XmlIgnore]
        public Vector2 canvasSize;

        [XmlElement("Group")]
        public List<Group> rootGroups = new List<Group>();

        public ASXMLCanvas()
        {
            canvasSize = new Vector2(1920, 1080);
            isDirty = true;
        }
        public static ASXMLCanvas LoadFromXML(string xmlPath)
        {
            xmlPath = Path.Combine(ASECore.AssetsPath, xmlPath);
            string xsdPath = Path.Combine(ASECore.AssetsPath, "internal/ui/ASXML.xsd");

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
        public void SetDirty()
        {
            isDirty = true;
        }
        public void Draw()
        {
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

            ASECore.defaultUIShader.Use();
            ASECore.defaultUIShader.SetVector("uCanvasSize", (Vector2i)canvasSize);

            GL.BindVertexArray(FullscreenQuadMesh.mesh.vao);

            Group[] groups = rootGroups.ToArray();

            if (isDirty)
            {
                LayoutGroups(groups, LayoutType.None, Vector2.Zero, canvasSize);
                isDirty = false;
            }

            DrawGroups(groups);

            GL.BindVertexArray(0);
            GL.Disable(EnableCap.Blend);
        }

        //TODO: create dirty feature, only rebuilding the layout when necessary
        //TODO: create per-element dirty feature
        private void LayoutGroups(Group[] groups, LayoutType layoutType, Vector2 parentPositionT, Vector2 parentSizeT)
        {
            if (groups.Length == 0)
                return;

            ILayoutProcessor layoutProcessor = LayoutDictionary.layoutProcessors[layoutType];
            layoutProcessor.ProcessLayout(parentPositionT, parentSizeT, groups);

            for (int i = 0; i < groups.Length; i++)
            {
                Group group = groups[i];

                LayoutType nextLayout = group.layoutType == LayoutType.Inherit ? layoutType : group.layoutType;

                LayoutGroups(group.children.ToArray(), nextLayout, group.position, group.size);
            }
        }
        private void DrawGroups(Group[] groups)
        {
            foreach (Group group in groups)
            {
                if (!string.IsNullOrEmpty(group.imagePath))
                {
                    ASECore.defaultUIShader.SetVector("uPosition", group.position);
                    ASECore.defaultUIShader.SetVector("uSize", group.size);
                    ASECore.defaultUIShader.SetColor("uColor", group.color);

                    GL.ActiveTexture(TextureUnit.Texture0);
                    GL.BindTexture(TextureTarget.Texture2D, group.texture);
                    ASECore.defaultUIShader.SetTexture("uMainTex", 0);

                    GL.DrawArrays(PrimitiveType.Triangles, 0, 6);
                    GL.BindTexture(TextureTarget.Texture2D, 0);
                }

                DrawGroups(group.children.ToArray());
            }
        }
        //private void LayoutAndDrawGroups(LayoutType layoutType, Group[] groups, Vector2 parentPositionT, Vector2 parentSizeT)
        //{
        //    if (groups.Length == 0)
        //        return;

        //    ILayoutProcessor layoutProcessor = LayoutDictionary.layoutProcessors[layoutType];
        //    (Vector2, Vector2)[] layoutResult = layoutProcessor.ProcessLayout(parentPositionT, parentSizeT, groups);

        //    for (int i = 0; i < groups.Length; i++)
        //    {
        //        Group group = groups[i];

        //        if (!string.IsNullOrEmpty(group.imagePath))
        //        {
        //            Core.defaultUIShader.SetVector("uPosition", layoutResult[i].Item1);
        //            Core.defaultUIShader.SetVector("uSize", layoutResult[i].Item2);
        //            Core.defaultUIShader.SetColor("uColor", group.color);

        //            GL.ActiveTexture(TextureUnit.Texture0);
        //            GL.BindTexture(TextureTarget.Texture2D, group.texture);
        //            Core.defaultUIShader.SetTexture("uMainTex", 0);

        //            GL.DrawArrays(PrimitiveType.Triangles, 0, 6);
        //            GL.BindTexture(TextureTarget.Texture2D, 0);
        //        }

        //        LayoutType nextLayout = group.layoutType == LayoutType.Inherit ? layoutType : group.layoutType;

        //        LayoutAndDrawGroups(nextLayout, group.children.ToArray(), layoutResult[i].Item1, layoutResult[i].Item2);
        //    }
        //}
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