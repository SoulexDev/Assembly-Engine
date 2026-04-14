#version 460 core
in RenderParams{
	vec3 fragPos;
	vec3 normal;
	vec2 texCoord;
	vec4 lightSpaceFragPos;
	float ao;
} v_in;

#include "internal/ase_lighting.glsl"
//#extension GL_ARB_shading_language_include : enable ??

out vec4 FragColor;

uniform sampler2D tex_atlas;

void main(){
	ShadeParams p;
	p.albedo = texture(tex_atlas, v_in.texCoord).rgb * v_in.ao;
	p.normal = v_in.normal;
	p.specular = vec3(1.0);
	p.glossiness = 0.0;
	FragColor = vec4(lighting_pbr(p), 1.0);
}