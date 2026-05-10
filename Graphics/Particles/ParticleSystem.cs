using OpenTK.Mathematics;

namespace AssemblyEngine.Graphics
{
    public enum ParticleEmissionType { Constant, Burst }
    public enum ParticleEmitterShape { Sphere, Cone }

    internal class ParticleSystem
    {
        public int ID;
        public bool active = true;
        public bool dead = false;

        public bool looping;
        public float duration;
        public ParticleEmissionType particleEmissionType;
        public ParticleEmitterShape particleEmitterShape;
        public int particleCount;
        public float minLifeTime, maxLifeTime;
        public float minSize, maxSize;
        public float sizeAtStart, sizeAtEnd;
        public float minStartSpeed, maxStartSpeed;
        public uint textureID;

        private float remainingDuration;
        private float emissionRate;
        private float emissionTimer;

        public ParticleSystem(int ID,
            uint textureID,
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
            this.textureID = textureID;
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

            remainingDuration = duration;
            emissionRate = 1f / particleCount;
        }
        public void Start(List<Particle> particles)
        {
            active = true;

            if (particleEmissionType == ParticleEmissionType.Burst)
            {
                ParticleManager.SmartAllocateParticles(particleCount, ID);
            }
            else
            {
                ParticleManager.SmartAllocateParticles(1, ID);
            }
        }
        public void Stop(List<Particle> particles)
        {
            active = false;

            for (int i = 0; i < particles.Count; i++)
            {
                Particle p = particles[i];

                if (p.systemID == ID)
                    p.systemID = -1;

                particles[i] = p;
            }
        }
        public Particle InitializeParticle(Particle p)
        {
            p.systemID = ID;

            p.textureID = textureID;
            p.position = ASERandom.InSphere(1);
            p.velocity = ASERandom.InSphere(1) * 5;
            p.color = Color4.Aqua;
            p.life = ASERandom.Range(minLifeTime, maxLifeTime);
            p.size = Vector2.One * ASERandom.Range(minSize, maxSize);

            p.startSize = p.size;
            p.startLife = p.life;

            return p;
        }
        public void UpdateParticles(List<Particle> particles)
        {
            emissionTimer += Time.deltaTime;
            if (emissionTimer >= emissionRate)
            {
                if (active && particleEmissionType == ParticleEmissionType.Constant)
                {
                    ParticleManager.SmartAllocateParticles(1, ID);
                }
                emissionTimer -= emissionRate;
            }

            dead = true;
            for (int i = 0; i < particles.Count; i++)
            {
                Particle p = particles[i];

                if (p.systemID != ID)
                    continue;

                float lifeProgress = 1f - p.life / p.startLife;

                p.life -= Time.deltaTime;
                p.size = p.startSize * MathHelper.Lerp(sizeAtStart, sizeAtEnd, lifeProgress);
                p.position += p.velocity * Time.deltaTime;

                if (p.life <= 0)
                    p.systemID = -1;

                particles[i] = p;

                dead = false;
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
