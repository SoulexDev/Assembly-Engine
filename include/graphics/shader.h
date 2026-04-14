#pragma once
#include <map>
#include <string>

#include <glm/glm.hpp>
#include <glad/glad.h>

class Shader {
public:
	unsigned int shaderProgram;

	std::map<std::string, int> uniformLocations;

	Shader() {
		shaderProgram = 0;
	}
	Shader(unsigned int shaderProgram);
	void use();
	void set_int(std::string name, int value);
	void set_float(std::string name, float value);
	void set_vector(std::string name, glm::vec2 value);
	void set_vector(std::string name, glm::vec3 value);
	void set_vector(std::string name, glm::vec4 value);
	void set_matrix4(std::string name, glm::mat4 value, bool transpose = false);
	void set_texture(std::string name, unsigned int value);
	static bool compile_shader(unsigned int* shader);
	static bool create_shader(unsigned int* shader, const char* shaderContent, GLenum shaderType);
	static bool create_shader_program(unsigned int* shader, const char* vertRead, const char* fragRed, const char* geoRead);
};