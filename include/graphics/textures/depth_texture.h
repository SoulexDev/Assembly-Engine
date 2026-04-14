#pragma once
#include "texture2d.h"

class DepthTexture {
public:
	unsigned int fbo{ 0 };
	unsigned int texture{ 0 };

	int width, height;

	DepthTexture();
	DepthTexture(int w, int h);
	void bind_framebuffer();
	void unbind_framebuffer();
	//void rescale_framebuffer(float width, float height);
	void destroy();

	operator unsigned int();
	operator Texture2D();
};