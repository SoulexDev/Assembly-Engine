#pragma once
#include "transform.h"

#include <glm/glm.hpp>

enum CameraProjectionType_ {
	CameraProjectionType_Perspective,
	CameraProjectionType_Orthographic
};

class Camera {
public:
	static Camera main;

	Transform transform;

	CameraProjectionType_ projectionType;
	glm::mat4 viewMatrix;
	glm::mat4 projectionMatrix;

	float fov;
	float orthoWidth;
	float orthoHeight;
	float cameraNear;
	float cameraFar;

	void init();
	void set_matrices();
};