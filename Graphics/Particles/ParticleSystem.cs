using OpenTK.Mathematics;

namespace AssemblyEngine.Graphics
{
    public enum ParticleEmissionType { Constant, Burst }
    public enum ParticleEmitterShape { Sphere, Cone }

    internal struct ParticleSystem
    {
        public bool looping;
        public float duration;
        public ParticleEmissionType particleEmissionType;
        public ParticleEmitterShape particleEmitterShape;
        public int particleCount;
        public float minLifeTime, maxLifeTime;
        public float minSize, maxSize;
        public float sizeAtStart, sizeAtEnd;
        public float minStartSpeed, maxStartSpeed;

        public int ID;
        public bool active = true;

        private float remainingDuration;

        //public ParticleSystem(List<Particle> particles)
        //{
        //    active = true;
        //    this.settings = settings;

        //    particleLength = settings.particleCount;
        //    remainingDuration = settings.duration;

        //    for (int i = 0; i < particleLength; i++)
        //    {
        //        Particle p = particles[particleStartIndex + i];
        //        p.position = ASERandom.InSphere(1);
        //        p.velocity = (p.position - Vector3.Zero).Normalized() * 
        //            ASERandom.Range(settings.minStartSpeed, settings.maxStartSpeed);
        //        p.inUse = true;
        //    }
        //}
        public ParticleSystem(int ID, 
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
            this.ID = ID;
            this.looping = looping;
            this.duration = duration;
            particleEmissionType = emissionType;
            particleEmitterShape = emitterShape;
            this.particleCount = particleCount;
            this.minLifeTime = minLifeTime;
            this.maxLifeTime = maxLifeTime;
            this.minSize = minSize;
            this.maxSize = maxSize;
            this.sizeAtStart = sizeAtStart;
            this.sizeAtEnd = sizeAtEnd;
            this.minStartSpeed = minStartSpeed;
            this.maxStartSpeed = maxStartSpeed;

            ParticleManager.SmartAllocateParticles(particleCount, ID);
        }
        public void Start(uint textureID, List<Particle> particles)
        {
            active = true;
            for (int i = 0; i < particles.Count; i++)
            {
                Particle p = particles[i];
                if (p.systemID != ID)
                    continue;

                p.textureID = textureID;
                p.position = ASERandom.InSphere(1);
                p.velocity = ASERandom.InSphere(1) * 5;
                p.color = Color4.Aqua;
                p.life = ASERandom.Range(minLifeTime, maxLifeTime);
                p.size = Vector2.One * ASERandom.Range(minSize, maxSize);

                particles[i] = p;
            }
        }
        public void Stop()
        {
            active = false;
        }
        public void UpdateParticles(List<Particle> particles)
        {
            if (!active)
                return;

            for (int i = 0; i < particles.Count; i++)
            {
                Particle p = particles[i];
                if (p.systemID != ID)
                    continue;

                p.life -= Time.deltaTime;
                //p.size = MathHelper.Lerp(sizeAtStart, sizeAtEnd, p.life /);
                p.velocity += -p.position * 2 * Time.deltaTime;
                p.position += p.velocity * Time.deltaTime;

                particles[i] = p;
            }
            remainingDuration -= Time.deltaTime;

            if (remainingDuration <= 0)
            {
                if (looping)
                {
                    remainingDuration += duration;
                }
                else
                {
                    active = false;
                }
            }
        }
    }
}
