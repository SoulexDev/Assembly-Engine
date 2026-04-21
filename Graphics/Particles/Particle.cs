using OpenTK.Mathematics;

namespace AssemblyEngine.Graphics
{
    public struct Particle
    {
        public bool inUse;
        public Vector3 position;
        public Vector3 velocity;
        public Color4 color;
        public float life;
    }
}
