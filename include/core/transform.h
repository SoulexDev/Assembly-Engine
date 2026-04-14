#pragma once
#include "transform.fwd.h"
#include "engine_object.fwd.h"

#include <glm/glm.hpp>
#include <glm/gtc/matrix_transform.hpp>
#include <glm/gtc/type_ptr.hpp>

#include <vector>

/// <summary>
/// Fantasy engine Transform class
/// </summary>
class Transform {
private:
	/*glm::vec3 position = glm::vec3(0.0, 0.0, 0.0);
	glm::quat rotation = glm::quatLookAt(glm::vec3(0.0f, 0.0f, 1.0f), glm::vec3(0.0f, 1.0f, 0.0f));
	glm::vec3 scale = glm::vec3(1.0, 1.0, 1.0);*/

	glm::vec3 localPosition = glm::vec3(0.0, 0.0, 0.0);
	glm::quat localRotation = glm::quatLookAt(glm::vec3(0.0f, 0.0f, 1.0f), glm::vec3(0.0f, 1.0f, 0.0f));
	glm::vec3 localScale = glm::vec3(1.0, 1.0, 1.0);
public:
	EngineObject* engineObject;

	Transform* parent = nullptr;
	std::vector<Transform> children;

	/// <summary>
	/// Gets the transform position
	/// </summary>
	/// <returns>The world space position of this transform</returns>
	glm::vec3 get_position();

	/// <summary>
	/// Sets the transform position
	/// </summary>
	/// <returns></returns>
	void set_position(glm::vec3 position);

	/// <summary>
	/// Gets the transform rotation
	/// </summary>
	/// <returns>The world space rotation of this transform</returns>
	glm::quat get_rotation();

	/// <summary>
	/// Sets the transform rotation
	/// </summary>
	/// <returns></returns>
	void set_rotation(glm::quat rotation);

	/// <summary>
	/// Gets the transform scale
	/// </summary>
	/// <returns>The world space scale of this transform</returns>
	glm::vec3 get_scale();

	/// <summary>
	/// Sets the transform scale
	/// </summary>
	/// <returns></returns>
	void set_scale(glm::vec3 scale);

	glm::vec3 get_right();
	glm::vec3 get_up();
	glm::vec3 get_forward();

	void set_parent(Transform* transform, bool keepPosition = true);
	bool has_children();
	bool has_parent();
	int child_count();

	~Transform() {
		for (Transform child : children) {
			child.parent = nullptr;
		}
	}
};