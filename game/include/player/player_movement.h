#pragma once
#include "../../../include/core/transform.h"

class PlayerMovement {
private:
	float mouseX;
	float mouseY;
public:
	Transform transform;

	PlayerMovement();
	void update();
};