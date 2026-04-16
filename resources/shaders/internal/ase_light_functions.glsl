#include "internal/ase.glsl"

//normal distribution functions
float trowbridge_reitz(float nh, float glossiness){
    float roughnessSqr = pow2(1.0 - glossiness);
    float distribution = nh * nh * (roughnessSqr - 1.0) + 1.0;
    return roughnessSqr / (ASE_PI * distribution * distribution);
}
float trowbridge_reitz_anisotropic(float anisotropic, float nh, float hx, float hy, float glossiness){

    float aspect = sqrt(1.0 - anisotropic * 0.9);
    float x = max(0.001, pow2(1.0 - glossiness) / aspect) * 5.0;
    float y = max(0.001, pow2(1.0 - glossiness) * aspect) * 5.0;
    
    return 1.0 / (ASE_PI * x * y * pow2(pow2(hx / x) + pow2(hy / y) + nh * nh));
}

//geometric shadowing functions
float schlick_ggx(float nl, float nv, float glossiness){
    float k = (1.0 - glossiness) * 0.5;

    float smithL = (nl) / (nl * (1.0 - k) + k);
    float smithV = (nv) / (nv * (1.0 - k) + k);

	return smithL * smithV;
}

//fresnel
float schlick_fresnel(float i){
    float x = clamp(1.0 - i , 0.0, 1.0);
    return pow5(x);
}

//normal incidence reflection calculation
float F0 (float nl, float nv, float lh, float glossiness){
    float fresnelDiffuse90 = 0.5 + 2.0 * lh * lh * (1.0 - glossiness);
    return mix(1.0, fresnelDiffuse90, schlick_fresnel(nl)) * mix(1.0, fresnelDiffuse90, schlick_fresnel(nv));
}

//specular
vec3 calculate_specular(vec3 specularColor, float nl, float nv, float distribution, float fresnel, float geometricShadow){
    return specularColor * (distribution * fresnel * geometricShadow / (4 * nl * nv));
}