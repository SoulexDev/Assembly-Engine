#pragma once
#include "shader.h"
#include "textures/texture2d.h"

#include <vector>
#include <string>

#include <glad/glad.h>
#include <glm/glm.hpp>

class Material {
public:
	Shader shader;

	bool blendingEnabled = false;
	GLenum srcBlendMode = GL_SRC_ALPHA;
	GLenum dstBlendMode = GL_ONE_MINUS_SRC_ALPHA;

	bool logicOpEnabled = false;
	GLenum logicOp = GL_NOOP;

	bool cullFaceEnabled = true;
	GLenum cullFaceMode = GL_BACK;

	bool depthTestEnabled = true;
	GLenum depthFunction = GL_LEQUAL;

	std::vector<std::pair<std::string, Texture2D>> texture2Ds;
	std::vector<std::pair<std::string, int>> integers;
	std::vector<std::pair<std::string, float>> floats;
	std::vector<std::pair<std::string, glm::vec2>> vec2s;
	std::vector<std::pair<std::string, glm::vec3>> vec3s;
	std::vector<std::pair<std::string, glm::vec4>> vec4s;

	Material() {

	}
	Material(
		Shader& shader,
		bool blendingEnabled = false,
		GLenum srcBlendMode = GL_SRC_ALPHA,
		GLenum dstBlendMode = GL_ONE_MINUS_SRC_ALPHA,
		bool logicOpEnabled = false,
		GLenum logicOp = GL_NOOP,
		bool cullFaceEnabled = true,
		GLenum cullFaceMode = GL_BACK,
		bool depthTestEnabled = true,
		GLenum depthFunction = GL_LEQUAL) {

		this->shader = shader;
		this->blendingEnabled = blendingEnabled;
		this->srcBlendMode = srcBlendMode;
		this->dstBlendMode = dstBlendMode;
		this->logicOpEnabled = logicOpEnabled;
		this->logicOp = logicOp;
		this->cullFaceEnabled = cullFaceEnabled;
		this->depthTestEnabled = depthTestEnabled;
		this->depthFunction = depthFunction;
	}
	void use();
	void destroy();
	GLenum get_texture_unit(int index);
};