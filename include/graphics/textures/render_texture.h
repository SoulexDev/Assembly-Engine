#pragma once
#include "texture2d.h"

class RenderTexture {
public:
	unsigned int fbo{0};
	unsigned int rbo{0};
	unsigned int texture{0};
	bool depthOnly;

	int width, height;

	RenderTexture();
	RenderTexture(int w, int h, bool depthOnly = false);
	void bind_framebuffer();
	void unbind_framebuffer();
	void rescale_framebuffer(float width, float height);
	void destroy();

	operator unsigned int();
	operator Texture2D();
};