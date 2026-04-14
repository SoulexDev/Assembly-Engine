#include "../../include/core/input.h"
#include "../../include/core/console.h"

#include <string>
#include <iostream>

#include <SDL3/SDL.h>

const bool* Input::currentKeyStates;
const bool* Input::lastKeyStates;
float Input::lastMousePosX;
float Input::lastMousePosY;

float Input::horizontal;
float Input::vertical;
float Input::longitudinal;
float Input::mouseDeltaX;
float Input::mouseDeltaY;

void Input::init() {
	//float mouseX;
	//float mouseY;

	//SDL_GetMouseState(&mouseX, &mouseY);

	lastMousePosX = 1920.0f * 0.5f;
	lastMousePosY = 1080.0f * 0.5f;

	currentKeyStates = SDL_GetKeyboardState(nullptr);
	lastKeyStates = SDL_GetKeyboardState(nullptr);
}
void Input::update() {
	float hl = is_key_pressed(SDL_SCANCODE_A) ? -1.0f : 0.0f;
	float hr = is_key_pressed(SDL_SCANCODE_D) ? 1.0f : 0.0f;
	horizontal = hl + hr;

	float vf = is_key_pressed(SDL_SCANCODE_W) ? 1.0f : 0.0f;
	float vb = is_key_pressed(SDL_SCANCODE_S) ? -1.0f : 0.0f;
	vertical = vf + vb;

	float lu = is_key_pressed(SDL_SCANCODE_E) ? 1.0f : 0.0f;
	float ld = is_key_pressed(SDL_SCANCODE_Q) ? -1.0f : 0.0f;
	longitudinal = lu + ld;

	float mouseX;
	float mouseY;

	SDL_MouseButtonFlags buttonFlags = SDL_GetMouseState(&mouseX, &mouseY);

	mouseDeltaX = (lastMousePosX - mouseX) / 1920.0f;
	mouseDeltaY = (lastMousePosY - mouseY) / 1080.0f;

	lastMousePosX = mouseX;
	lastMousePosY = mouseY;
}
void Input::set_current_state() {
	currentKeyStates = SDL_GetKeyboardState(nullptr);
}
void Input::set_last_state() {
	lastKeyStates = currentKeyStates;
}
void Input::on_event(SDL_Event* event) {
	/*if (event.type == SDL_EVENT_MOUSE_MOTION) {
		mouseDeltaX = event.motion.x - lastMousePosX;
		mouseDeltaY = event.motion.y - lastMousePosY;

		lastMousePosX = event.motion.x;
		lastMousePosY = event.motion.y;

		std::string msg = "dX: " + std::to_string(mouseDeltaX) + ", dY: " + std::to_string(mouseDeltaY);
		Console::write(msg);
	}*/
}
bool Input::get_key_down(SDL_Scancode scanCode) {
	std::cout << currentKeyStates[scanCode] << std::endl;
	std::cout << lastKeyStates[scanCode] << std::endl;

	return currentKeyStates[scanCode] && !lastKeyStates[scanCode];
}
bool Input::get_key_up(SDL_Scancode scanCode) {
	return !currentKeyStates[scanCode] && lastKeyStates[scanCode];
}
bool Input::is_key_pressed(SDL_Scancode scanCode) {
	return currentKeyStates[scanCode];
}