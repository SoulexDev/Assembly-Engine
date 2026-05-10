using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System.Collections;

namespace AssemblyEngine.Graphics
{
    public class Material
    {
        public Shader shader;

        private BlendingFactor srcBlendMode = BlendingFactor.SrcAlpha;
        private BlendingFactor dstBlendMode = BlendingFactor.OneMinusSrcAlpha;

        private LogicOp logicOp = LogicOp.Noop;

        private TriangleFace cullFaceMode = TriangleFace.Back;

        private DepthFunction depthFunction = DepthFunction.Lequal;

        public List<(string, Texture2D)> texture2Ds = new List<(string, Texture2D)>();
        public List<(string, int)> integers = new List<(string, int)>();
        public List<(string, float)> floats = new List<(string, float)>();
        public List<(string, Vector2)> vector2s = new List<(string, Vector2)>();
        public List<(string, Vector3)> vector3s = new List<(string, Vector3)>();
        public List<(string, Vector4)> vector4s = new List<(string, Vector4)>();

        private BitArray materialStates;

        public Material(Shader shader)
        {
            this.shader = shader;

            //blending, logic op, cull face, depth test
            materialStates = new BitArray([false, false, true, true]);
        }
        public Material EnableBlending(BlendingFactor srcBlendMode, BlendingFactor dstBlendMode)
        {
            materialStates[0] = true;
            this.srcBlendMode = srcBlendMode;
            this.dstBlendMode = dstBlendMode;
            return this;
        }
        public Material DisableBlending()
        {
            materialStates[0] = false;
            return this;
        }
        public Material EnableLogicOp(LogicOp logicOp)
        {
            materialStates[1] = true;
            this.logicOp = logicOp;
            return this;
        }
        public Material DisableLogicOp()
        {
            materialStates[1] = false;
            return this;
        }
        public Material EnableCullFace(TriangleFace cullFaceMode)
        {
            materialStates[2] = true;
            this.cullFaceMode = cullFaceMode;
            return this;
        }
        public Material DisableCullFace()
        {
            materialStates[2] = false;
            return this;
        }
        public Material EnableDepthTest(DepthFunction depthFunction)
        {
            materialStates[3] = true;
            this.depthFunction = depthFunction;
            return this;
        }
        public Material DisableDepthTest()
        {
            materialStates[3] = false;
            return this;
        }
        public Material Clone()
        {
            Material clone = new Material(shader);

            clone.materialStates = new BitArray(materialStates);
            clone.srcBlendMode = srcBlendMode;
            clone.dstBlendMode = dstBlendMode;
            clone.logicOp = logicOp;
            clone.cullFaceMode = cullFaceMode;
            clone.depthFunction = depthFunction;
            clone.texture2Ds = new List<(string, Texture2D)>(texture2Ds);
            clone.integers = new List<(string, int)>(integers);
            clone.floats = new List<(string, float)>(floats);
            clone.vector2s = new List<(string, Vector2)>(vector2s);
            clone.vector3s = new List<(string, Vector3)>(vector3s);
            clone.vector4s = new List<(string, Vector4)>(vector4s);

            return clone;
        }
        public Material AssignTexture(string textureName, Texture2D texture)
        {
            if (!texture2Ds.Exists(t => t.Item1 == textureName))
            {
                texture2Ds.Add((textureName, texture));
            }
            else
            {
                int index = texture2Ds.FindIndex(t => t.Item1 == textureName);
                if (index == -1)
                    Console.WriteLine($"Texture {textureName} is not present in material.");
                else
                    texture2Ds[index] = (textureName, texture);
            }

            return this;
        }
        public void Use()
        {
            shader.Use();

            //blending
            if (materialStates[0])
            {
                GL.Enable(EnableCap.Blend);
                GL.BlendFunc(srcBlendMode, dstBlendMode);
            }
            else
                GL.Disable(EnableCap.Blend);

            //logic op
            if (materialStates[1])
            {
                GL.Enable(EnableCap.ColorLogicOp);
                GL.LogicOp(logicOp);
            }
            else
                GL.Disable(EnableCap.ColorLogicOp);

            //cull face
            if (materialStates[2])
            {
                GL.Enable(EnableCap.CullFace);
                GL.CullFace(cullFaceMode);
            }
            else
                GL.Disable(EnableCap.CullFace);

            //depth test
            if (materialStates[3])
            {
                GL.Enable(EnableCap.DepthTest);
                GL.DepthFunc(depthFunction);
            }
            else
                GL.Disable(EnableCap.DepthTest);

            //textures
            if (texture2Ds.Count > 0)
            {
                for (int i = 0; i < texture2Ds.Count; i++)
                {
                    //TODO: increment and decrement light count depending on distance to determine the shadow texture offset
                    texture2Ds[i].Item2.Bind(GetTextureUnit(i + RenderPipeline.lights.Count));
                    shader.SetTexture(texture2Ds[i].Item1, i + RenderPipeline.lights.Count);
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