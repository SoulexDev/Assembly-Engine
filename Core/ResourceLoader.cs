using ASE.Graphics;
using OpenTK.Graphics.OpenGL4;
using Assimp;
using System.Text.RegularExpressions;
using Assimp.Unmanaged;
using Assimp.Configs;

namespace ASE
{
    public class ResourceLoader
    {
        private static Dictionary<ShaderType, string> shaderFileExtensions = new Dictionary<ShaderType, string>
        {
            { ShaderType.VertexShader, ".vert" },
            { ShaderType.GeometryShader, ".geo" },
            { ShaderType.FragmentShader, ".frag" },
            { ShaderType.ComputeShader, ".comp" },
            { ShaderType.TessControlShader, ".tesc" },
            { ShaderType.TessEvaluationShader, ".tese" }
        };
        private static AssimpContext importer = new AssimpContext();

        public static void LoadResource(out Texture2D texture, string filePath)
        {
            filePath = Path.Combine(Core.AssetsPath, filePath);

            texture = new Texture2D(filePath);
        }
        //public static bool LoadResource(out Shader shader, string filePath)
        //{
        //    filePath = Path.Combine(Core.AssetsPath, filePath);

        //    string vertRead = File.ReadAllText(filePath + ".vert");
        //    if (vertRead == string.Empty)
        //    {
        //        shader = -1;
        //        return false;
        //    }
        //    Console.WriteLine(vertRead);

        //    string fragRead = File.ReadAllText(filePath + ".frag");
        //    if (fragRead == string.Empty)
        //    {
        //        shader = -1;
        //        return false;
        //    }
        //    Console.WriteLine(fragRead);

        //    if (Shader.CreateShaderProgram(out int id, vertRead, fragRead))
        //    {
        //        shader = id;
        //        return true;
        //    }
        //    else
        //    {
        //        shader = -1;
        //        return false;
        //    }
        //}
        public static bool LoadResource(out Shader shader, params (string, ShaderType)[] fileInfo)
        {
            (string, ShaderType)[] shaderInfo = new (string, ShaderType)[fileInfo.Length];

            for (int i = 0; i < fileInfo.Length; i++)
            {
                var sInfo = shaderInfo[i];

                string filePath = Path.Combine(Core.AssetsPath, fileInfo[i].Item1);

                sInfo.Item1 = File.ReadAllText(filePath + shaderFileExtensions[fileInfo[i].Item2]);
                sInfo.Item2 = fileInfo[i].Item2;

                if (sInfo.Item1 == string.Empty)
                {
                    shader = -1;
                    return false;
                }
                Console.WriteLine(sInfo.Item1);

                shaderInfo[i] = sInfo;
            }

            if (Shader.CreateShaderProgram(out int id, shaderInfo))
            {
                shader = id;
                return true;
            }
            else
            {
                shader = -1;
                return false;
            }
        }
        public static bool LoadResource(out Renderable renderable, string filePath)
        {
            string fileType = Path.GetExtension(filePath);

            using (Stream stream = File.OpenRead(filePath))
            {
                Scene scene = importer.ImportFileFromStream(stream, fileType);

                if (scene == null || ((scene.SceneFlags & SceneFlags.Incomplete) != 0) || scene.RootNode == null)
                {
                    Console.WriteLine("Failed to import model " + filePath);
                    renderable = null;
                    return false;
                }
            }
            renderable = null;
            return true;
        } 
        //private static string PreprocessShaderContent(string shaderRead)
        //{
        //Regex includeRegex = new Regex(@"^#include\s*(?:<([^>]+)>|""([^""]+)"")");

        //while (Regex)
        //{

        //}
        //}
    }
}