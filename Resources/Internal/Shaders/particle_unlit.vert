#version 460 core

layout (location = 0) in vec3 aPos;
layout (location = 1) in vec3 aNorm;
layout (location = 2) in vec2 aUv;

out RenderParams{
	vec3 fragPos;
	//vec3 normal;
	vec2 texCoord;
	//vec4 lightSpaceFragPos;
} v_out;

uniform mat4 uModel;
uniform mat4 uView;
uniform mat4 uProjection;

uniform vec2 uSize;

void main(){
	v_out.texCoord = aUv;

	vec3 camRight = vec3(uView[0][0], uView[1][0], uView[2][0]);
	vec3 camUp = vec3(uView[0][1], uView[1][1], uView[2][1]);

	vec3 screenPos = camRight * aPos.x * uSize.x + camUp * aPos.y * uSize.y;
	vec4 screenCenter = uModel * vec4(0.0, 0.0, 0.0, 1.0);

	gl_Position = uProjection * uView * vec4(screenCenter.xyz + screenPos, 1.0);
}