using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace AssemblyEngine.Graphics.Testing
{
    internal class PlaneGenerator
    {
        private static Vector2[] quadUVs = {
            new Vector2(0.0f, 0.0f),
            new Vector2(1.0f, 0.0f),
            new Vector2(0.0f, 1.0f),
            new Vector2(1.0f, 1.0f),
        };
        private static Vector3[] quadVerts = {
            //bottom left, top left, bottom right,
            //bottom right, top left, top right,
            new Vector3(0.0f, 0.0f, 0.0f), //bottom left - 0[0] 7
            new Vector3(1.0f, 0.0f, 0.0f), //bottom right - 1[2, 3] 6
            new Vector3(0.0f, 0.0f, 1.0f), //top left - 2[1, 4] 2
            new Vector3(1.0f, 0.0f, 1.0f)  //top right - 3[5] 3
        };
        public static Plane Generate(Texture2D texture, PrimitiveType primitiveType, int widthX = 10, int widthZ = 10, float uvScale = 1)
        {
            ResourceLoader.LoadResource(out Shader shader, 
                ("shaders/internal/simple_lit", ShaderType.VertexShader), 
                ("shaders/internal/simple_lit", ShaderType.FragmentShader));

            Material mat = new Material(shader);
            mat.texture2Ds.Add(("uMainTex", texture));

            List<float> vertices = new List<float>();
            List<int> indices = new List<int>();

            int index = 0;

            for (int z = 0; z < widthZ; z++)
            {
                for (int x = 0; x < widthX; x++)
                {
                    Vector2 uvCorner = new Vector2((float)x / widthX, (float)z / widthZ);
                    for (int i = 0; i < 4; i++)
                    {
                        Vector3 vert = quadVerts[i];
                        Vector2 uv = quadUVs[i];

                        //position
                        vertices.Add(x + vert.X - widthX * 0.5f);
                        vertices.Add(vert.Y);
                        vertices.Add(z + vert.Z - widthZ * 0.5f);

                        //normals
                        vertices.Add(0.0f);
                        vertices.Add(1.0f);
                        vertices.Add(0.0f);

                        //uvs
                        vertices.Add((float)(x + uv.X) / (float)widthX * uvScale);
                        vertices.Add((float)(z + uv.Y) / (float)widthZ * uvScale);
                        //vertices.Add(x + vert.X);
                        //vertices.Add(z + vert.Z);
                        //Console.WriteLine(new Vector2((float)(x + uv.X) / (float)widthX, (float)(z + uv.Y) / (float)widthZ));
                    }

                    indices.Add(index);
                    indices.Add(index + 2);
                    indices.Add(index + 1);
                    indices.Add(index + 1);
                    indices.Add(index + 2);
                    indices.Add(index + 3);

                    index += 4;
                }
            }
            
            Mesh mesh = new Mesh(vertices, indices, primitiveType, BufferUsageHint.StaticDraw, 
                VertexAttribute.Vector3, VertexAttribute.Vector3, VertexAttribute.Vector2);

            return new Plane(mesh, mat);
        }
    }
}
