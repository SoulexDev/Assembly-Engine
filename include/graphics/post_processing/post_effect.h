#pragma once
#include "../textures/render_texture.h"
#include "../shader.h"
#include "../material.h"

class PostEffect {
private:
	//RenderTexture renderTex;
	Shader shader;
public:
	PostEffect();
	PostEffect(Shader shader);

	void render(unsigned int texture);
};
