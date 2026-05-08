using BepuPhysics;
using BepuPhysics.Collidables;

namespace AssemblyEngine.Physics
{
    [Serializable]
    public class Collider : Component
    {
        internal TypedIndex collidableIndex;
        protected StaticHandle staticHandle;

        public ContinuousDetectionMode continuousDetectionMode = ContinuousDetectionMode.Discrete;

        protected bool isStatic;

        public override void Init()
        {
            if (!engineObject.HasComponentOfType<RigidBody>())
            {
                RigidPose pose = new RigidPose((System.Numerics.Vector3)transform.position, (System.Numerics.Quaternion)transform.rotation);
                StaticDescription staticDescription = new StaticDescription(pose, collidableIndex);
                staticHandle = PhysicsSimulation.simulation.Statics.Add(staticDescription);

                isStatic = true;
            }

            transform.OnTransformChanged += Transform_OnTransformChanged;
        }
        public override void OnComponentAdded(Component component)
        {
            if (component is RigidBody)
            {
                PhysicsSimulation.simulation.Statics.Remove(staticHandle);

                isStatic = false;
            }
        }
        public override void OnComponentRemoved(Component component)
        {
            if (component is RigidBody)
            {
                RigidPose pose = new RigidPose((System.Numerics.Vector3)transform.position, (System.Numerics.Quaternion)transform.rotation);
                StaticDescription staticDescription = new StaticDescription(pose, collidableIndex);
                staticHandle = PhysicsSimulation.simulation.Statics.Add(staticDescription);

                isStatic = true;
            }
        }
        private void Transform_OnTransformChanged()
        {
            if (isStatic)
            {
                StaticDescription desc = PhysicsSimulation.simulation.Statics[staticHandle].GetDescription();
                desc.Pose = transform;
                PhysicsSimulation.simulation.Statics[staticHandle].ApplyDescription(desc);
            }
        }
        internal virtual BodyInertia GetBodyIntertia(float mass)
        {
            return default(BodyInertia);
        }
    }
}
