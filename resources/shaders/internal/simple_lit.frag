#version 460 core
out vec4 FragColor;

in RenderParams{
	vec3 fragPos;
	vec3 normal;
	vec2 texCoord;
	vec4 lightSpaceFragPos;
} v_in;

uniform sampler2D uMainTex;
//uniform sampler2D uNormal;
//uniform sampler2D uGlossiness;
//uniform sampler2D uSpecular;
uniform sampler2D uShadowTex0;

uniform float uTime;
uniform vec3 uLightPos;
uniform vec3 uViewPos;

vec2 vogel_disk_sample(int sampleIndex, int sampleCount, float phi){
	float r = sqrt(sampleIndex + 0.5) / sqrt(sampleCount);
	float theta = sampleIndex * 2.4 + phi;

	return vec2(cos(theta), sin(theta)) * r;
}
float interleaved_gradient_noise(vec2 screenPos){
	vec3 magic = vec3(0.06711056, 0.00583715, 52.9829189);
	return fract(magic.z * fract(dot(screenPos, magic.xy)));
}
float avg_blockers_depth_to_penumbra(float lightSize, float testingZ, float avgBlockerDepth){
	return lightSize * (testingZ - avgBlockerDepth) / avgBlockerDepth;
}
float penumbra(float gradientNoise, vec2 sampleCenter, float testingZ, int sampleCount){
	float avgBlockerDepth = 0.0;
	float blockerCount = 0.0;

	for (int i = 0; i < sampleCount; i++){
		vec2 samplePoint = vogel_disk_sample(i, sampleCount, gradientNoise);
		samplePoint = sampleCenter + samplePoint * 0.05;

		float depth = texture(uShadowTex0, samplePoint).r;

		if (depth < testingZ){
			avgBlockerDepth += depth;
			blockerCount += 1.0;
		}
	}

	if (blockerCount > 0.0){
		avgBlockerDepth /= blockerCount;
		return avg_blockers_depth_to_penumbra(10.0, testingZ, avgBlockerDepth);
	}
	else{
		return 0.25;
	}
}
vec2 to_pixel_point(vec2 point, float texels){
	float r = 1.0 / texels;
	return floor(point * texels) * r + r * 0.5;
}
float calculate_shadow(vec4 lightSpaceFragPos, float bias){
	vec3 projCoords = lightSpaceFragPos.xyz / lightSpaceFragPos.w;
	projCoords *= 0.5;
	projCoords += 0.5;

	if (projCoords.z > 1.0)
		return 1.0;

	float fragDepth = projCoords.z;

	int sampleCount = 16;
	float texelSize = 1.0 / 2048.0;
	
	float noise = fract(gl_FragCoord.x + gl_FragCoord.y) + interleaved_gradient_noise(gl_FragCoord.xy * gl_FragCoord.yx);
	//float p = penumbra(noise, projCoords.xy, texture(uShadowTex0, projCoords.xy).r, sampleCount / 2);

	float shadow = 0.0;
    for (int i = 0; i < sampleCount; i++)
    {
		vec2 samplePoint = projCoords.xy + vogel_disk_sample(i, sampleCount, uTime / max(noise, 0.01)) * texelSize * 4;
        
		float lightDepth = texture(uShadowTex0, to_pixel_point(samplePoint, 2048.0)).r;

		shadow += fragDepth - bias * 2 > lightDepth ? 0.0 : 1.0;
    }

	//if the depth of the geometry, relative to the sun, is larger than the depth from the sun to the nearest visible point
	return shadow / float(sampleCount);
}

void main(){
	vec3 lightDir = normalize(uLightPos - v_in.fragPos);
	vec3 viewDir = normalize(uViewPos - v_in.fragPos);
	
	vec3 diffuse = texture(uMainTex, v_in.texCoord).rgb;
	
	float bias = 0.001;
	float normalBias = 0.001;
	float finalBias = max((1.0 - dot(v_in.normal, lightDir)) * normalBias, bias);
	//finalBias = gl_FrontFacing ? finalBias : 0.0;
	
	float nl = pow(dot(v_in.normal, lightDir) * 0.5 + 0.5, 2);
	nl = 1;
	float shadow = (calculate_shadow(v_in.lightSpaceFragPos, finalBias) * nl + 0.2);
	FragColor = vec4(diffuse * shadow, 1.0);
	//FragColor = vec4(v_in.texCoord.x, v_in.texCoord.y, 0.0, 1.0);
}