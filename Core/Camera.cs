using OpenTK.Mathematics;

namespace AssemblyEngine
{
    public enum CameraProjectionType { Perspective, Orthographic }
    public sealed class Camera : Component
    {
        public static Camera main;

        public float fov;
        public float orthoWidth;
        public float orthoHeight;
        public float nearPlane;
        public float farPlane;
        public CameraProjectionType cameraProjectionType;

        public Matrix4 viewMatrix;
        public Matrix4 projectionMatrix;

        public override void Init()
        {
            fov = 90.0f;
            orthoWidth = 16.0f;
            orthoHeight = 9.0f;
            nearPlane = 0.1f;
            farPlane = 1000.0f;

            cameraProjectionType = CameraProjectionType.Perspective;
        }
        public Camera InitializeParameters(float fov = 90.0f, float nearPlane = 0.1f, float farPlane = 1000.0f)
        {
            this.fov = fov;
            this.nearPlane = nearPlane;
            this.farPlane = farPlane;

            cameraProjectionType = CameraProjectionType.Perspective;

            return this;
        }
        public Camera InitializeParameters(float orthoWidth = 16.0f, float orthoHeight = 9.0f, float nearPlane = 0.1f, float farPlane = 1000.0f)
        {
            this.orthoWidth = orthoWidth;
            this.orthoHeight = orthoHeight;
            this.nearPlane = nearPlane;
            this.farPlane = farPlane;

            cameraProjectionType = CameraProjectionType.Orthographic;

            return this;
        }
        public void SetMatrices()
        {
            viewMatrix = Matrix4.LookAt(transform.position, transform.position + transform.forward, Vector3.UnitY);

            switch (cameraProjectionType)
            {
                case CameraProjectionType.Perspective:
                    projectionMatrix = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegToRad * fov, (float)Engine.screenWidth / Engine.screenHeight, nearPlane, farPlane);
                    break;
                case CameraProjectionType.Orthographic:
                    projectionMatrix = Matrix4.CreateOrthographic(orthoWidth, orthoHeight, nearPlane, farPlane);
                    break;
                default:
                    break;
            }
        }
    }
}
