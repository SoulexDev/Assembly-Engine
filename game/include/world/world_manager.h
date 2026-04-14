#pragma once
#include "../../../include/graphics/material.h"
#include "../data/block_type.h"
#include "../data/block.h"
#include "chunk.h"

#include <glm/glm.hpp>
#include <map>

class WorldManager {
public:
	static Material opaqueMat;
	static std::map<std::pair<int, int>, Chunk> chunks;

	static int checkCountFuckDebugging;

	static void generate();
	static BlockType get_block(int x, int y, int z);
	static Block get_block_data(int x, int y, int z);
	static Block get_block_data(glm::ivec3 position);
	static Block get_block_data(BlockType blockType);
};