using OpenTK.Mathematics;

namespace AssemblyEngine
{
    public class Transform
    {
        public bool isDirty = true;

        public Transform parent;
        public List<Transform> children;

        public Vector3 localPosition = Vector3.Zero;
        public Quaternion localRotation = Quaternion.Identity;
        public Vector3 localScale = Vector3.One;
        public Vector3 position
        {
            get
            {
                if (parent == null)
                    return localPosition;

                //Vector3 dir = localPosition;
                return parent.position + parent.rotation * localPosition * parent.scale;
            }
            set
            {
                if (localPosition != value)
                {
                    if (parent == null)
                        localPosition = value;
                    else
                        localPosition = value - parent.position;

                    isDirty = true;
                }
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
                if (localRotation != value)
                {
                    if (parent == null)
                        localRotation = value;
                    else
                        localRotation = value * parent.rotation.Inverted();

                    isDirty = true;
                }
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
                if (localScale != value)
                {
                    if (parent == null)
                        localScale = value;
                    else if (parent.scale.LengthSquared != 0)
                        localScale = value / parent.scale;

                    isDirty = true;
                }
            }
        }
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
            //parent = transform;

            if (transform != null)
            {
                if (keepWorld)
                {
                    Vector3 relativePos = position - transform.position;
                    Quaternion relativeRot = rotation * transform.rotation.Inverted();
                    Vector3 relativeScale = scale / transform.scale;
                    parent = transform;
                    localPosition = relativePos;
                    localRotation = relativeRot;
                    localScale = relativeScale;
                }
                else
                {
                    parent = transform;
                    localPosition = Vector3.Zero;
                    localRotation = Quaternion.Identity;
                    localScale = Vector3.One;
                }
            }
            else
            {
                if (keepWorld)
                {
                    Vector3 worldPos = position;
                    Quaternion worldRot = rotation;
                    Vector3 worldScale = scale;

                    parent = null;

                    position = worldPos;
                    rotation = worldRot;
                    scale = worldScale;
                }
                else
                {
                    parent = null;
                }
            }
        }
        public void Rotate(Vector3 axis, float angle)
        {
            rotation *= Quaternion.FromAxisAngle(axis, angle * (MathF.PI / 180.0f));
        }
    }
}
