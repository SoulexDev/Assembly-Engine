#include "../../include/graphics/shader.h"

#include <glad/glad.h>
#include <SDL3/SDL_opengl.h>
#include <glm/glm.hpp>
#include <glm/gtc/type_ptr.hpp>

#include <iostream>
#include <string>

Shader::Shader(unsigned int shaderProgram) {
	this->shaderProgram = shaderProgram;

	int uniformCount = 0;
	int uniformLength = 0;
	glGetProgramiv(this->shaderProgram, GL_ACTIVE_UNIFORMS, &uniformCount);
	glGetProgramiv(this->shaderProgram, GL_ACTIVE_UNIFORM_MAX_LENGTH, &uniformLength);
	
	//char* key = malloc(sizeof(char) * uniformLength);
	char* key = new char[uniformLength];
	GLsizei length = 0;
	GLint size = 0;
	GLenum type = 0;
	for (int i = 0; i < uniformCount; ++i)
	{
		glGetActiveUniform(this->shaderProgram, i, sizeof(char) * uniformLength, &length, &size, &type, key);
		int location = glGetUniformLocation(this->shaderProgram, key);
		std::string keyString = key;
		uniformLocations.insert({ keyString, location });
	}

	delete[] key;
}
void Shader::use() {
	glUseProgram(shaderProgram);
}
//TODO: optimize lookups
void Shader::set_int(std::string name, int value) {
	if (uniformLocations.find(name) != uniformLocations.end())
		glUniform1i(uniformLocations.at(name), value);
}
void Shader::set_float(std::string name, float value) {
	if (uniformLocations.find(name) != uniformLocations.end())
		glUniform1f(uniformLocations.at(name), value);
}
void Shader::set_vector(std::string name, glm::vec2 value) {
	if (uniformLocations.find(name) != uniformLocations.end())
		glUniform2f(uniformLocations.at(name), value.x, value.y);
}
void Shader::set_vector(std::string name, glm::vec3 value) {
	if (uniformLocations.find(name) != uniformLocations.end())
		glUniform3f(uniformLocations.at(name), value.x, value.y, value.z);
}
void Shader::set_vector(std::string name, glm::vec4 value) {
	if (uniformLocations.find(name) != uniformLocations.end())
		glUniform4f(uniformLocations.at(name), value.x, value.y, value.z, value.w);
}
void Shader::set_matrix4(std::string name, glm::mat4 value, bool transpose) {
	if (uniformLocations.find(name) != uniformLocations.end())
		glUniformMatrix4fv(uniformLocations.at(name), 1, transpose, glm::value_ptr(value));
}
void Shader::set_texture(std::string name, unsigned int value) {
	if (uniformLocations.find(name) != uniformLocations.end())
		glUniform1i(uniformLocations.at(name), value);
}
bool Shader::create_shader_program(unsigned int* shaderProgram, const char* vertRead, const char* fragRead, const char* geoRead) {
	//create vertex shader
	unsigned int vertShader;
	if (!create_shader(&vertShader, vertRead, GL_VERTEX_SHADER)) {
		//std::cout << "at :" << vertFilePath << std::endl;
		return false;
	}

	//create fragment shader
	unsigned int fragShader;
	if (!create_shader(&fragShader, fragRead, GL_FRAGMENT_SHADER)) {
		//std::cout << "at :" << fragFilePath << std::endl;
		return false;
	}

	//create shader program
	*shaderProgram = glCreateProgram();
	glAttachShader(*shaderProgram, vertShader);
	glAttachShader(*shaderProgram, fragShader);
	glLinkProgram(*shaderProgram);

	char infoLog[2048];
	int success;
	glGetProgramiv(*shaderProgram, GL_LINK_STATUS, &success);
	if (!success) {
		glGetProgramInfoLog(*shaderProgram, 2048, NULL, infoLog);
		std::cout << "Error: Shader compilation failed.\n" << infoLog << std::endl;
		return false;
	}

	//delete shaders
	glDeleteShader(vertShader);
	glDeleteShader(fragShader);
	return true;
}
bool Shader::create_shader(unsigned int* shader, const char* shaderContent, GLenum shaderType) {
	std::cout << "Creating shader of type: " << shaderType << std::endl;

	*shader = glCreateShader(shaderType);
	glShaderSource(*shader, 1, &shaderContent, nullptr);
	glCompileShader(*shader);

	if (!compile_shader(&*shader))
		return false;
	else
		return true;
}
bool Shader::compile_shader(unsigned int* shader) {
	int success;
	char infoLog[2048];
	glGetShaderiv(*shader, GL_COMPILE_STATUS, &success);

	if (success) {
		return true;
	}
	else {
		glGetShaderInfoLog(*shader, 2048, NULL, infoLog);
		std::cout << "Error: Shader compilation failed.\n" << infoLog << std::endl;
		return false;
	}
}