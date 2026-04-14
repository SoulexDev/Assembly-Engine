#include "../../include/core/engine_object.h"

EngineObject::EngineObject() {
	transform = *new Transform();
	name = "New Object";
}
EngineObject::EngineObject(const char* name) {
	transform = *new Transform();
	transform.engineObject = this;
	this->name = name;
}
//template<typename Component>
void EngineObject::attach(Component* component) {
	component->objectTransform = &transform;
	components.push_back(*component);
}
/*template<typename Component>
void EngineObject::detach(Component component) {
	components.erase(component);
}*/