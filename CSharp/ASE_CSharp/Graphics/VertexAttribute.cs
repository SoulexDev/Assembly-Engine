using OpenTK.Graphics.OpenGL4;

namespace ASE.Graphics
{
    public class VertexAttribute
    {
        public int sizeInBytes;
        public int componentsCount;
        public VertexAttribPointerType pointerType;

        public static VertexAttribute Int16 = new VertexAttribute(VertexAttribPointerType.Int, sizeof(Int16), 1);
        public static VertexAttribute Int32 = new VertexAttribute(VertexAttribPointerType.Int, sizeof(Int32), 1);
        public static VertexAttribute Int64 = new VertexAttribute(VertexAttribPointerType.Int, sizeof(Int64), 1);
        public static VertexAttribute Float = new VertexAttribute(VertexAttribPointerType.Float, sizeof(Int32), 1);
        public static VertexAttribute Vector2 = new VertexAttribute(VertexAttribPointerType.Float, sizeof(float) * 2, 2);
        public static VertexAttribute Vector3 = new VertexAttribute(VertexAttribPointerType.Float, sizeof(float) * 3, 3);
        public static VertexAttribute Vector4 = new VertexAttribute(VertexAttribPointerType.Float, sizeof(float) * 4, 4);

        public VertexAttribute(VertexAttribPointerType pointerType, int sizeInBytes, int componentsCount)
        {
            this.pointerType = pointerType;
            this.sizeInBytes = sizeInBytes;
            this.componentsCount = componentsCount;
        }
        public static int GetTotalSizeInBytes(params VertexAttribute[] attributes)
        {
            int size = 0;
            for (int i = 0; i < attributes.Length; i++)
            {
                size += attributes[i].sizeInBytes;
            }
            return size;
        }
    }
}
