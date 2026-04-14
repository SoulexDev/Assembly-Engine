#pragma once
#include <vector>
#include "engine_object.h"

//#include <SDL3/SDL.h>

class Engine {
public:
	static int screenWidth;
	static int screenHeight;

	static std::vector<EngineObject> objects;
	//static SDL_Window* window;
	//static SDL_GLContext glContext;
};