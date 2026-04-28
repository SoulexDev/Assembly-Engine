#version 460 core
layout (location = 0) in vec2 aPos;
layout (location = 1) in vec2 aUv;

out vec2 uv;

uniform vec2 uCanvasSize;
uniform vec2 uPosition;
uniform vec2 uSize;
//uniform vec2 uAnchor;

void main(){
	vec2 scale = uSize / uCanvasSize;
	//vec2 nAnchor = uAnchor - 0.5;
	//vec2 anchor = mix(vec2(0.0), uSize, uAnchor);
	vec2 position = (uPosition + uSize * 0.5) / uCanvasSize;
	position *= 2.0;
	position -= 1.0;

	gl_Position = vec4(position + aPos * scale, 0.0, 1.0);
	uv = aUv;
}