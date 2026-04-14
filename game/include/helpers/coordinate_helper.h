#pragma once
#include <glm/glm.hpp>

class CoordinateHelper {
public:
	static int coord_to_index(int x, int y, int z, int size);
	static int coord_to_index(int x, int y, int size);

	static glm::ivec3 index_to_coord3(int index, int size);
	static glm::ivec2 index_to_coord2(int index, int size);
};
