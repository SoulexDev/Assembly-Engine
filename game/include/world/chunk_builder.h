#pragma once
#include "../../../include/graphics/mesh.h"
#include "naive_block_storage.h"

#include <glm/glm.hpp>

class ChunkBuilder {
public:
	struct ChunkVertex {
		float x, y, z;
		float u, v;
	};

	const static std::vector<glm::ivec2> voxelUvs;
	const static std::vector<glm::ivec3> voxelVertices;
	const static std::vector<std::vector<int>> voxelTriangles;
	const static std::vector<glm::ivec3> faceIndices;
	const static std::vector<float> aoValues;
	const static std::vector<std::vector<glm::ivec3>> faceAos;
	const static std::vector<glm::ivec3> faceNormals;

	static Mesh build_naive(int x, int y, int z, NaiveBlockStorage blockStorage);
	static int get_ao_state(int side1, int side2, int corner);
	//Mesh build_binary_greedy();
};
