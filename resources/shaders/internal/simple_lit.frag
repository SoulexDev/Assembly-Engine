#version 460 core
out vec4 FragColor;

in RenderParams{
	vec3 fragPos;
	vec3 normal;
	vec2 texCoord;
	vec4 lightSpaceFragPos;
} v_in;

uniform sampler2D uMainTex;
uniform sampler2D uShadowTex0;

uniform float uTime;
uniform vec3 uLightPos;
uniform vec3 uViewPos;

float calculate_shadow(vec4 lightSpaceFragPos, float bias){
	vec3 projCoords = lightSpaceFragPos.xyz / lightSpaceFragPos.w;
	projCoords *= 0.5;
	projCoords += 0.5;

	if (projCoords.z > 1.0)
		return 1.0;

	float texelSize = 1.0 / 1024.0;
	float fragDepth = projCoords.z;

	int sampleCount = 16;

	float shadow;
    for (int i = 0; i < sampleCount; i++)
    {
        // Calculate Vogel disk sample
        float theta = 2.4 * float(i) + gl_FragCoord.x + gl_FragCoord.y + uTime * 16.0;
        float r = sqrt(float(i) + 0.5) / sqrt(float(sampleCount));
        vec2 u = r * vec2(cos(theta), sin(theta));
        vec2 pos = projCoords.xy + u * texelSize;
        
        float lightDepthPoint = texture(uShadowTex0, floor(pos * 1024.0) * texelSize + vec2(texelSize * 0.5)).r;
		float lightDepthLinear = texture(uShadowTex0, pos).r;
		float lightDepth = lightDepthPoint > lightDepthLinear ? lightDepthPoint : lightDepthLinear;

		shadow += fragDepth - bias > lightDepth ? 0.0 : 1.0;
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