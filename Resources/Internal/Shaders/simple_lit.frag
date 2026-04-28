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
uniform vec4 uLightPos0;
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
vec2 to_pixel_point(vec2 point, float texels){
	float r = 1.0 / texels;
	return floor(point * texels) * r + r * 0.5;
}
float get_blocker_dist(float gradientNoise, vec2 uv, float zReciever, int sampleCount){
	float dist = 0;
	int blockerCount = 0;
	float texelSize = 1.0 / 2048.0;
	for	(int i = 0; i < sampleCount; i++){
		vec2 samplePoint = uv + vogel_disk_sample(i, sampleCount, gradientNoise) * texelSize * 16;
		float depth = texture(uShadowTex0, samplePoint).r;

		if (zReciever > depth){
			blockerCount++;
			dist += depth;
		}
	}

	return dist / float(blockerCount);
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
	float d = fragDepth - bias;
	float p = get_blocker_dist(noise, projCoords.xy, d, sampleCount);
	p = ((d - p) / p) * 32;
	p = max(p, 2.0);

	float shadow = 0.0;
    for (int i = 0; i < sampleCount; i++)
    {
		vec2 samplePoint = projCoords.xy + vogel_disk_sample(i, sampleCount, uTime / max(noise, 0.01)) * texelSize * p;
        
		float lightDepthPoint = texture(uShadowTex0, to_pixel_point(samplePoint, 2048.0)).r;
		float lightDepthLinear = texture(uShadowTex0, samplePoint).r;
		float lightDepth = max(lightDepthPoint, lightDepthLinear);

		shadow += fragDepth - bias * p * 0.5 > lightDepth ? 0.0 : 1.0;
    }
	return shadow / float(sampleCount);
}

void main(){
	vec3 normal = normalize(v_in.normal);

	vec3 lightDir = normalize(uLightPos0.w == 1 ? -uLightPos0.xyz : (uLightPos0.xyz - v_in.fragPos));
	vec3 viewDir = normalize(uViewPos - v_in.fragPos);
	
	vec3 diffuse = texture(uMainTex, v_in.texCoord).rgb;
	
	float bias = 0.001;
	float normalBias = 0.001;
	float finalBias = max((1.0 - dot(normal, lightDir)) * normalBias, bias);
	//finalBias = gl_FrontFacing ? finalBias : 0.0;
	
	float nl = dot(normal, lightDir) * 0.5 + 0.5;
	nl *= nl;
	float shadow = (calculate_shadow(v_in.lightSpaceFragPos, finalBias) * nl + 0.2);
	shadow = min(shadow, 1.0);

	FragColor = vec4(diffuse * shadow, 1.0);
	//FragColor = vec4(v_in.texCoord.x, v_in.texCoord.y, 0.0, 1.0);
}