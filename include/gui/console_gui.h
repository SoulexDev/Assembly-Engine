#pragma once
#include "../../include/gui/gui_window.h"
#include "../../include/core/console.h"

class ConsoleGUI : public GUIWindow {
public:
	void init() override;
	void render(int screenW, int screenH) override;
};