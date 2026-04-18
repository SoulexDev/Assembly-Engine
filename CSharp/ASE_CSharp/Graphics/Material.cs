using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace ASE.Graphics
{
    public class Material
    {
        public Shader shader;

        public bool blendingEnabled = false;
        public BlendingFactor srcBlendMode = BlendingFactor.SrcAlpha;
        public BlendingFactor dstBlendMode = BlendingFactor.OneMinusSrcAlpha;

        public bool logicOpEnabled = false;
        public LogicOp logicOp = LogicOp.Noop;

        public bool cullFaceEnabled = true;
        public TriangleFace cullFaceMode = TriangleFace.Back;

        public bool depthTestEnabled = true;
        public DepthFunction depthFunction = DepthFunction.Lequal;

        public List<(string, Texture2D)> texture2Ds = new List<(string, Texture2D)>();
        public List<(string, int)> integers = new List<(string, int)>();
        public List<(string, float)> floats = new List<(string, float)>();
        public List<(string, Vector2)> vector2s = new List<(string, Vector2)>();
        public List<(string, Vector3)> vector3s = new List<(string, Vector3)>();
        public List<(string, Vector4)> vector4s = new List<(string, Vector4)>();

        public Material(Shader shader, 
            bool blendingEnabled = false, 
            BlendingFactor srcBlendMode = BlendingFactor.SrcAlpha, 
            BlendingFactor dstBlendMode = BlendingFactor.OneMinusSrcAlpha, 
            bool logicOpEnabled = false, 
            LogicOp logicOp = LogicOp.Noop, 
            bool cullFaceEnabled = true,
            TriangleFace cullFaceMode = TriangleFace.Back, 
            bool depthTestEnabled = true, 
            DepthFunction depthFunction = DepthFunction.Lequal)
        {
            this.shader = shader;
            this.blendingEnabled = blendingEnabled;
            this.srcBlendMode = srcBlendMode;
            this.dstBlendMode = dstBlendMode;
            this.logicOpEnabled = logicOpEnabled;
            this.logicOp = logicOp;
            this.cullFaceEnabled = cullFaceEnabled;
            this.cullFaceMode = cullFaceMode;
            this.depthTestEnabled = depthTestEnabled;
            this.depthFunction = depthFunction;
        }
        public void Use()
        {
            shader.Use();

            //blending
            if (blendingEnabled)
            {
                if (!GL.IsEnabled(EnableCap.Blend))
                    GL.Enable(EnableCap.Blend);

                GL.BlendFunc(srcBlendMode, dstBlendMode);
            }
            else if (GL.IsEnabled(EnableCap.Blend))
                GL.Disable(EnableCap.Blend);

            //logic op
            if (logicOpEnabled)
            {
                if (!GL.IsEnabled(EnableCap.ColorLogicOp))
                    GL.Enable(EnableCap.ColorLogicOp);

                GL.LogicOp(logicOp);
            }
            else if (GL.IsEnabled(EnableCap.ColorLogicOp))
                GL.Disable(EnableCap.ColorLogicOp);

            //face culling
            if (cullFaceEnabled)
            {
                if (!GL.IsEnabled(EnableCap.CullFace))
                    GL.Enable(EnableCap.CullFace);

                GL.CullFace(cullFaceMode);
            }
            else if (GL.IsEnabled(EnableCap.CullFace))
                GL.Disable(EnableCap.CullFace);

            //depth test
            if (depthTestEnabled)
            {
                if (!GL.IsEnabled(EnableCap.DepthTest))
                    GL.Enable(EnableCap.DepthTest);

                GL.DepthFunc(depthFunction);
            }
            else if (GL.IsEnabled(EnableCap.DepthTest))
                GL.Disable(EnableCap.DepthTest);

            //textures
            if (texture2Ds.Count > 0)
            {
                for (int i = 0; i < texture2Ds.Count; i++)
                {
                    texture2Ds[i].Item2.Bind(GetTextureUnit(i + 1));
                    shader.SetTexture(texture2Ds[i].Item1, i + 1);
                }
            }

            //integers
            if (integers.Count > 0)
            {
                for (int i = 0; i < integers.Count; i++)
                {
                    shader.SetInt(integers[i].Item1, integers[i].Item2);
                }
            }

            //floats
            if (floats.Count > 0)
            {
                for (int i = 0; i < floats.Count; i++)
                {
                    shader.SetFloat(floats[i].Item1, floats[i].Item2);
                }
            }

            //vector2s
            if (vector2s.Count > 0)
            {
                for (int i = 0; i < vector2s.Count; i++)
                {
                    shader.SetVector(vector2s[i].Item1, vector2s[i].Item2);
                }
            }

            //vector3s
            if (vector3s.Count > 0)
            {
                for (int i = 0; i < vector3s.Count; i++)
                {
                    shader.SetVector(vector3s[i].Item1, vector3s[i].Item2);
                }
            }

            //vector4s
            if (vector4s.Count > 0)
            {
                for (int i = 0; i < vector4s.Count; i++)
                {
                    shader.SetVector(vector4s[i].Item1, vector4s[i].Item2);
                }
            }
        }
        private static TextureUnit GetTextureUnit(int index)
        {
            if (index >= 32)
            {
                Console.WriteLine("Requested texture unit index is out of bounds.");
                return TextureUnit.Texture31;
            }
            return TextureUnit.Texture0 + index;
        }
    }
}