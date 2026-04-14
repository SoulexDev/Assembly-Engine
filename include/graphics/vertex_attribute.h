#pragma once
#include <glad/glad.h>
#include <vector>

class VertexAttribute {
public:
	GLenum pointerType;
	unsigned int sizeInBytes;
	unsigned int componentsCount;

	static VertexAttribute Int32;
	static VertexAttribute Int64;
	static VertexAttribute Float;
	static VertexAttribute Vector2;
	static VertexAttribute Vector3;
	static VertexAttribute Vector4;

	VertexAttribute(GLenum pointerType, unsigned int sizeInBytes, unsigned int componentsCount);
	static VertexAttribute create(GLenum pointerType, unsigned int sizeInBytes, unsigned int componentsCount);
	static unsigned int get_total_size_in_bytes(std::vector<VertexAttribute> attributes);
};
