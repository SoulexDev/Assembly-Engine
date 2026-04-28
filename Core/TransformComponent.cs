using OpenTK.Mathematics;

namespace AssemblyEngine.Core
{
    public struct TransformComponent
    {
        public int parentIndex;

        public Matrix4 transformMatrix;

        public Vector3 position;
        public Quaternion rotation;
        public Vector3 scale;
    }
}
