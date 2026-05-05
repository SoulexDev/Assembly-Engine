using BepuPhysics;
using BepuPhysics.Collidables;
using BepuPhysics.CollisionDetection;
using BepuPhysics.Constraints;
using BepuUtilities;
using BepuUtilities.Memory;
using System.Runtime.CompilerServices;
//using OpenTK.Mathematics;

namespace AssemblyEngine.Physics
{
    public class PhysicsSimulation
    {
        public struct NarrowPhaseCallbacks : INarrowPhaseCallbacks
        {
            public SpringSettings contactSpringiness;
            public float maximumRecoveryVelocity;
            public float frictionCoefficient;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public bool AllowContactGeneration(int workerIndex, CollidableReference a, CollidableReference b, ref float speculativeMargin)
            {
                return a.Mobility == CollidableMobility.Dynamic || b.Mobility == CollidableMobility.Dynamic;
            }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public bool AllowContactGeneration(int workerIndex, CollidablePair pair, int childIndexA, int childIndexB)
            {
                return true;
            }
            public bool ConfigureContactManifold<TManifold>(int workerIndex, CollidablePair pair, ref TManifold manifold, out PairMaterialProperties pairMaterial) where TManifold : unmanaged, IContactManifold<TManifold>
            {
                pairMaterial.FrictionCoefficient = frictionCoefficient;
                pairMaterial.MaximumRecoveryVelocity = maximumRecoveryVelocity;
                pairMaterial.SpringSettings = contactSpringiness;
                return true;
            }
            public bool ConfigureContactManifold(int workerIndex, CollidablePair pair, int childIndexA, int childIndexB, ref ConvexContactManifold manifold)
            {
                return true;
            }
            public void Initialize(Simulation simulation)
            {
                if (contactSpringiness.AngularFrequency == 0 && contactSpringiness.TwiceDampingRatio == 0)
                {
                    contactSpringiness = new SpringSettings(30, 1);
                    maximumRecoveryVelocity = 2;
                    frictionCoefficient = 1;
                }
            }
            public void Dispose()
            {

            }
        }
        public struct PoseIntegratorCallbacks : IPoseIntegratorCallbacks
        {
            public System.Numerics.Vector3 gravity;
            public float linearDamping;
            public float angularDamping;

            public AngularIntegrationMode AngularIntegrationMode => AngularIntegrationMode.Nonconserving;

            public bool AllowSubstepsForUnconstrainedBodies => false;

            public bool IntegrateVelocityForKinematics => false;

            Vector3Wide gravityWideDt;
            System.Numerics.Vector<float> linearDampingDt;
            System.Numerics.Vector<float> angularDampingDt;

            public PoseIntegratorCallbacks(System.Numerics.Vector3 gravity, float linearDamping = 0.03f, float angularDamping = 0.03f) : this()
            {
                this.gravity = gravity;
                this.linearDamping = linearDamping;
                this.angularDamping = angularDamping;
            }
            public void Initialize(Simulation simulation)
            {
                
            }
            public void IntegrateVelocity(System.Numerics.Vector<int> bodyIndices, Vector3Wide position, QuaternionWide orientation, BodyInertiaWide localInertia, System.Numerics.Vector<int> integrationMask, int workerIndex, System.Numerics.Vector<float> dt, ref BodyVelocityWide velocity)
            {
                //velocity.Linear = (velocity.Linear + gravityWideDt) * linearDampingDt;
                //velocity.Angular = velocity.Angular * angularDampingDt;
                velocity.Linear += gravityWideDt;
            }
            public void PrepareForIntegration(float dt)
            {
                linearDampingDt = new System.Numerics.Vector<float>(MathF.Pow(MathHelper.Clamp(1 - linearDamping, 0, 1), dt));
                angularDampingDt = new System.Numerics.Vector<float>(MathF.Pow(MathHelper.Clamp(1 - angularDamping, 0, 1), dt));
                //TODO: cache gravity * dt
                gravityWideDt = Vector3Wide.Broadcast(gravity * dt);
            }
        }
        internal static Simulation simulation;

        private static ThreadDispatcher threadDispatcher;
        private static BufferPool bufferPool;

        public static float secondsPerTick = 1.0f / 50.0f;
        public static long millisecondsPerTick => (long)(secondsPerTick * 1000);

        public static void Init()
        {
            threadDispatcher = new ThreadDispatcher(Environment.ProcessorCount);
            bufferPool = new BufferPool();

            simulation = Simulation.Create(bufferPool, new NarrowPhaseCallbacks(), 
                new PoseIntegratorCallbacks(new System.Numerics.Vector3(0, -9.81f, 0)), new SolveDescription(8, 1));
        }
        public static void Tick()
        {
            if (Time.fixedDeltaTime <= 0)
            {
                //Console.WriteLine($"Fixed: {Time.fixedDeltaTime}, Time: {Time.time}");
                Time.fixedDeltaTime = secondsPerTick;
            }
            simulation.Timestep(secondsPerTick, threadDispatcher);
        }
        public static void Dispose()
        {
            simulation.Dispose();
            threadDispatcher.Dispose();
            bufferPool.Clear();
        }
    }
}