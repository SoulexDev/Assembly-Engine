using System;
using System.Collections.Generic;
using System.Text;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace AssemblyEngine.Graphics
{
    //TODO: switch to particles knowing their system ID, rather than systems knowing their particles. This way,
    //you can simply iterate through each particle, and ask its system to update the particle with its settings.
    //This allows each particle to know whether or not its available, and allows you to iterate through them contiguously.
    //The systems can now be moved around easily, rather than moving around whole blocks of particles to ensure that they are iterated
    //through contiguously.
    //The drawback is that you have to order the systems contiguously in memory, but it is a whole lot better than the alternative

    //iterate through each system and have each system iterate through the particles. ezpz
    public class ParticleManager
    {
        private static List<Particle> particlePool = new List<Particle>();
        private static Queue<ParticleSystem> particleSystems = new Queue<ParticleSystem>();
        public static int EmitParticles(Texture2D texture)
        {
            ParticleSystem p = new ParticleSystem(particleSystems.Count);
            p.Start(texture, particlePool);
            particleSystems.Enqueue(p);

            return p.ID;
        }
        public static void UpdateParticles()
        {
            foreach (ParticleSystem system in particleSystems)
            {
                system.UpdateParticles(particlePool);
            }
            if (!particleSystems.Peek().active)
                particleSystems.Dequeue();
        }
        public static void DrawParticles()
        {
            uint lastTexID = 0;

            Core.defaultShader.Use();

            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindVertexArray(QuadMesh.mesh.vao);
            foreach(Particle p in particlePool)
            {
                if (lastTexID != p.textureID)
                {
                    GL.BindTexture(TextureTarget.Texture2D, p.textureID);
                    Core.defaultShader.SetTexture("uMainTex", 0);
                }
                GL.DrawArrays(PrimitiveType.Triangles, 0, 6);

                lastTexID = p.textureID;
            }
            GL.BindTexture(TextureTarget.Texture2D, 0);
        }
        public static void StopParticles(int ID)
        {
            if (particleSystems.Any(p => p.ID == ID))
                particleSystems.First(p => p.ID == ID).Stop();
        }
        internal static void SmartAllocateParticles(int particleCount, int systemID)
        {
            int allocationCount = 0;
            for (int i = 0; i < particlePool.Count; i++)
            {
                Particle p = particlePool[i];
                if (p.systemID == -1)
                {
                    p.systemID = systemID;
                    p.position = Vector3.Zero;
                    p.velocity = Vector3.Zero;
                    p.life = 0;
                    p.color = Color4.White;

                    allocationCount++;
                }
            }
            if (allocationCount < particleCount)
            {
                int dif = particleCount - allocationCount;
                for (int i = 0; i < dif; i++)
                {
                    Particle p = new Particle();
                    p.systemID = systemID;
                    particlePool.Add(p);
                }
            }
        }
    }
}
