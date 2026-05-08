using AssemblyEngine.Graphics;
using Hexa.NET.ImGui;
using OpenTK.Graphics.OpenGL4;
using System.Numerics;

namespace AssemblyEngine.GUI
{
    public class SceneViewGUI : GUIWindow
    {
        private RenderTexture sceneTex;
        private Vector2 viewportSize;

        public SceneViewGUI()
        {
            sceneTex = new RenderTexture(1920, 1080, RenderTextureType.Normal);
        }
        public override void Draw()
        {
            ImGui.Begin("Scene View");

            ImGui.BeginChild("Viewport");

            viewportSize = ImGui.GetWindowSize();
            if ((int)viewportSize.X != sceneTex.width || (int)viewportSize.Y != sceneTex.height)
            {
                sceneTex.Resize((int)viewportSize.X, (int)viewportSize.Y);
            }
            //CorrectViewport();

            nint texID = (ImTextureID)(sceneTex.texture.id);

            unsafe
            {
                ImGui.Image(new ImTextureRef(null, texID), viewportSize);
            }
            
            ImGui.EndChild();

            ImGui.End();
        }
        public void CorrectViewport()
        {
            GL.Viewport(0, 0, (int)viewportSize.X, (int)viewportSize.Y);
        }
        public void BindFBO()
        {
            sceneTex.Bind();
        }
        public void UnbindFBO()
        {
            sceneTex.Unbind();
        }
    }
}
