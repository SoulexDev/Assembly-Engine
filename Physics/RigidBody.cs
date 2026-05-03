using BepuPhysics;
using BepuPhysics.Collidables;
using OpenTK.Mathematics;

namespace AssemblyEngine.Physics
{
    //unity requires that trigger colliders have rigidbody components... bullshit
    //colliders will automatically create and dispose of physics bodies used for detection
    public sealed class RigidBody : Component
    {
        internal BodyHandle bodyID { get; private set; }
        internal BodyInertia bodyIntertia { get; private set; }

        public Vector3 linearVelocity => (Vector3)PhysicsSimulation.simulation.Bodies[bodyID].Velocity.Linear;
        public Vector3 angularVelocity => (Vector3)PhysicsSimulation.simulation.Bodies[bodyID].Velocity.Angular;

        private bool _kinematic;
        public bool kinematic
        {
            get { return _kinematic; }
            set
            {
                _kinematic = value;
                if (_kinematic)
                {
                    PhysicsSimulation.simulation.Bodies[bodyID].BecomeKinematic();
                }
                else
                    PhysicsSimulation.simulation.Bodies[bodyID].SetLocalInertia(bodyIntertia);
            }
        }
        public override void Init()
        {
            List<Component> colliders = engineObject.GetComponentsOfType<Collider>();

            BodyDescription bodyDescription;
            BodyActivityDescription bodyActivityDescription = new BodyActivityDescription(0.001f);

            if (colliders.Count > 0)
            {
                Collider collider = (colliders[0] as Collider);
                bodyIntertia = collider.GetBodyIntertia(1);
                //TODO: make this an option later
                //ContinuousDetection continuousDetection = ContinuousDetection.Continuous();

                //CollidableDescription collidableDescription = new CollidableDescription(collider.collidableIndex, continuousDetection);
                bodyDescription = BodyDescription.CreateDynamic((System.Numerics.Vector3)transform.position, bodyIntertia, collider.collidableIndex, 0.01f);
            }
            else
            {
                BodyVelocity bodyVelocity = new BodyVelocity(System.Numerics.Vector3.Zero);
                CollidableDescription collidableDescription = new CollidableDescription();
                bodyDescription = BodyDescription.CreateKinematic((System.Numerics.Vector3)transform.position, bodyVelocity, collidableDescription, bodyActivityDescription);
            }

            bodyID = PhysicsSimulation.simulation.Bodies.Add(bodyDescription);
        }
        public override void OnComponentAdded(Component component)
        {

        }
        public override void PhysicsTick()
        {
            transform.position = (Vector3)PhysicsSimulation.simulation.Bodies[bodyID].Pose.Position;
            transform.rotation = (Quaternion)PhysicsSimulation.simulation.Bodies[bodyID].Pose.Orientation;
        }
    }
}