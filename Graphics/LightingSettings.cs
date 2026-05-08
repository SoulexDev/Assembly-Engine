using OpenTK.Mathematics;
using System.Text.Json.Serialization;

namespace AssemblyEngine.Graphics
{
    public enum AmbientLightingMode 
    {
        Color, 
        Gradient, 
        Skybox 
    };

    [JsonSerializable(typeof(LightingSettings), GenerationMode = JsonSourceGenerationMode.Default)]
    public class LightingSettings
    {
        public bool useFog = false;
        public Color4 fogColor = Color4.DarkSlateGray;

        public AmbientLightingMode ambientLightingMode = AmbientLightingMode.Color;

        /// <summary>
        /// The ambient lighting color for AmbientLightingMode.Color
        /// </summary>
        public Color4 ambientLightingColor = Color4.DarkSlateGray;

        /// <summary>
        /// The top ambient lighting color for AmbientLightingMode.Gradient
        /// </summary>
        public Color4 ambientLightingColorTop = Color4.AliceBlue;

        /// <summary>
        /// The middle ambient lighting color for AmbientLightingMode.Gradient
        /// </summary>
        public Color4 ambientLightingColorMiddle = Color4.DarkSlateGray;

        /// <summary>
        /// The bottom ambient lighting color for AmbientLightingMode.Gradient
        /// </summary>
        public Color4 ambientLightingColorBottom = Color4.DimGray;
    }
}