#version 460 core
out vec4 FragColor;
  
in vec2 uv;

uniform sampler2D uScreenTexture;

void main()
{ 
    FragColor = vec4(pow(texture(uScreenTexture, uv).rgb, vec3(1.0 / 2.2)), 1.0);
}