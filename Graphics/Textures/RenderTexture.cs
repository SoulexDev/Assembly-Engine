using OpenTK.Graphics.OpenGL4;

namespace ASE.Graphics
{
    public enum RenderTextureType { Normal, Depth }
    public class RenderTexture : IDisposable
    {
        private uint fbo;
        private uint rbo;
        private Texture2D texture;

        private bool usesRBO;

        public int width;
        public int height;

        public RenderTexture(int width, int height, RenderTextureType renderTextureType, TextureWrapMode textureWrapMode = TextureWrapMode.Repeat)
        {
            usesRBO = renderTextureType == RenderTextureType.Normal;

            this.width = width;
            this.height = height;

            GL.GenFramebuffers(1, out fbo);
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, fbo);

            if (renderTextureType == RenderTextureType.Normal)
                texture = new Texture2D(this.width, this.height, textureWrapMode: textureWrapMode);
            else
            {
                texture = new Texture2D(this.width, this.height,
                    PixelInternalFormat.DepthComponent, PixelFormat.DepthComponent,
                    PixelType.Float, textureWrapMode: textureWrapMode);

                GL.BindTexture(TextureTarget.Texture2D, texture);
                float[] borderColor = { 1.0f, 1.0f, 1.0f, 1.0f };
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureBorderColor, borderColor);
                GL.BindTexture(TextureTarget.Texture2D, 0);
            }

            if (renderTextureType == RenderTextureType.Normal)
            {
                GL.GenRenderbuffers(1, out rbo);
                GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, rbo);
                GL.RenderbufferStorage(RenderbufferTarget.Renderbuffer, RenderbufferStorage.Depth24Stencil8, this.width, this.height);

                GL.FramebufferRenderbuffer(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthStencilAttachment, RenderbufferTarget.Renderbuffer, rbo);
                GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, TextureTarget.Texture2D, texture, 0);
            }
            else
            {
                GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthAttachment, TextureTarget.Texture2D, texture, 0);
                GL.DrawBuffer(DrawBufferMode.None);
                GL.ReadBuffer(ReadBufferMode.None);
            }

            if (GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer) != FramebufferErrorCode.FramebufferComplete)
                Console.WriteLine("Error: Framebuffer incomplete.");

            GL.BindTexture(TextureTarget.Texture2D, 0);
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);

            if (renderTextureType == RenderTextureType.Normal)
                GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, 0);
        }
        public void Bind()
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, fbo);
        }
        public void Unbind()
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
        }
        //disposal
        private bool _disposed;
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        public virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                GL.DeleteFramebuffers(1, ref fbo);
                if (usesRBO)
                    GL.DeleteRenderbuffers(1, ref rbo);
            }
        }
        ~RenderTexture()
        {
            Dispose(false);
        }

        public static implicit operator uint(RenderTexture rt) => rt.texture.id;
        //public static implicit operator Texture2D(RenderTexture rt) => rt.texture;
    }
}