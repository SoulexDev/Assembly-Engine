#pragma once
#include <string>
#include <glad/glad.h>

class Texture2D{
public:
	unsigned int textureID;

	Texture2D(unsigned int textureID);
	Texture2D(std::string filePath);
	void bind(GLenum textureUnit);
	void unbind(GLenum textureUnit);
	void destroy();
	operator unsigned int();
};
