using BepuPhysics;
using BepuPhysics.Collidables;

namespace AssemblyEngine.Physics
{
    public class Collider : Component
    {
        internal TypedIndex collidableIndex;
        protected StaticHandle staticHandle;

        public ContinuousDetectionMode continuousDetectionMode = ContinuousDetectionMode.Discrete;

        public override void Init()
        {
            if (!engineObject.HasComponentOfType<RigidBody>())
            {
                RigidPose pose = new RigidPose((System.Numerics.Vector3)transform.position, (System.Numerics.Quaternion)transform.rotation);
                StaticDescription staticDescription = new StaticDescription(pose, collidableIndex);
                staticHandle = PhysicsSimulation.simulation.Statics.Add(staticDescription);
            }
        }
        public override void OnComponentAdded(Component component)
        {
            if (component is RigidBody)
            {
                PhysicsSimulation.simulation.Statics.Remove(staticHandle);
            }
        }
        public override void OnComponentRemoved(Component component)
        {
            if (component is RigidBody)
            {
                RigidPose pose = new RigidPose((System.Numerics.Vector3)transform.position, (System.Numerics.Quaternion)transform.rotation);
                StaticDescription staticDescription = new StaticDescription(pose, collidableIndex);
                staticHandle = PhysicsSimulation.simulation.Statics.Add(staticDescription);
            }
        }
        internal virtual BodyInertia GetBodyIntertia(float mass)
        {
            return default(BodyInertia);
        }
    }
}
