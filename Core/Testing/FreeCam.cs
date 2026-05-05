using Hexa.NET.ImGui;
using OpenTK.Mathematics;
using static SDL3.SDL;

namespace AssemblyEngine.Testing
{
    internal class FreeCam : Component
    {
        private Camera camera;
        private float mouseX, mouseY;

        public float speed = 8;
        public float fastSpeedMultiplier = 2;

        public override void Init()
        {
            camera = engineObject.AddComponent<Camera>("camera");
            Camera.main = camera;
        }
        public override void Update()
        {
            if (!Input.IsMouseButtonPressed(SDL_MouseButtonFlags.SDL_BUTTON_RMASK))
                return;

            Vector3 moveVector = Vector3.Zero;

            mouseX += Input.mouseDeltaX * 360.0f;
            mouseY -= Input.mouseDeltaY * 360.0f;

            mouseY = MathHelper.Clamp(mouseY, -89.0f, 89.0f);

            transform.rotation = Quaternion.FromAxisAngle(Vector3.UnitY, MathHelper.DegToRad * mouseX);
            transform.rotation *= Quaternion.FromAxisAngle(Vector3.UnitX, MathHelper.DegToRad * mouseY);

            moveVector = transform.right * Input.horizontal + transform.up * Input.longitudinal + transform.forward * Input.vertical;
            moveVector.Normalize();

            moveVector *= speed;

            if (Input.IsKeyPressed(SDL_Scancode.SDL_SCANCODE_LSHIFT))
                moveVector *= fastSpeedMultiplier;

            if (moveVector.Length > 0)
                transform.position += moveVector * Time.deltaTime;
        }
        public override void DrawInspector()
        {
            ImGui.SliderFloat("Speed", ref speed, 0.01f, 25);
            ImGui.SliderFloat("Fast Speed Multiplier", ref fastSpeedMultiplier, 1, 4);
        }
    }
}