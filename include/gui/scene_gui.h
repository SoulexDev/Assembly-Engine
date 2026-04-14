#pragma once
#include "../../include/graphics/textures/render_texture.h"
#include "gui_window.h"

class SceneGUI : public GUIWindow {
public:
	void init() override;
	void render(int screenW, int screenH) override;
	void bind_fbo();
	void unbind_fbo();
	void destroy() override;
};