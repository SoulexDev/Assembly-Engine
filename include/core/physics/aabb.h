#pragma once
#include <glm/glm.hpp>

struct AABB {
public:
	float minX, minY, minZ;
	float maxX, maxY, maxZ;

	AABB();
	AABB(float minX, float minY, float minZ, float maxX, float maxY, float maxZ);
	AABB(glm::vec3 min, glm::vec3 max);

	/// <summary>
	/// Expands the AABB directionally
	/// </summary>
	/// <param name="x">X Axis growth</param>
	/// <param name="y">Y Axis growth</param>
	/// <param name="z">Z Axis growth</param>
	AABB expand(float x, float y, float z);

	/// <summary>
	/// Grows the AABB on all sides
	/// </summary>
	/// <param name="x">X Axis growth</param>
	/// <param name="y">Y Axis growth</param>
	/// <param name="z">Z Axis growth</param>
	AABB grow(float x, float y, float z);

	/// <summary>
	/// Encapsulates a point
	/// </summary>
	/// <param name="x">X point to encapsulate</param>
	/// <param name="y">Y point to encapsulate</param>
	/// <param name="z">Z point to encapsulate</param>
	void encapsulate(float x, float y, float z);

	//void set(float minX, float minY, float minZ, float maxX, float maxY, float maxZ);
	void move(float x, float y, float z);
	void move(glm::vec3 value);
	void set_center(glm::vec3 pos);
	glm::vec3 get_size();
	glm::vec3 get_center();

	float get_width();
	float get_height();
	float get_depth();

	float get_dist_x(AABB against, float deltaX);
	float get_dist_y(AABB against, float deltaY);
	float get_dist_z(AABB against, float deltaZ);

	bool intersects(AABB against);
	bool intersects_x(AABB against);
	bool intersects_y(AABB against);
	bool intersects_z(AABB against);

	AABB copy(AABB from);
};