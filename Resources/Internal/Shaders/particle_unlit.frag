#version 460 core
out vec4 FragColor;

in RenderParams{
	vec3 fragPos;
	vec2 texCoord;
} v_in;

uniform sampler2D uMainTex;

uniform float uTime;
//uniform vec3 uLightPos;
//uniform vec3 uViewPos

void main(){
	FragColor = texture(uMainTex, v_in.texCoord);
}