#pragma once
#include "vertex_attribute.h"

#include <glad/glad.h>

#include <vector>

class Mesh {
public:
	GLuint vao;
	GLuint vbo;
	GLuint ebo;

	int count;

	bool usingIndices = false;
	bool generatedBuffers = false;
	GLenum primitiveType;
	GLenum drawType;

	Mesh();
	/*Mesh(
		float vertices[],
		unsigned int indices[],
		std::vector<VertexAttribute> attributes,
		bool useIndices = true,
		GLenum primitiveType = GL_TRIANGLES,
		GLenum drawType = GL_STATIC_DRAW);*/

	Mesh(std::vector<float> vertices,
		std::vector<VertexAttribute> attributes,
		GLenum primitiveType = GL_TRIANGLES,
		GLenum drawType = GL_STATIC_DRAW);

	Mesh(
		std::vector<float> vertices,
		std::vector<unsigned int> indices,
		std::vector<VertexAttribute> attributes,
		GLenum primitiveType = GL_TRIANGLES,
		GLenum drawType = GL_STATIC_DRAW);

	Mesh* set_mesh(std::vector<float> vertices, std::vector<VertexAttribute> attributes);
	Mesh* set_mesh(std::vector<float> vertices, std::vector<unsigned int> indices, std::vector<VertexAttribute> attributes);
	void draw();
	void destroy();
};