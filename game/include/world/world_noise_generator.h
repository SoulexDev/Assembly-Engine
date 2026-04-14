#pragma once
#include "../data/block_type.h"
#include "../../include/world/FastNoiseLite.h"

class WorldNoiseGenerator {
private:
	static FastNoiseLite noise;
public:
	static void init();
	static BlockType get_block(float x, int y, float z);
};
