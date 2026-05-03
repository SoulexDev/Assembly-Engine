using BepuPhysics;
using BepuPhysics.Collidables;

namespace AssemblyEngine.Physics
{
    public class BoxCollider : Collider
    {
        internal Box collidable;

        public float width
        {
            get { return collidable.Width; }
            set {  collidable.Width = value; }
        }
        public float height
        {
            get { return collidable.Height; }
            set { collidable.Height = value; }
        }
        public float length
        {
            get { return collidable.Length; }
            set { collidable.Length = value; }
        }
        public override void Init()
        {
            //TODO: make fit object bounding box
            collidable = new Box(transform.scale.X, transform.scale.Y, transform.scale.Z);
            collidableIndex = PhysicsSimulation.simulation.Shapes.Add(collidable);

            //always call base after in this scenario
            base.Init();
        }
        internal override BodyInertia GetBodyIntertia(float mass)
        {
            return collidable.ComputeInertia(mass);
        }
    }
}