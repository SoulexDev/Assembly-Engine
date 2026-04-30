using OpenTK.Mathematics;

namespace AssemblyEngine.Physics
{
    //unity requires that trigger colliders have rigidbody components... bullshit
    //colliders will automatically create and dispose of physics bodies used for detection
    public sealed class RigidBody : Component
    {
        public Vector3 linearVelocity { get; private set; }
        public Vector3 angularVelocity { get; private set; }

        public override void Init()
        {
            
        }
        public override void Update()
        {
            
        }
    }
}
