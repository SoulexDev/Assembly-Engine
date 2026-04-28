#version 460 core
out vec4 FragColor;

in vec3 texCoord;

void main(){
	vec3 dir = normalize(texCoord);
	vec3 color = vec3(0.0);

	vec3 skyColor = vec3(0.4, 0.55, 1);
	vec3 horizonColor = vec3(0.6, 0.75, 0.9);
	//vec3 bottomColor = vec3(0.2, 0.5, 0.6);

	color = mix(horizonColor, skyColor, pow(max(dir.y, 0.0), 0.5));

	FragColor = vec4(color, 1.0);
}