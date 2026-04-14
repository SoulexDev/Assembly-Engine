#include "../../include/graphics/material.h"
#include "../../include/core/camera.h"
#include "../../include/graphics/shader.h"
#include "../../include/graphics/textures/texture2d.h"
#include "../../include/core/console.h"
#include "../../include/graphics/render_pipeline.h"

#include <glm/glm.hpp>
#include <glad/glad.h>

void Material::use() {
	shader.use();

	//blending
	if (blendingEnabled) {
		if (!glIsEnabled(GL_BLEND))
			glEnable(GL_BLEND);

		glBlendFunc(srcBlendMode, dstBlendMode);
	}
	else if (glIsEnabled(GL_BLEND)) {
		glDisable(GL_BLEND);
	}

	//logic op
	if (logicOpEnabled) {
		if (!glIsEnabled(GL_LOGIC_OP))
			glEnable(GL_LOGIC_OP);

		glLogicOp(logicOp);
	}
	else if (glIsEnabled(GL_LOGIC_OP)) {
		glDisable(GL_LOGIC_OP);
	}

	//cull face
	if (cullFaceEnabled) {
		if (!glIsEnabled(GL_CULL_FACE))
			glEnable(GL_CULL_FACE);

		glCullFace(cullFaceMode);
	}
	else if (glIsEnabled(GL_CULL_FACE)) {
		glDisable(GL_CULL_FACE);
	}

	//depth test
	if (depthTestEnabled) {
		if (!glIsEnabled(GL_DEPTH_TEST))
			glEnable(GL_DEPTH_TEST);

		glDepthFunc(depthFunction);
	}
	else if (glIsEnabled(GL_DEPTH_TEST)) {
		glDisable(GL_DEPTH_TEST);
	}

	glActiveTexture(GL_TEXTURE0);
	glBindTexture(GL_TEXTURE_2D, RenderPipeline::shadowTex);
	shader.set_texture("shadow_map", 0);

	//TODO: optimize shader uniform calls
	if (texture2Ds.size() > 0) {
		for (int i = 0; i < texture2Ds.size(); ++i)
		{
			texture2Ds[i].second.bind(get_texture_unit(i + 1));
			shader.set_int(texture2Ds[i].first, i + 1);
		}
	}

	if (integers.size() > 0) {
		for (int i = 0; i < integers.size(); i++)
		{
			shader.set_int(integers[i].first, integers[i].second);
		}
	}

	if (floats.size() > 0) {
		for (int i = 0; i < floats.size(); i++)
		{
			shader.set_float(floats[i].first, floats[i].second);
		}
	}

	if (vec2s.size() > 0) {
		for (int i = 0; i < vec2s.size(); i++)
		{
			shader.set_vector(vec2s[i].first, vec2s[i].second);
		}
	}

	if (vec3s.size() > 0) {
		for (int i = 0; i < vec3s.size(); i++)
		{
			shader.set_vector(vec3s[i].first, vec3s[i].second);
		}
	}

	if (vec4s.size() > 0) {
		for (int i = 0; i < vec4s.size(); i++)
		{
			shader.set_vector(vec4s[i].first, vec4s[i].second);
		}
	}
}
void Material::destroy() {
	for (std::pair<std::string, Texture2D> pair : texture2Ds) {
		pair.second.destroy();
	}
}
GLenum Material::get_texture_unit(int index) {
	if (index >= 31) {
		Console::write_warning("Texture unit index is out of bounds.");
		return GL_TEXTURE31;
	}

	return GL_TEXTURE0 + index;
}