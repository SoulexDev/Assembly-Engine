#pragma once
#include <SDL3/SDL.h>

class Input {
protected:
	const static bool* currentKeyStates;
	const static bool* lastKeyStates;
	static float lastMousePosX;
	static float lastMousePosY;
public:
	static float horizontal;
	static float vertical;
	static float longitudinal;
	static float mouseDeltaX;
	static float mouseDeltaY;

	static void init();
	static void update();
	static void set_current_state();
	static void set_last_state();
	static void on_event(SDL_Event* event);
	static bool get_key_down(SDL_Scancode scanCode);
	static bool get_key_up(SDL_Scancode scanCode);
	static bool is_key_pressed(SDL_Scancode scanCode);
};