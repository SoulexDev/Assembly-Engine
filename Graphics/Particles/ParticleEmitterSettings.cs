using System;
using System.Collections.Generic;
using System.Text;

namespace AssemblyEngine.Graphics
{
    public enum ParticleEmissionType { Constant, Burst }
    public enum ParticleEmitterShape { Sphere, Cone }
    public struct ParticleEmitterSettings
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
    }
}
