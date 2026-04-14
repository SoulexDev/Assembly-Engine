#pragma once
#include "gui_window.h"
#include "../../include/core/engine_object.h"
#include "../../lib/imgui/imgui.h"

#include <vector>

class HierarchyGUI : public GUIWindow {
public:
	void render(int screenW, int screenH) override;
	//void draw_nodes(EngineObject& obj, std::vector<Transform> children);
	ImGuiInputTextCallback search_callback();
};