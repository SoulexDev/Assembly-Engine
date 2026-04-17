using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Text;

namespace ASE
{
    public enum CameraProjectionType { Perspective, Orthographic }
    public class Camera
    {
        public static Camera main;

        public Transform transform;

        public float fov;
        public float orthoWidth;
        public float orthoHeight;
        public float nearPlane;
        public float farPlane;
        public CameraProjectionType cameraProjectionType;

        public Matrix4 viewMatrix;
        public Matrix4 projectionMatrix;

        public Camera()
        {
            transform = new Transform();

            fov = 90.0f;
            orthoWidth = 4.0f;
            orthoHeight = 4.0f;
            nearPlane = 0.1f;
            farPlane = 1000.0f;

            cameraProjectionType = CameraProjectionType.Perspective;
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
