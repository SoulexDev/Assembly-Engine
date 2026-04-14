#pragma once
#include <SDL3/SDL.h>
#include "gui_window.h"
#include "../../include/core/console.h"

#include <vector>

class GUIManager {
public:
	std::vector<GUIWindow> guiWindows;
	static void init();
	static void update_platform();
	static void draw(int screenW, int screenH);
	static void render_data();
	static void destroy();
	static void bind_fbos();
	static void unbind_fbos();
	static void setup_imgui_style();
};