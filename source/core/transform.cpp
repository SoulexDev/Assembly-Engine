#include "../../include/core/transform.h"

#include <glm/glm.hpp>

glm::vec3 Transform::get_position() {
	if (parent == NULL) {
		return localPosition;
	}

	return parent->get_position() + localPosition;
}
void Transform::set_position(glm::vec3 position) {
	if (parent == NULL) {
		localPosition = position;
		return;
	}

	localPosition = position - parent->get_position();
}
glm::quat Transform::get_rotation() {
	if (parent == NULL) {
		return localRotation;
	}

	return parent->get_rotation() + localRotation;
}
void Transform::set_rotation(glm::quat rotation) {
	if (parent == NULL) {
		localRotation = rotation;
		return;
	}

	localRotation = rotation * glm::inverse(parent->get_rotation());
}
glm::vec3 Transform::get_scale() {
	if (parent == NULL) {
		return localScale;
	}

	return parent->get_scale() * localScale;
}
void Transform::set_scale(glm::vec3 scale) {
	if (parent == NULL) {
		localScale = scale;
		return;
	}

	if (parent->get_scale().length == 0)
		return;

	localScale = scale / parent->get_scale();
}
glm::vec3 Transform::get_right() {
	return glm::vec3(1.0f, 0.0f, 0.0f) * get_rotation();
}
glm::vec3 Transform::get_up() {
	return glm::vec3(0.0f, 1.0f, 0.0f) * get_rotation();
}
glm::vec3 Transform::get_forward() {
	return glm::vec3(0.0f, 0.0f, -1.0f) * get_rotation();
}
void Transform::set_parent(Transform* transform, bool keepPosition) {
	parent = transform;

	if (parent != NULL) {
		parent->children.push_back(*this);
	}
}
bool Transform::has_children() {
	return children.size() > 0;
}
bool Transform::has_parent() {
	return parent != NULL;
}
int Transform::child_count() {
	return children.size();
}