using OpenTK.Graphics.OpenGL4;
using StbImageSharp;
using System.Runtime.CompilerServices;

namespace ASE.Graphics
{
    public class Texture2D : IDisposable
    {
        public uint id;
        public int width;
        public int height;

        public static Texture2D Create(string filePath)
        {
            return new Texture2D(filePath);
        }
        public Texture2D(uint id)
        {
            this.id = id;
        }
        public Texture2D(
            string filePath, 
            TextureWrapMode textureWrapMode = TextureWrapMode.Repeat, 
            TextureMinFilter minFilter = TextureMinFilter.Linear, 
            TextureMagFilter magFilter = TextureMagFilter.Linear, 
            bool generateMipMap = true)
        {
            GL.GenTextures(1, out id);
            GL.BindTexture(TextureTarget.Texture2D, id);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)textureWrapMode);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)textureWrapMode);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)minFilter);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)magFilter);

            AssignData(filePath);

            if (generateMipMap)
                GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);

            GL.BindTexture(TextureTarget.Texture2D, 0);
        }
        public Texture2D(
            int width,
            int height,
            PixelInternalFormat internalPixelFormat = PixelInternalFormat.Rgba, 
            PixelFormat pixelFormat = PixelFormat.Rgba, 
            PixelType pixelType = PixelType.UnsignedByte, 
            TextureWrapMode textureWrapMode = TextureWrapMode.Repeat,
            TextureMinFilter minFilter = TextureMinFilter.Linear,
            TextureMagFilter magFilter = TextureMagFilter.Linear)
        {
            GL.GenTextures(1, out id);
            GL.BindTexture(TextureTarget.Texture2D, id);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)textureWrapMode);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)textureWrapMode);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)minFilter);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)magFilter);

            GL.TexImage2D(TextureTarget.Texture2D, 0, internalPixelFormat, width, height, 0, pixelFormat, pixelType, 0);

            GL.BindTexture(TextureTarget.Texture2D, 0);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void AssignData(string filePath)
        {
            StbImage.stbi_set_flip_vertically_on_load(1);

            using (Stream stream = File.OpenRead(filePath))
            {
                ImageInfo? info = ImageInfo.FromStream(stream);
                
                if (info == null)
                {
                    Console.WriteLine("Could not load image because image info could not be obtained.");
                    return;
                }
                ImageResult image = ImageResult.FromStream(stream, info.Value.ColorComponents);
                if (image.SourceComp == ColorComponents.RedGreenBlue)
                {
                    GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb, image.Width, image.Height, 0,
                        PixelFormat.Rgb, PixelType.UnsignedByte, image.Data);
                }
                else if (image.SourceComp == ColorComponents.RedGreenBlueAlpha)
                {
                    GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, image.Width, image.Height, 0,
                        PixelFormat.Rgba, PixelType.UnsignedByte, image.Data);
                }

                width = image.Width;
                height = image.Height;
            }
        }
        public void Bind(TextureUnit unit = TextureUnit.Texture0)
        {
            GL.ActiveTexture(unit);
            GL.BindTexture(TextureTarget.Texture2D, id);
        }
        public void Unbind(TextureUnit unit = TextureUnit.Texture0)
        {
            GL.ActiveTexture(unit);
            GL.BindTexture(TextureTarget.Texture2D, 0);
        }

        //operators
        public static implicit operator uint(Texture2D texture) => texture.id;

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
                //this would be for freeing managed resources
                //if (disposing)
                //{
                    
                //}
                GL.DeleteTexture(id);
                _disposed = true;
            }
        }
        ~Texture2D()
        {
            Dispose(false);
        }
    }
}
