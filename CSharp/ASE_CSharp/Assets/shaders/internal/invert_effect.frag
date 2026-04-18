#version 460 core
out vec4 FragColor;
  
in vec2 uv;

uniform sampler2D uScreenTexture;

void main()
{ 
    FragColor = vec4(1.0 - texture(uScreenTexture, uv).rgb, 1.0);
}