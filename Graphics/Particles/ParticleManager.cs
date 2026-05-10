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
        private static List<ParticleSystem> particleSystems = new List<ParticleSystem>();
        /// <summary>
        /// Emit particles
        /// </summary>
        /// <param name="texture"></param>
        /// <param name="looping"></param>
        /// <param name="duration"></param>
        /// <param name="emissionType"></param>
        /// <param name="emitterShape"></param>
        /// <param name="particleCount">For constant emission, this is the rate of particles per second. For burst emission, this is the amount of particles that are emitted at once</param>
        /// <param name="minLifeTime"></param>
        /// <param name="maxLifeTime"></param>
        /// <param name="minSize"></param>
        /// <param name="maxSize"></param>
        /// <param name="sizeAtStart"></param>
        /// <param name="sizeAtEnd"></param>
        /// <param name="minStartSpeed"></param>
        /// <param name="maxStartSpeed"></param>
        /// <returns></returns>
        public static int EmitParticles(Texture2D texture,
            bool looping = false,
            float duration = 4,
            ParticleEmissionType emissionType = ParticleEmissionType.Burst,
            ParticleEmitterShape emitterShape = ParticleEmitterShape.Sphere,
            int particleCount = 20,
            float minLifeTime = 0.5f,
            float maxLifeTime = 2.0f,
            float minSize = 0.5f,
            float maxSize = 1.0f,
            float sizeAtStart = 0.5f,
            float sizeAtEnd = 1.0f,
            float minStartSpeed = 0.5f,
            float maxStartSpeed = 2.0f)
        {
            ParticleSystem p = new ParticleSystem(particleSystems.Count,
                texture,
                looping,
                duration,
                emissionType,
                emitterShape,
                particleCount,
                minLifeTime,
                maxLifeTime,
                minSize,
                maxSize,
                sizeAtStart,
                sizeAtEnd,
                minStartSpeed,
                maxStartSpeed);

            p.Start(particlePool);
            particleSystems.Add(p);

            return p.ID;
        }
        public static void UpdateParticles()
        {
            if (particleSystems.Count == 0)
                return;

            foreach (ParticleSystem system in particleSystems)
            {
                system.UpdateParticles(particlePool);
            }
            if (particleSystems[particleSystems.Count - 1].dead)
            {
                particleSystems.RemoveAt(particleSystems.Count - 1);
            }
        }
        public static void DrawParticles()
        {
            uint lastTexID = 0;
            GL.Disable(EnableCap.CullFace);
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

            ASECore.defaultParticleShader.Use();

            ASECore.defaultParticleShader.SetMatrix4("uView", Camera.main.viewMatrix);
            ASECore.defaultParticleShader.SetMatrix4("uProjection", Camera.main.projectionMatrix);

            //GL.BindVertexArray(QuadMesh.mesh.vao);
            foreach(Particle p in particlePool)
            {
                if (p.systemID == -1)
                    continue;

                if (lastTexID != p.textureID)
                {
                    GL.ActiveTexture(TextureUnit.Texture0);
                    GL.BindTexture(TextureTarget.Texture2D, p.textureID);
                    ASECore.defaultParticleShader.SetTexture("uMainTex", 0);
                }

                //Matrix4 modelMatrix = Matrix4.Identity;
                //modelMatrix *= Matrix4.CreateScale(Vector3.One);
                Matrix4.CreateTranslation(p.position, out Matrix4 modelMatrix);

                ASECore.defaultParticleShader.SetMatrix4("uModel", modelMatrix);
                ASECore.defaultParticleShader.SetVector("uSize", p.size);

                QuadMesh.mesh.Draw();
                //GL.DrawArrays(PrimitiveType.Triangles, 0, 6);

                lastTexID = p.textureID;
            }
            GL.Disable(EnableCap.Blend);

            //GL.BindTexture(TextureTarget.Texture2D, 0);
        }
        public static void StopParticles(int ID)
        {
            if (particleSystems.Any(p => p.ID == ID))
                particleSystems.First(p => p.ID == ID).Stop(particlePool);
        }
        internal static void SmartAllocateParticles(int particleCount, int systemID)
        {
            int allocationCount = 0;
            ParticleSystem system = particleSystems[systemID];

            for (int i = 0; i < particlePool.Count; i++)
            {
                Particle p = particlePool[i];
                if (p.systemID == -1)
                {
                    particlePool[i] = system.InitializeParticle(p);

                    allocationCount++;

                    if (allocationCount >= particleCount)
                        break;
                }
            }

            if (allocationCount < particleCount)
            {
                int dif = particleCount - allocationCount;
                for (int i = 0; i < dif; i++)
                {
                    Particle p = system.InitializeParticle(new Particle());
                    particlePool.Add(p);
                }
            }
        }
    }
}
