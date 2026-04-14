#include "../../include/graphics/renderable.h"
#include "../../include/core/camera.h"
#include "../../include/core/component.h"
#include "../../include/core/console.h"
#include "../../include/graphics/render_pipeline.h"
#include "../../include/core/time.h"

#include <glm/glm.hpp>

Renderable::Renderable() {
	objectTransform = new Transform();
	RenderPipeline::renderables.push_back(*this);
}
Renderable::Renderable(Mesh mesh) {
	this->mesh = mesh;

	objectTransform = new Transform();
	RenderPipeline::renderables.push_back(*this);
}
Renderable::Renderable(Mesh mesh, Material material) {
	this->mesh = mesh;
	this->material = material;

	objectTransform = new Transform();
	RenderPipeline::renderables.push_back(*this);
}
void Renderable::draw(Camera camera) {
	if (mesh.count == 0)
		return;

	modelMatrix = glm::mat4_cast(objectTransform->get_rotation());
	glm::translate(modelMatrix, objectTransform->get_position());
	glm::scale(modelMatrix, objectTransform->get_scale());

	material.use();

	material.shader.set_matrix4("uModel", modelMatrix);
	material.shader.set_matrix4("uView", camera.viewMatrix);
	material.shader.set_matrix4("uProjection", camera.projectionMatrix);
	material.shader.set_matrix4("uLightSpace", RenderPipeline::sunCam.projectionMatrix * RenderPipeline::sunCam.viewMatrix);

	material.shader.set_vector("uLightPos", RenderPipeline::sunCam.transform.get_position());
	material.shader.set_vector("uViewPos", camera.transform.get_position());
	material.shader.set_float("uTime", Time::time);

	mesh.draw();
}
void Renderable::draw(Camera camera, Shader shader) {
	if (mesh.count == 0)
		return;

	modelMatrix = glm::mat4_cast(objectTransform->get_rotation());
	glm::translate(modelMatrix, objectTransform->get_position());
	glm::scale(modelMatrix, objectTransform->get_scale());

	shader.use();

	shader.set_matrix4("uModel", modelMatrix);
	shader.set_matrix4("uView", camera.viewMatrix);
	shader.set_matrix4("uProjection", camera.projectionMatrix);
	shader.set_matrix4("uLightSpace", RenderPipeline::sunCam.projectionMatrix * RenderPipeline::sunCam.viewMatrix);

	shader.set_vector("uLightPos", RenderPipeline::sunCam.transform.get_position());
	shader.set_vector("uViewPos", camera.transform.get_position());
	shader.set_float("uTime", Time::time);

	/*glUniformMatrix4fv(shader.uniformLocations.at("uModel"), 1, GL_FALSE, glm::value_ptr(modelMatrix));
	glUniformMatrix4fv(shader.uniformLocations.at("uView"), 1, GL_FALSE, glm::value_ptr(camera.viewMatrix));
	glUniformMatrix4fv(shader.uniformLocations.at("uProjection"), 1, GL_FALSE, glm::value_ptr(camera.projectionMatrix));
	glUniformMatrix4fv(shader.uniformLocations.at("uLightSpace"), 1, GL_FALSE,
		glm::value_ptr(RenderPipeline::sunCam.projectionMatrix * RenderPipeline::sunCam.viewMatrix));*/

	mesh.draw();
}
void Renderable::destroy() {
	mesh.destroy();
	material.destroy();
}