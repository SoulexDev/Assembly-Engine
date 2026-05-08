using BepuPhysics;
using OpenTK.Mathematics;
using System.Text.Json.Serialization;

namespace AssemblyEngine
{
    [JsonSerializable(typeof(Transform), GenerationMode = JsonSourceGenerationMode.Default)]
    public class Transform
    {
        public EngineObject engineObject;

        public Transform parent;
        public List<Transform> children = new List<Transform>();

        private Matrix4 translationMatrix;
        private Matrix4 rotationMatrix;
        private Matrix4 scaleMatrix;

        public Matrix4 transformationMatrix = Matrix4.Identity;

        public delegate void TransformChanged();
        public event TransformChanged OnTransformChanged;

        public Vector3 localPosition = Vector3.Zero;
        public Quaternion localRotation = Quaternion.Identity;
        public Vector3 localScale = Vector3.One;

        //private Vector3 _globalPosition;
        //private Quaternion _globalRotation;
        //private Vector3 _globalScale;

        internal Transform()
        {

        }
        internal Transform(EngineObject engineObject)
        {
            this.engineObject = engineObject;
        }
        //public Vector3 localPosition
        //{
        //    get { return _localPosition; }
        //    set
        //    {
        //        _localPosition = value;
        //        RecalculateTransformationMatrix();

        //        OnTransformChanged?.Invoke();
        //    }
        //}
        //public Quaternion localRotation
        //{
        //    get { return _localRotation; }
        //    set
        //    {
        //        _localRotation = value;
        //        RecalculateTransformationMatrix();

        //        OnTransformChanged?.Invoke();
        //    }
        //}
        //public Vector3 localScale
        //{
        //    get { return _localScale; }
        //    set
        //    {
        //        _localScale = value;
        //        RecalculateTransformationMatrix();

        //        OnTransformChanged?.Invoke();
        //    }
        //}
        public Vector3 position
        {
            //get
            //{
            //    return _globalPosition;
            //}
            //set
            //{
            //    //transform into local space
            //    //set local value
            //    //transform back into world space

            //    localPosition = (new Vector4(value, 1.0f) * transformationMatrix.Inverted()).Xyz;
            //    _globalPosition = transformationMatrix.ExtractTranslation();
            //}
            get
            {
                if (parent == null)
                    return localPosition;

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

                    //Matrix4.CreateTranslation(position, out translationMatrix);

                    //RecalculateTransformationMatrix();
                    OnTransformChanged?.Invoke();
                }
            }
        }
        public Quaternion rotation
        {
            //get
            //{
            //    return _globalRotation;
            //}
            //set
            //{
            //    localRotation = value * transformationMatrix.ExtractRotation().Inverted();
            //    _globalRotation = transformationMatrix.ExtractRotation();
            //}
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
                }

                //Matrix4.CreateFromQuaternion(rotation, out rotationMatrix);

                //RecalculateTransformationMatrix();
                OnTransformChanged?.Invoke();
            }
        }
        public Vector3 scale
        {
            //get
            //{
            //    return _globalScale;
            //}
            //set
            //{
            //    localScale = (new Vector4(value, 1f) * transformationMatrix.Inverted()).Xyz;
            //    _globalScale = transformationMatrix.ExtractScale();
            //}
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
                }

                //Matrix4.CreateScale(scale, out scaleMatrix);

                //RecalculateTransformationMatrix();
                OnTransformChanged?.Invoke();
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
            if (transform == this)
            {
                Console.WriteLine("Cannot set transform as parent of self");
                return;
            }
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
                    transform.children.Add(this);

                    localPosition = relativePos;
                    localRotation = relativeRot;
                    localScale = relativeScale;
                }
                else
                {
                    parent = transform;
                    transform.children.Add(this);

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

                    parent.children.Remove(this);
                    parent = null;

                    position = worldPos;
                    rotation = worldRot;
                    scale = worldScale;
                }
                else
                {
                    parent.children.Remove(this);
                    parent = null;
                }
            }
        }
        private void RecalculateTransformationMatrix()
        {
            //TODO: optimize
            transformationMatrix = scaleMatrix * rotationMatrix * translationMatrix;
        }
        public void Rotate(Vector3 axis, float angle)
        {
            rotation *= Quaternion.FromAxisAngle(axis, angle * (MathF.PI / 180.0f));
        }
        public static implicit operator RigidPose(Transform transform)
        {
            return new RigidPose((System.Numerics.Vector3)transform.position, (System.Numerics.Quaternion)transform.rotation);
        }
    }
}
