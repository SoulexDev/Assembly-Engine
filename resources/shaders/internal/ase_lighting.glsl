//rendering
uniform vec3 uLightPos;
uniform vec3 uViewPos;

//time
uniform float uTime;

//textures
uniform sampler2D shadow_map;

struct Light{
	
}
struct ShadeParams{
	vec3 albedo;
	vec3 normal;
	vec3_specular;
	float glossiness;
}
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
        vec2 pos = projCoords.xy + u * texelSize * 2;
        
        float lightDepthPoint = texture(shadow_map, floor(pos * 1024.0) * texelSize + vec2(texelSize * 0.5)).r;
		float lightDepthLinear = texture(shadow_map, pos).r;
		float lightDepth = lightDepthPoint > lightDepthLinear ? lightDepthPoint : lightDepthLinear;

		shadow += fragDepth - bias > lightDepth ? 0.0 : 1.0;
    }

	//if the depth of the geometry, relative to the sun, is larger than the depth from the sun to the nearest visible point
	return shadow / sampleCount;
}
vec3 lighting_pbr(ShadeParams p){
	vec3 lightDir = normalize(uLightPos - vs_in.fragPos);
	vec3 viewDir = normalize(uViewPos - vs_in.fragPos);

	float nl = clamp(dot(p.normal, lightDir), 0.0, 1.0);
	//float rv = clamp(dot(reflect(viewDir, normal), viewDir), 0.0, 1.0);

	float bias = 0.001;
	float normalBias = 0.0005;

	float finalBias = max((1.0 - dot(p.normal, lightDir)) * normalBias, bias);

	finalBias = gl_FrontFacing ? finalBias : 0.0;
	float shadow = calculate_shadow(vs_in.lightSpaceFragPos, finalBias);
	//vec3 spec = vec3(pow(rv, glossiness * 100.0)) * specular;
	float dist = clamp(distance(vs_in.fragPos, uViewPos) / 16.0, 0.0, 1.0);
	shadow = mix(shadow, 1.0, dist);
	return p.albedo * (nl * shadow + vec3(0.125, 0.1, 0.05));
}
vec3 lighting_npr_illustrative(){

}
vec3 lighting_npr_simple(){

}
vec3 lighting_npr_anime(){

}