using System;
using System.Collections.Generic;
using System.Text;

namespace AssemblyEngine.Graphics
{
    //TODO: switch to particles knowing their system ID, rather than systems knowing their particles. This way,
    //you can simply iterate through each particle, and ask its system to update the particle with its settings.
    //This allows each particle to know whether or not its available, and allows you to iterate through them contiguously.
    //The systems can now be moved around easily, rather than moving around whole blocks of particles to ensure that they are iterated
    //through contiguously.
    //The drawback is that you have to order the systems contiguously in memory, but it is a whole lot better than the alternative

    public class ParticleManager
    {
        private static List<Particle> particlePool = new List<Particle>();
        private static List<ParticleSystem> particleSystems = new List<ParticleSystem>();
        public static void EmitParticles(ParticleEmitterSettings settings)
        {

        }
        public static void UpdateParticles()
        {
            foreach (Particle particle in particlePool)
            {

            }
        }
    }
}
