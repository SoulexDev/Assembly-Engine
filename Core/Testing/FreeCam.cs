using OpenTK.Mathematics;
using static SDL3.SDL;

namespace AssemblyEngine.Testing
{
    internal class FreeCam : Component
    {
        private Camera camera;
        private float mouseX, mouseY;

        public override void Init()
        {
            camera = new Camera();
            Camera.main = camera;
        }
        public override void Update()
        {
            Vector3 moveVector = Vector3.Zero;

            mouseX += Input.mouseDeltaX * 360.0f;
            mouseY -= Input.mouseDeltaY * 360.0f;

            mouseY = MathHelper.Clamp(mouseY, -89.0f, 89.0f);

            camera.transform.rotation = Quaternion.FromAxisAngle(Vector3.UnitY, MathHelper.DegToRad * mouseX);
            camera.transform.rotation *= Quaternion.FromAxisAngle(Vector3.UnitX, MathHelper.DegToRad * mouseY);

            moveVector = camera.transform.right * Input.horizontal + camera.transform.up * Input.longitudinal + camera.transform.forward * Input.vertical;
            moveVector.Normalize();

            moveVector *= 8;

            if (Input.IsKeyPressed(SDL_Scancode.SDL_SCANCODE_LSHIFT))
                moveVector *= 2;

            if (moveVector.Length > 0)
                camera.transform.position += moveVector * Time.deltaTime;
        }
    }
}
