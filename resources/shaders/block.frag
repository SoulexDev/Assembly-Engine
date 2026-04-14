#version 460 core
//#extension GL_ARB_shading_language_include : enable ??
//#include "./internal/ase_lighting.glsl"

in VS_OUT {
	vec3 fragPos;
	vec3 normal;
	vec2 texCoord;
	vec4 lightSpaceFragPos;
	float ao;
} vs_in;

out vec4 FragColor;

//uniform sampler2D tex_atlas;

void main(){
//	ShadeParams p;
//	p.albedo = texture(tex_atlas, vs_in.texCoord).rgb * vs_in.ao;
//	p.normal = vs_in.normal;
//	p.specular = vec3(1.0);
//	p.glossiness = 0.0;
	FragColor = vec4(/*(lighting_pbr(p)*/1.0);
}