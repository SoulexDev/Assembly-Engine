using OpenTK.Mathematics;

namespace AssemblyEngine.Graphics
{
    public struct Particle
    {
        /// <summary>
        /// The ID of the system that currently "owns" this particle.
        /// A system ID of -1 means the particle is unused.
        /// </summary>
        public int systemID;
        public uint textureID;

        public Vector3 position;
        public Vector3 velocity;
        public Vector2 size;
        public Color4 color;
        public float life;

        public Vector2 startSize;
        public float startLife;
    }
}