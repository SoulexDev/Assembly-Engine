using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace AssemblyEngine.Graphics
{
    public class Shader
    {
        public int id;
        public Dictionary<string, int> uniformLocations = new Dictionary<string, int>();

        public Shader(int id)
        {
            this.id = id;

            GL.GetProgram(id, GetProgramParameterName.ActiveUniforms, out int uniformCount);
            GL.GetProgram(id, GetProgramParameterName.ActiveUniformMaxLength, out int uniformLength);

            for (int i = 0; i < uniformCount; i++)
            {
                string key = GL.GetActiveUniform(id, i, out _, out _);
                uniformLocations.Add(key, GL.GetUniformLocation(id, key));
            }
        }
        public void Use()
        {
            GL.UseProgram(id);
        }
        public void SetInt(string name, int value)
        {
            if (uniformLocations.ContainsKey(name))
                GL.Uniform1(uniformLocations[name], value);
        }
        public void SetFloat(string name, float value)
        {
            if (uniformLocations.ContainsKey(name))
                GL.Uniform1(uniformLocations[name], value);
        }
        public void SetVector(string name, Vector2 value)
        {
            if (uniformLocations.ContainsKey(name))
                GL.Uniform2(uniformLocations[name], value);
        }
        public void SetVector(string name, Vector3 value)
        {
            if (uniformLocations.ContainsKey(name))
                GL.Uniform3(uniformLocations[name], value);
        }
        public void SetVector(string name, Vector4 value)
        {
            if (uniformLocations.ContainsKey(name))
                GL.Uniform4(uniformLocations[name], value);
        }
        public void SetMatrix4(string name, Matrix4 value, bool transpose = false)
        {
            if (uniformLocations.ContainsKey(name))
                GL.UniformMatrix4(uniformLocations[name], transpose, ref value);
        }
        public void SetTexture(string name, int value)
        {
            SetInt(name, value);
        }
        public static bool CreateShaderProgram(out int id, params (string, ShaderType)[] shaderInfo)
        {
            id = 0;
            int[] shaders = new int[shaderInfo.Length];

            for (int i = 0; i < shaderInfo.Length; i++)
            {
                if (!CreateAndCompileShader(ref shaders[i], shaderInfo[i].Item1, shaderInfo[i].Item2))
                    return false;
            }
            id = GL.CreateProgram();
            for (int i = 0; i < shaders.Length; i++)
            {
                GL.AttachShader(id, shaders[i]);
            }
            GL.LinkProgram(id);

            GL.GetProgram(id, GetProgramParameterName.LinkStatus, out int success);
            if (success != (int)All.True)
            {
                GL.GetShaderInfoLog(id, out string info);
                Console.WriteLine(info);
                return false;
            }

            for (int i = 0; i < shaders.Length; i++)
            {
                GL.DeleteShader(shaders[i]);
            }
            return true;
        }
        private static bool CreateAndCompileShader(ref int id, string shaderContent, ShaderType shaderType)
        {
            //Console.WriteLine("Creating shader of type: " + shaderType.ToString());
            id = GL.CreateShader(shaderType);
            GL.ShaderSource(id, shaderContent);
            GL.CompileShader(id);

            GL.GetShader(id, ShaderParameter.CompileStatus, out int success);
            if (success == (int)All.True)
                return true;
            else
            {
                GL.GetShaderInfoLog(id, out string info);
                Console.WriteLine(info);
                return false;
            }
        }

        //operators
        public static implicit operator Shader(int value) => new Shader(value);
    }
}
