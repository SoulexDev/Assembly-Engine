#version 460 core
out vec4 FragColor;
  
in vec2 uv;

uniform sampler2D uMainTex;
uniform vec4 uColor;

void main()
{
    FragColor = texture(uMainTex, uv) * uColor;
}