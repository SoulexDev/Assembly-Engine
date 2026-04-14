#include "../../include/graphics/mesh.h"
#include "../../include/core/console.h"

#include <glad/glad.h>
#include <iterator>

Mesh::Mesh() {
    vao = 0;
    vbo = 0;
    ebo = 0;
    count = 0;
    primitiveType = GL_TRIANGLES;
    drawType = GL_STATIC_DRAW;
}
Mesh::Mesh(
    std::vector<float> vertices,
    std::vector<VertexAttribute> attributes,
    GLenum primitiveType,
    GLenum drawType) {

    this->primitiveType = primitiveType;
    this->drawType = drawType;
    set_mesh(vertices, attributes);
}
Mesh::Mesh(
    std::vector<float> vertices,
    std::vector<unsigned int> indices,
    std::vector<VertexAttribute> attributes,
    GLenum primitiveType,
    GLenum drawType) {

    this->primitiveType = primitiveType;
    this->drawType = drawType;
    set_mesh(vertices, indices, attributes);
}
Mesh* Mesh::set_mesh(std::vector<float> vertices, std::vector<VertexAttribute> attributes) {
    if (vertices.size() <= 0)
        return nullptr;

    usingIndices = false;

    //generate vao, vbo, and ebo
    if (!generatedBuffers) {
        glGenVertexArrays(1, &vao);
        glGenBuffers(1, &vbo);
    }

    //bind vao
    glBindVertexArray(vao);

    //assign vbo data
    count = vertices.size();

    glBindBuffer(GL_ARRAY_BUFFER, vbo);
    glBufferData(GL_ARRAY_BUFFER, count * sizeof(float), &vertices[0], drawType);

    //create vertex attributes
    unsigned int stride = VertexAttribute::get_total_size_in_bytes(attributes);
    unsigned int offset = 0;
    for (int i = 0; i < attributes.size(); ++i)
    {
        VertexAttribute attrib = attributes[i];

        glVertexAttribPointer(i, attrib.componentsCount, attrib.pointerType, GL_FALSE, stride, (void*)offset);
        glEnableVertexAttribArray(i);
        offset += attrib.sizeInBytes;
    }

    generatedBuffers = true;
    return this;
}
Mesh* Mesh::set_mesh(std::vector<float> vertices, std::vector<unsigned int> indices, std::vector<VertexAttribute> attributes) {
    if (vertices.size() <= 0 || indices.size() <= 0)
        return nullptr;

    usingIndices = true;

    //generate vao, vbo, and ebo
    if (!generatedBuffers) {
        glGenVertexArrays(1, &vao);
        glGenBuffers(1, &vbo);
        glGenBuffers(1, &ebo);
    }

    //bind vao
    glBindVertexArray(vao);

    //assign vbo data
    glBindBuffer(GL_ARRAY_BUFFER, vbo);
    glBufferData(GL_ARRAY_BUFFER, vertices.size() * sizeof(float), &vertices[0], drawType);

    //assign ebo data
    count = indices.size();

    glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, ebo);
    glBufferData(GL_ELEMENT_ARRAY_BUFFER, count * sizeof(unsigned int), &indices[0], drawType);

    //create vertex attributes
    unsigned int stride = VertexAttribute::get_total_size_in_bytes(attributes);
    unsigned int offset = 0;
    for (int i = 0; i < attributes.size(); ++i)
    {
        VertexAttribute attrib = attributes[i];

        glVertexAttribPointer(i, attrib.componentsCount, attrib.pointerType, GL_FALSE, stride, (void*)offset);
        glEnableVertexAttribArray(i);
        offset += attrib.sizeInBytes;
    }

    generatedBuffers = true;
    return this;
}
void Mesh::draw() {
    if (!generatedBuffers)
        return;

    glBindVertexArray(vao);
    if (usingIndices)
        glDrawElements(primitiveType, count, GL_UNSIGNED_INT, 0);
    else
        glDrawArrays(primitiveType, 0, count);
    glBindVertexArray(0);
}
void Mesh::destroy() {
    glDeleteBuffers(1, &vao);
    glDeleteBuffers(1, &vbo);
    glDeleteBuffers(1, &ebo);
}