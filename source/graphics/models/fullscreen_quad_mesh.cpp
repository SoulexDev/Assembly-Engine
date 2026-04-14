#include "../../../include/graphics/models/fullscreen_quad_mesh.h"
#include "../../../include/graphics/mesh.h"
#include "../../../include/graphics/vertex_attribute.h"

#include <vector>

const std::vector<float> FullscreenQuadMesh::vertices = {
    -1.0f,  1.0f,  0.0f, 1.0f,
    -1.0f, -1.0f,  0.0f, 0.0f,
     1.0f, -1.0f,  1.0f, 0.0f,

    -1.0f,  1.0f,  0.0f, 1.0f,
     1.0f, -1.0f,  1.0f, 0.0f,
     1.0f,  1.0f,  1.0f, 1.0f
};

Mesh FullscreenQuadMesh::mesh;

void FullscreenQuadMesh::create() {
    mesh = *new Mesh(vertices, std::vector<VertexAttribute>{VertexAttribute::Vector2, VertexAttribute::Vector2});
}