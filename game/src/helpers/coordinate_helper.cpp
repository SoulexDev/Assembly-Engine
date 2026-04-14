#include "../../include/helpers/coordinate_helper.h"

int CoordinateHelper::coord_to_index(int x, int y, int z, int size) {
	return x + y * size + z * size * size;
}
int CoordinateHelper::coord_to_index(int x, int y, int size) {
	return x + y * size;
}
glm::ivec3 CoordinateHelper::index_to_coord3(int index, int size) {
	return glm::ivec3(index % size, index / size % size, index / (size * size));
}
glm::ivec2 CoordinateHelper::index_to_coord2(int index, int size) {
	return glm::ivec2(index % size, index / size);
}