#include "../../include/graphics/vertex_attribute.h"

#include <vector>
#include <glm/glm.hpp>

VertexAttribute VertexAttribute::Int32 = VertexAttribute::create(GL_INT, sizeof(int32_t), 1);
VertexAttribute VertexAttribute::Int64 = VertexAttribute::create(GL_INT, sizeof(int64_t), 1);
VertexAttribute VertexAttribute::Float = VertexAttribute::create(GL_FLOAT, sizeof(float), 1);
VertexAttribute VertexAttribute::Vector2 = VertexAttribute::create(GL_FLOAT, sizeof(glm::vec2), 2);
VertexAttribute VertexAttribute::Vector3 = VertexAttribute::create(GL_FLOAT, sizeof(glm::vec3), 3);
VertexAttribute VertexAttribute::Vector4 = VertexAttribute::create(GL_FLOAT, sizeof(glm::vec4), 4);

VertexAttribute::VertexAttribute(GLenum pointerType, unsigned int sizeInBytes, unsigned int componentsCount) {
	this->pointerType = pointerType;
	this->sizeInBytes = sizeInBytes;
	this->componentsCount = componentsCount;
}
VertexAttribute VertexAttribute::create(GLenum pointerType, unsigned int sizeInBytes, unsigned int componentsCount) {
	return *new VertexAttribute(pointerType, sizeInBytes, componentsCount);
}
unsigned int VertexAttribute::get_total_size_in_bytes(std::vector<VertexAttribute> attributes) {
	unsigned int size = 0;
	for (int i = 0; i < attributes.size(); ++i)
	{
		size += attributes[i].sizeInBytes;
	}
	return size;
}