#include "../../include/world/world_noise_generator.h"
#include "../../include/world/FastNoiseLite.h"
#include "../../include/data/block_type.h"

#include <glm/glm.hpp>

FastNoiseLite WorldNoiseGenerator::noise;

void WorldNoiseGenerator::init() {
	noise.SetNoiseType(FastNoiseLite::NoiseType_Perlin);
	noise.SetFractalOctaves(4);
	noise.SetFrequency(0.1f);
	noise.SetFractalLacunarity(1.7f);
}
BlockType WorldNoiseGenerator::get_block(float x, int y, float z) {
	int height = (noise.GetNoise(x, z) * 0.5f + 0.5f) * 12;
	height += 32;

	if (y > height) {
		return BlockType_Air;
	}
	else if (y == height) {
		return BlockType_Grass;
	}
	else if (y > height - 3) {
		return BlockType_Dirt;
	}
	else
		return BlockType_Stone;
}
