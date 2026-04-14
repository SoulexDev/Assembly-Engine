#include "../../../include/core/physics/aabb.h"

AABB::AABB() {
	minX = 0;
	minY = 0;
	minZ = 0;
	maxX = 0;
	maxY = 0;
	maxZ = 0;
}
AABB::AABB(float minX, float minY, float minZ, float maxX, float maxY, float maxZ) {
	this->minX = minX;
	this->minY = minY;
	this->minZ = minZ;
	this->maxX = maxX;
	this->maxY = maxY;
	this->maxZ = maxZ;
}
AABB::AABB(glm::vec3 min, glm::vec3 max) {
	this->minX = min.x;
	this->minY = min.y;
	this->minZ = min.z;
	this->maxX = max.x;
	this->maxY = max.y;
	this->maxZ = max.z;
}
AABB AABB::expand(float x, float y, float z) {
	AABB returnValue = copy(*this);

	if (x > 0)
		returnValue.maxX += x;
	else
		returnValue.minX += x;

	if (y > 0)
		returnValue.maxY += y;
	else
		returnValue.minY += y;

	if (z > 0)
		returnValue.maxZ += z;
	else
		returnValue.minZ += z;

	return returnValue;
}
AABB AABB::grow(float x, float y, float z) {
	AABB returnValue = copy(*this);

	returnValue.minX -= x;
	returnValue.maxX += x;
	returnValue.minY -= y;
	returnValue.maxY += y;
	returnValue.minZ -= z;
	returnValue.maxZ += z;

	return returnValue;
}
void AABB::encapsulate(float x, float y, float z) {
	if (x < minX)
		minX = x;
	else if (x > maxX)
		maxX = x;

	if (y < minY)
		minY = y;
	else if (y > maxY)
		maxY = y;

	if (z < minZ)
		minZ = z;
	else if (z > maxZ)
		maxZ = z;
}
void AABB::move(float x, float y, float z) {
	minX += x;
	minY += y;
	minZ += z;
	maxX += x;
	maxY += y;
	maxZ += z;
}
void AABB::move(glm::vec3 value) {
	minX += value.x;
	minY += value.y;
	minZ += value.z;
	maxX += value.x;
	maxY += value.y;
	maxZ += value.z;
}
void AABB::set_center(glm::vec3 pos) {
	float width = get_width();
	minX = pos.x - width * 0.5f;
	maxX = pos.x + width * 0.5f;

	float height = get_height();
	minY = pos.y - height * 0.5f;
	maxY = pos.y + height * 0.5f;

	float depth = get_depth();
	minZ = pos.z - depth * 0.5f;
	maxZ = pos.z + depth * 0.5f;
}
glm::vec3 AABB::get_size() {
	return glm::vec3(get_width(), get_height(), get_depth());
}
glm::vec3 AABB::get_center() {
	return glm::vec3(minX + maxX, minY + maxY, minZ + maxZ) * 0.5f;
}
float AABB::get_width() {
	return maxX - minX;
}
float AABB::get_height() {
	return maxY - minY;
}
float AABB::get_depth() {
	return maxZ - minZ;
}
float AABB::get_dist_x(AABB against, float deltaX) {
	if (intersects_y(against) && intersects_z(against)) {
		if (deltaX > 0 && maxX <= against.minX) {
			float dist = against.minX - maxX;
			if (deltaX > dist)
				deltaX = dist;
		}
		if (deltaX < 0 && minX <= against.maxX) {
			float dist = against.maxX - minX;
			if (deltaX < dist)
				deltaX = dist;
		}
	}
	return deltaX;
}
float AABB::get_dist_y(AABB against, float deltaY) {
	if (intersects_x(against) && intersects_z(against)) {
		if (deltaY > 0 && maxY <= against.minY) {
			float dist = against.minY - maxY;
			if (deltaY > dist)
				deltaY = dist;
		}
		if (deltaY < 0 && minY <= against.maxY) {
			float dist = against.maxY - minY;
			if (deltaY < dist)
				deltaY = dist;
		}
	}
	return deltaY;
}
float AABB::get_dist_z(AABB against, float deltaZ) {
	if (intersects_x(against) && intersects_y(against)) {
		if (deltaZ > 0 && maxZ <= against.minZ) {
			float dist = against.minZ - maxZ;
			if (deltaZ > dist)
				deltaZ = dist;
		}
		if (deltaZ < 0 && minZ <= against.maxZ) {
			float dist = against.maxZ - minZ;
			if (deltaZ < dist)
				deltaZ = dist;
		}
	}
	return deltaZ;
}
bool AABB::intersects(AABB against) {
	return intersects_x(against) && intersects_y(against) && intersects_z(against);
}
bool AABB::intersects_x(AABB against) {
	return minX < against.maxX && maxX > against.minX;
}
bool AABB::intersects_y(AABB against) {
	return minY < against.maxY && maxY > against.minY;
}
bool AABB::intersects_z(AABB against) {
	return minZ < against.maxZ && maxZ > against.minZ;
}
AABB AABB::copy(AABB from) {
	return *new AABB(from.minX, from.minY, from.minZ, from.maxX, from.maxY, from.maxZ);
}