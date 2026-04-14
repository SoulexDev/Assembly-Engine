#include "../../include/player/player_movement.h"
#include "../../../include/core/input.h"
#include "../../../include/core/time.h"
#include "../../../include/core/camera.h"

#include <glm/glm.hpp>
#include <iostream>

PlayerMovement::PlayerMovement() {
	transform = *new Transform();
	transform.set_position(glm::vec3(16.0f, 48.0f, 16.0f));

	mouseX = 0;
	mouseY = 0;
}
void PlayerMovement::update() {
	mouseX += Input::mouseDeltaX * 360.0f;
	mouseY -= Input::mouseDeltaY * 360.0f;

	mouseY = glm::clamp(mouseY, -89.0f, 89.0f);

	glm::quat rot = glm::angleAxis(glm::radians(mouseY), glm::vec3(1.0f, 0.0f, 0.0f));
	rot *= glm::angleAxis(glm::radians(-mouseX), glm::vec3(0.0f, 1.0f, 0.0f));

	transform.set_rotation(rot);

	glm::vec3 moveVector = glm::normalize(
		Input::horizontal * transform.get_right() +
		Input::vertical * transform.get_forward() +
		Input::longitudinal * transform.get_up());

	if (glm::isnan(moveVector.x))
		moveVector.x = 0.0f;
	if (glm::isnan(moveVector.y))
		moveVector.y = 0.0f;
	if (glm::isnan(moveVector.z))
		moveVector.z = 0.0f;

	moveVector *= Input::is_key_pressed(SDL_SCANCODE_LSHIFT) ? 8.0f : 4.0f;

	glm::vec3 pos = transform.get_position() + moveVector * Time::deltaTime;

	transform.set_position(pos);

	Camera::main.transform.set_position(pos);
	Camera::main.transform.set_rotation(rot);
}