#include "../../../include/graphics/textures/texture2d.h"

#define STB_IMAGE_IMPLEMENTATION
#include "../../../lib/stb_image.h"

#include <iostream>
#include <string>

Texture2D::Texture2D(unsigned int textureID) {
	this->textureID = textureID;
}
Texture2D::Texture2D(std::string filePath) {
	int width, height, nChannels;

	//generate and bind texture
	glGenTextures(1, &textureID);
	glBindTexture(GL_TEXTURE_2D, textureID);

	//set texture parameters
	glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_S, GL_REPEAT);
	glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_T, GL_REPEAT);
	glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_NEAREST_MIPMAP_NEAREST);
	glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_NEAREST);

	//load texture data
	stbi_set_flip_vertically_on_load(true);
	unsigned char* data = stbi_load(filePath.c_str(), &width, &height, &nChannels, 0);

	//assign texture data
	if (data) {
		
		if (nChannels == 3)
			glTexImage2D(GL_TEXTURE_2D, 0, GL_RGB, width, height, 0, GL_RGB, GL_UNSIGNED_BYTE, data);
		else if (nChannels == 4)
			glTexImage2D(GL_TEXTURE_2D, 0, GL_RGBA, width, height, 0, GL_RGBA, GL_UNSIGNED_BYTE, data);

		glGenerateMipmap(GL_TEXTURE_2D);
	}

	//free texture data from memory
	stbi_image_free(data);

	//unbind texture
	glBindTexture(GL_TEXTURE_2D, 0);
}
void Texture2D::bind(GLenum textureUnit){
	glActiveTexture(textureUnit);
	glBindTexture(GL_TEXTURE_2D, textureID);
}
void Texture2D::unbind(GLenum textureUnit) {
	glActiveTexture(textureUnit);
	glBindTexture(GL_TEXTURE_2D, 0);
}
void Texture2D::destroy() {
	glDeleteTextures(1, &textureID);
}
Texture2D::operator unsigned int() {
	return textureID;
}