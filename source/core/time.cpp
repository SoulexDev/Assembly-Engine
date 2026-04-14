#include "../../include/core/time.h"
#include "../../include/core/main.h"

#include <SDL3/SDL.h>
#include <iostream>

float Time::lastFrameTime = 0.0f;
float Time::time = 0.0f;
float Time::deltaTime = 0.0f;
float Time::fixedDeltaTime = 0.0f;

void Time::update() {
	time = SDL_GetTicks() / 1000.0f;
	deltaTime = time - lastFrameTime;
	lastFrameTime = time;
}