using ASE.Graphics;
using OpenTK.Graphics.OpenGL4;

namespace ASE
{
    public class ResourceLoader
    {
        private static Dictionary<ShaderType, string> shaderFileExtensions = new Dictionary<ShaderType, string>
        {
            { ShaderType.VertexShader, ".vert" },
            { ShaderType.GeometryShader, ".geo" },
            { ShaderType.FragmentShader, ".frag" },
            { ShaderType.ComputeShader, ".comp" },
            { ShaderType.TessControlShader, ".tesc" },
            { ShaderType.TessEvaluationShader, ".tese" }
        };
        private static Dictionary<Assimp.TextureWrapMode, TextureWrapMode> texWrapModeBindings = new Dictionary<Assimp.TextureWrapMode, TextureWrapMode>
        {
            { Assimp.TextureWrapMode.Wrap, TextureWrapMode.Repeat },
            { Assimp.TextureWrapMode.Mirror, TextureWrapMode.MirroredRepeat },
            { Assimp.TextureWrapMode.Clamp, TextureWrapMode.ClampToBorder },
            { Assimp.TextureWrapMode.Decal, TextureWrapMode.ClampToBorder },
        };
        private static Assimp.AssimpContext importer = new Assimp.AssimpContext();

        public static void LoadResource(out Texture2D texture, string filePath)
        {
            filePath = Path.Combine(Core.AssetsPath, filePath);

            Console.WriteLine("Loading texture " + filePath);

            texture = new Texture2D(filePath);
        }
        public static bool LoadResource(out Shader shader, params (string, ShaderType)[] fileInfo)
        {
            Console.WriteLine("Loading shader " + Path.Combine(Core.AssetsPath, fileInfo[0].Item1));

            (string, ShaderType)[] shaderInfo = new (string, ShaderType)[fileInfo.Length];

            for (int i = 0; i < fileInfo.Length; i++)
            {
                var sInfo = shaderInfo[i];

                string filePath = Path.Combine(Core.AssetsPath, fileInfo[i].Item1);

                sInfo.Item1 = File.ReadAllText(filePath + shaderFileExtensions[fileInfo[i].Item2]);
                sInfo.Item2 = fileInfo[i].Item2;

                if (sInfo.Item1 == string.Empty)
                {
                    shader = -1;
                    return false;
                }

                shaderInfo[i] = sInfo;
            }

            if (Shader.CreateShaderProgram(out int id, shaderInfo))
            {
                shader = id;
                return true;
            }
            else
            {
                shader = -1;
                return false;
            }
        }
        public static bool LoadResource(out Renderable renderable, string filePath)
        {
            filePath = Path.Combine(Core.AssetsPath, filePath);

            Console.WriteLine("Loading model " + filePath);

            string fileType = Path.GetExtension(filePath);

            using (Stream stream = File.OpenRead(filePath))
            {
                Assimp.Scene scene = importer.ImportFileFromStream(stream, fileType);

                if (scene == null || ((scene.SceneFlags & Assimp.SceneFlags.Incomplete) != 0) || scene.RootNode == null)
                {
                    Console.WriteLine($"Failed to import model {filePath}");
                    renderable = null;
                    return false;
                }

                renderable = new Renderable(new Transform());

                ProcessNode(scene.RootNode, scene, ref renderable);
            }
            return true;
        }
        private static void ProcessNode(Assimp.Node node, Assimp.Scene scene, ref Renderable renderable)
        { 
            for (int i = 0; i < node.MeshCount; i++)
            {
                Assimp.Mesh mesh = scene.Meshes[node.MeshIndices[i]];
                renderable.meshes.Add(ProcessMesh(mesh, scene));
            }
            for (int i = 0; i < node.ChildCount; i++)
            {
                ProcessNode(node.Children[i], scene, ref renderable);
            }
        }
        private static (Mesh, Material) ProcessMesh(Assimp.Mesh assimpMesh, Assimp.Scene scene)
        {
            //float[] vertices = new float[mesh.VertexCount];
            List<float> vertices = new List<float>();
            List<int> indices = new List<int>();
            Material mat = new Material(Core.defaultShader);

            //VertexAttributeType types = 0;

            //types |= (assimpMesh.HasVertices ? VertexAttributeType.Vector3 : 0);
            //types |= (assimpMesh.HasNormals ? VertexAttributeType.Vector3 : 0);
            //types |= (assimpMesh.HasNormals ? VertexAttributeType.Vector3 : 0);

            //vertices
            for (int i = 0; i < assimpMesh.VertexCount; i++)
            {
                //position
                if (assimpMesh.HasVertices)
                {
                    Assimp.Vector3D position = assimpMesh.Vertices[i];
                    vertices.Add(position.X);
                    vertices.Add(position.Y);
                    vertices.Add(position.Z);
                }

                //normal
                if (assimpMesh.HasNormals)
                {
                    Assimp.Vector3D normal = assimpMesh.Normals[i];
                    vertices.Add(normal.X);
                    vertices.Add(normal.Y);
                    vertices.Add(normal.Z);
                }

                //uvs
                for (int t = 0; t < assimpMesh.TextureCoordinateChannelCount; t++)
                {
                    if (assimpMesh.HasTextureCoords(t))
                    {
                        Assimp.Vector3D texCoord = assimpMesh.TextureCoordinateChannels[t][i];
                        vertices.Add(texCoord.X);
                        vertices.Add(texCoord.Y);
                    }
                }

                //vertex colors
                //for (int v = 0; v < assimpMesh.VertexColorChannelCount; v++)
                //{
                //    if (assimpMesh.HasVertexColors(v))
                //    {
                //        Assimp.Color4D color = assimpMesh.VertexColorChannels[v][i];
                //        vertices.Add(color.R);
                //        vertices.Add(color.G);
                //        vertices.Add(color.B);
                //        vertices.Add(color.A);
                //    }
                //}
            }

            //indices
            for (int i = 0; i < assimpMesh.FaceCount; i++)
            {
                Assimp.Face face = assimpMesh.Faces[i];

                for (int j = 0; j < face.IndexCount; j++)
                {
                    indices.Add(face.Indices[j]);
                }
            }

            //materials
            if (assimpMesh.MaterialIndex >= 0)
            {
                List<(string, Texture2D)> textures = new List<(string, Texture2D)>();

                Assimp.Material assimpMat = scene.Materials[assimpMesh.MaterialIndex];
                LoadMaterialTextures(textures, assimpMat, Assimp.TextureType.Diffuse, "uDiffuse");
                LoadMaterialTextures(textures, assimpMat, Assimp.TextureType.Specular, "uSpecular");

                mat.texture2Ds.AddRange(textures);
            }

            Mesh mesh = new Mesh(vertices, indices, PrimitiveType.Triangles, 
                assimpMesh.HasBones ? BufferUsageHint.DynamicDraw : BufferUsageHint.StaticDraw, 
                VertexAttribute.Vector3, VertexAttribute.Vector3, VertexAttribute.Vector2);

            return (mesh, mat);
        }
        private static void LoadMaterialTextures(List<(string, Texture2D)> textures, Assimp.Material mat, Assimp.TextureType type, string textureName)
        {
            for (int i = 0; i < mat.GetMaterialTextureCount(type); i++)
            {
                mat.GetMaterialTexture(type, i, out Assimp.TextureSlot assimpTexture);
                //TODO: update texture to allow per U/V wrap modes
                textures.Add((textureName + i, new Texture2D(assimpTexture.FilePath, texWrapModeBindings[assimpTexture.WrapModeU])));
            }
        }
        //private static string PreprocessShaderContent(string shaderRead)
        //{
        //Regex includeRegex = new Regex(@"^#include\s*(?:<([^>]+)>|""([^""]+)"")");

        //while (Regex)
        //{

        //}
        //}
    }
}