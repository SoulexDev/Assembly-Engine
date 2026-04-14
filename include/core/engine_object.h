#pragma once
#include <vector>
#include <string>

#include "engine_object.fwd.h"
#include "transform.fwd.h"
#include "component.h"

class EngineObject {
public:
	std::string name;
	Transform transform;
	std::vector<Component> components;

	EngineObject();
	EngineObject(const char* name);
	//template<typename T>
	void attach(Component* component);

	/*template<typename T>
	void detach(T component);*/
};