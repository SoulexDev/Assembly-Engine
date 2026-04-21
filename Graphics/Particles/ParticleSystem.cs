using OpenTK.Mathematics;

namespace AssemblyEngine.Graphics
{
    internal struct ParticleSystem
    {
        private ParticleEmitterSettings settings;
        private int particleLength;
        private float remainingDuration;

        public bool active = true;

        public ParticleSystem(ParticleEmitterSettings settings, int particleStartIndex, List<Particle> particles)
        {
            active = true;
            this.settings = settings;

            particleLength = settings.particleCount;
            remainingDuration = settings.duration;

            for (int i = 0; i < particleLength; i++)
            {
                Particle p = particles[particleStartIndex + i];
                p.position = ASERandom.InSphere(1);
                p.velocity = (p.position - Vector3.Zero).Normalized() * 
                    ASERandom.Range(settings.minStartSpeed, settings.maxStartSpeed);
                p.inUse = true;
            }
        }
        public void UpdateParticles(int particleStartIndex, List<Particle> particles)
        {
            for (int i = 0; i < particleLength; i++)
            {
                Particle p = particles[particleStartIndex + i];
                p.position += p.velocity * Time.deltaTime;
            }
            remainingDuration -= Time.deltaTime;

            if (remainingDuration <= 0)
            {
                if (settings.looping)
                {
                    remainingDuration += settings.duration;
                }
                else
                {
                    active = false;
                }
            }
        }
    }
}
