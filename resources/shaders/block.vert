#version 460 core
layout (location = 0) in vec3 aPos;
layout (location = 1) in vec3 aNorm;
layout (location = 2) in vec2 aUv;
layout (location = 3) in float aAo;

out VS_OUT {
	vec3 fragPos;
	vec3 normal;
	vec2 texCoord;
	vec4 lightSpaceFragPos;
	float ao;
} vs_out;

uniform mat4 uModel;
uniform mat4 uView;
uniform mat4 uProjection;
uniform mat4 uLightSpace;

void main(){
	vs_out.fragPos = vec3(uModel * vec4(aPos, 1.0));
	vs_out.normal = transpose(inverse(mat3(uModel))) * aNorm;
	vs_out.texCoord = aUv;
	vs_out.ao = aAo;
	vs_out.lightSpaceFragPos = uLightSpace * vec4(vs_out.fragPos, 1.0);
	gl_Position = uProjection * uView * vec4(vs_out.fragPos, 1.0);
}