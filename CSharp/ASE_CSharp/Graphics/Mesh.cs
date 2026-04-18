using OpenTK.Graphics.OpenGL4;

namespace ASE.Graphics
{
    public class Mesh : IDisposable
    {
        public uint vao;
        public uint vbo;
        public uint ebo;
        public int count;
        public PrimitiveType primitiveType;
        public BufferUsageHint drawType;

        private bool generatedBuffers;
        private bool usingIndices;

        public Mesh()
        {
            
        }
        public Mesh(float[] vertices, PrimitiveType primitiveType, BufferUsageHint drawType, params VertexAttribute[] attributes)
        {
            this.primitiveType = primitiveType;
            this.drawType = drawType;
            SetMesh(vertices, attributes);
        }
        public Mesh(float[] vertices, int[] indices, PrimitiveType primitiveType, BufferUsageHint drawType, params VertexAttribute[] attributes)
        {
            this.primitiveType = primitiveType;
            this.drawType = drawType;
            SetMesh(vertices, indices, attributes);
        }
        public Mesh(List<float> vertices, PrimitiveType primitiveType, BufferUsageHint drawType, params VertexAttribute[] attributes)
        {
            this.primitiveType = primitiveType;
            this.drawType = drawType;
            SetMesh(vertices.ToArray(), attributes);
        }
        public Mesh(List<float> vertices, List<int> indices, PrimitiveType primitiveType, BufferUsageHint drawType, params VertexAttribute[] attributes)
        {
            this.primitiveType = primitiveType;
            this.drawType = drawType;
            SetMesh(vertices.ToArray(), indices.ToArray(), attributes);
        }
        public Mesh SetMesh(float[] vertices, params VertexAttribute[] attributes)
        {
            if (vertices.Length <= 0)
            {
                Console.WriteLine("Attempted to create mesh with no data. This is not allowed.");
                return this;
            }

            usingIndices = false;

            if (!generatedBuffers)
            {
                GL.GenVertexArrays(1, out vao);
                GL.GenBuffers(1, out vbo);
            }

            count = vertices.Length;

            GL.BindVertexArray(vao);

            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, count * sizeof(float), vertices.ToArray(), drawType);

            int stride = VertexAttribute.GetTotalSizeInBytes(attributes);
            int offset = 0;
            for (int i = 0; i < attributes.Length; i++)
            {
                VertexAttribute attrib = attributes[i];

                GL.VertexAttribPointer(i, attrib.componentsCount, attrib.pointerType, false, stride, offset);
                GL.EnableVertexAttribArray(i);
            }

            generatedBuffers = true;
            return this;
        }
        public Mesh SetMesh(float[] vertices, int[] indices, params VertexAttribute[] attributes)
        {
            if (vertices.Length <= 0 || indices.Length <= 0)
            {
                Console.WriteLine("Attempted to create mesh with no data. This is not allowed.");
                return this;
            }

            usingIndices = true;

            if (!generatedBuffers)
            {
                GL.GenVertexArrays(1, out vao);
                GL.GenBuffers(1, out vbo);
                GL.GenBuffers(1, out ebo);
            }

            GL.BindVertexArray(vao);

            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices.ToArray(), drawType);

            count = indices.Length;

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ebo);
            GL.BufferData(BufferTarget.ElementArrayBuffer, count * sizeof(uint), indices.ToArray(), drawType);

            int stride = VertexAttribute.GetTotalSizeInBytes(attributes);
            int offset = 0;
            for (int i = 0; i < attributes.Length; i++)
            {
                VertexAttribute attrib = attributes[i];

                GL.VertexAttribPointer(i, attrib.componentsCount, attrib.pointerType, false, stride, offset);
                GL.EnableVertexAttribArray(i);
            }

            generatedBuffers = true;
            return this;
        }
        //draw normally
        public void Draw()
        {
            if (!generatedBuffers || count == 0)
                return;

            GL.BindVertexArray(vao);
            if (usingIndices)
                GL.DrawElements(primitiveType, count, DrawElementsType.UnsignedInt, 0);
            else
                GL.DrawArrays(primitiveType, 0, count);

            GL.BindVertexArray(0);
        }
        //draw override with special primitive type
        public void Draw(PrimitiveType primitiveType)
        {
            if (!generatedBuffers)
                return;

            GL.BindVertexArray(vao);
            if (usingIndices)
                GL.DrawElements(primitiveType, count, DrawElementsType.UnsignedInt, 0);
            else
                GL.DrawArrays(primitiveType, 0, count);

            GL.BindVertexArray(0);
        }
        public void Clear()
        {
            count = 0;
        }
        private bool _disposed;
        ~Mesh()
        {
            Dispose(false);
        }
        public void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                GL.DeleteBuffers(1, ref vao);
                GL.DeleteBuffers(1, ref vbo);
                GL.DeleteBuffers(1, ref ebo);
                _disposed = true;
            }
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
