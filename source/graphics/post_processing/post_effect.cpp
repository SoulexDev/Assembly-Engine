#include "../../../include/graphics/post_processing/post_effect.h"
#include "../../../include/core/engine.h"
#include "../../../include/graphics/models/fullscreen_quad_mesh.h"

#include <glad/glad.h>

PostEffect::PostEffect() {

}
PostEffect::PostEffect(Shader shader) {
	this->shader = shader;

	//renderTex = *new RenderTexture(Engine::screenWidth, Engine::screenHeight);

	/*this->shader.use();
	this->shader.set_texture("uScreenTexture", texture);*/
}
void PostEffect::render(unsigned int texture) {
	//renderTex.bind_framebuffer();

	glClearColor(1.0f, 1.0f, 1.0f, 1.0f);
	glClear(GL_COLOR_BUFFER_BIT);

	shader.use();
	glActiveTexture(GL_TEXTURE0);
	glBindTexture(GL_TEXTURE_2D, texture);
	shader.set_texture("uScreenTexture", 0);
	glBindVertexArray(FullscreenQuadMesh::mesh.vao);
	
	glDrawArrays(GL_TRIANGLES, 0, 6);

	//renderTex.unbind_framebuffer();
}
