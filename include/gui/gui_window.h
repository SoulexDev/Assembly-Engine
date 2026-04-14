#pragma once

class GUIWindow {
public:
	virtual void init();
	virtual void render(int screenW, int screenH);
	virtual void destroy();
};