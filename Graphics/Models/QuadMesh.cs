using System;
using System.Collections.Generic;
using System.Text;
using OpenTK.Graphics.OpenGL4;

namespace AssemblyEngine.Graphics
{
    public class QuadMesh
    {
        public static float[] vertices =
        {
            0.5f, -0.5f, 0.0f,  0.0f, 0.0f, 1.0f,  1.0f, 0.0f,
            -0.5f, -0.5f, 0.0f,  0.0f, 0.0f, 1.0f,  0.0f, 0.0f,
            -0.5f, 0.5f, 0.0f,  0.0f, 0.0f, 1.0f,  0.0f, 1.0f,

            0.5f, -0.5f, 0.0f,  0.0f, 0.0f, 1.0f,  1.0f, 0.0f,
            -0.5f, 0.5f, 0.0f,  0.0f, 0.0f, 1.0f,  0.0f, 1.0f,
            0.5f, 0.5f, 0.0f,  0.0f, 0.0f, 1.0f,  1.0f, 1.0f
        };

        public static Mesh mesh;
        public static void Create()
        {
            mesh = new Mesh(vertices, PrimitiveType.Triangles, BufferUsageHint.StaticDraw, VertexAttribute.Vector3, VertexAttribute.Vector3, VertexAttribute.Vector2);
        }
    }
}
