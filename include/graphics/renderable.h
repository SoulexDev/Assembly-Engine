#pragma once
#include "../core/camera.h"
#include "shader.h"
#include "../core/component.h"
#include "material.h"
#include "mesh.h"

#include <glm/glm.hpp>

class Renderable : public Component {
public:
	Mesh mesh;
	Material material;
	bool useShadow = true;
	glm::mat4 modelMatrix = glm::mat4(1.0);

	Renderable();
	Renderable(Mesh mesh);
	Renderable(Mesh mesh, Material material);

	void draw(Camera camera);
	void draw(Camera camera, Shader shader);

	void destroy();
};