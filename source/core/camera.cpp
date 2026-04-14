#include "../../include/core/camera.h"
#include "../../include/core/time.h"
#include "../../include/core/input.h"
#include "../../include/core/console.h"
#include "../../include/core/transform.h"

#include <glm/glm.hpp>

#include <iostream>
#include <string>

Camera Camera::main;

void Camera::init() {
	transform = *new Transform();
	//transform.set_position(glm::vec3(4.0, 64.0, 4.0));

	fov = 90.0f;
	orthoWidth = 4.0f;
	orthoHeight = 4.0f;
	cameraNear = 0.1f;
	cameraFar = 1000.0f;

	projectionType = CameraProjectionType_Perspective;
}
//void Camera::update() {
//	
//
//	/*string msg = "X: " + std::to_string(pos.x) + ", Y: " + std::to_string(pos.y);
//	Console::write(msg);*/
//}
void Camera::set_matrices() {
	viewMatrix = glm::lookAt(
		transform.get_position(),
		transform.get_position() + transform.get_forward(),
		glm::vec3(0.0, 1.0, 0.0));

	switch (projectionType)
	{
	case CameraProjectionType_Perspective:
		projectionMatrix = glm::perspective(glm::radians(fov), 16.0f / 9.0f, cameraNear, cameraFar);
		break;
	case CameraProjectionType_Orthographic:
		projectionMatrix = glm::ortho(
			-orthoWidth * 0.5f, orthoWidth * 0.5f,
			-orthoHeight * 0.5f, orthoHeight * 0.5f,
			cameraNear, cameraFar);
		break;
	default:
		break;
	}
}