#version 460 core

layout (location = 0) in vec3 aPos;
layout (location = 1) in vec3 aNorm;
layout (location = 2) in vec2 aUv;

out RenderParams{
	vec3 fragPos;
	vec3 normal;
	vec2 texCoord;
	vec4 lightSpaceFragPos;
} v_out;

uniform mat4 uModel;
uniform mat4 uView;
uniform mat4 uProjection;
uniform mat4 uLightProjection;
uniform mat4 uLightView;

void main(){
	v_out.fragPos = vec3(uModel * vec4(aPos, 1.0));
	v_out.normal = transpose(inverse(mat3(uModel))) * aNorm;
	v_out.texCoord = aUv;
	v_out.lightSpaceFragPos = uLightProjection * uLightView * vec4(v_out.fragPos, 1.0);
	gl_Position = uProjection * uView * vec4(v_out.fragPos, 1.0);
}