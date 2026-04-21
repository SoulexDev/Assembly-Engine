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

void main(){
	v_out.texCoord = aUv;

	mat4 mv = uView * uModel;
	mv[0][0] = 1.0;
	mv[0][1] = 0.0;
	mv[0][2] = 0.0;
	mv[1][0] = 0.0;
	mv[1][1] = 1.0;
	mv[1][2] = 0.0;
	mv[2][0] = 0.0;
	mv[2][1] = 0.0;
	mv[2][2] = 1.0;

	gl_Position = uProjection * mv * vec4(aPos, 1.0);
}