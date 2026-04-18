using OpenTK.Mathematics;

namespace ASE
{
    public class Transform
    {
        public Transform? parent;
        public List<Transform>? children;

        public Vector3 position
        {
            get
            {
                if (parent == null)
                    return localPosition;

                return parent.position + localPosition;
            }
            set
            {
                if (parent == null)
                    localPosition = value;
                else
                    localPosition = value - parent.position;
            }
        }
        public Quaternion rotation
        {
            get 
            {
                if (parent == null)
                    return localRotation;

                return parent.rotation * localRotation;
            }
            set
            {
                if (parent == null)
                    localRotation = value;
                else
                    localRotation = value * parent.rotation.Inverted();
            }
        }
        public Vector3 scale
        {
            get
            {
                if (parent == null)
                    return localScale;

                return parent.scale * localScale;
            }
            set
            {
                if (parent == null)
                    localScale = value;
                else if (parent.scale.LengthSquared != 0)
                    localScale = value / parent.scale;
            }
        }

        public Vector3 localPosition = Vector3.Zero;
        public Quaternion localRotation = Quaternion.Identity;
        public Vector3 localScale = Vector3.One;

        public Vector3 right
        {
            get { return Vector3.Transform(Vector3.UnitX, rotation); }
            private set { }
        }
        public Vector3 up
        {
            get { return Vector3.Transform(Vector3.UnitY, rotation); }
            private set { }
        }
        public Vector3 forward
        {
            get { return Vector3.Transform(Vector3.UnitZ, rotation); }
            private set { }
        }
        public void SetParent(Transform transform, bool keepWorld = false)
        {
            //if (transform == null && parent == null)
            //    return;
            parent = transform;
            //if (keepWorld)
            //{
            //    if (transform == null && parent != null)
            //    {
            //        localPosition += parent.position;
            //        localRotation *= parent.rotation;

            //        parent.children.Remove(this);
            //        parent = null;
            //    }
            //    else if (transform != null)
            //    {
            //        localPosition -= parent!.position;
            //        localRotation *= parent.rotation.Inverted();

            //        parent = transform;
            //        parent.children.Add(this);
            //    }
            //}
            //else
            //{
            //    if (transform == null && parent != null)
            //    {
            //        parent.children.Remove(this);
            //        parent = null;
            //    }
            //    else
            //    {
            //        parent = transform;
            //        parent.children.Add(this);
            //    }
            //}
        }
        public void Rotate(Vector3 axis, float angle)
        {
            rotation *= Quaternion.FromAxisAngle(axis, angle * (MathF.PI / 180.0f));
        }
    }
}
