#include "../../include/world/world_manager.h"
#include "../../../include/core/resource_loader.h"
#include "../../include/world/world_properties.h"
#include "../../include/helpers/coordinate_helper.h"
#include "../../../include/graphics/material.h"
#include "../../include/data/block.h"

#include <glm/glm.hpp>

#include <map>
#include <iostream>
#include "../../include/world/world_noise_generator.h"
#include "../../../include/graphics/render_pipeline.h"

Material WorldManager::opaqueMat;
std::map<std::pair<int, int>, Chunk> WorldManager::chunks;

int WorldManager::checkCountFuckDebugging;

void WorldManager::generate() {
	Shader shader = *ResourceLoader<Shader>::load_resource("H:/GitRepos/FantasyEngine/resources/shaders/block.vert");
	Texture2D texAtlas = *ResourceLoader<Texture2D>::load_resource("H:/GitRepos/FantasyEngine/resources/textures/tex_atlas.png");

	opaqueMat = *new Material(shader);
	opaqueMat.texture2Ds.push_back(std::make_pair("tex_atlas", texAtlas));

	Block::register_blocks();
	WorldNoiseGenerator::init();

	int worldSize = 2;

	for (int z = -worldSize / 2; z < worldSize / 2; z++)
	{
		for (int x = -worldSize / 2; x < worldSize / 2; x++)
		{
			Chunk chunk = *new Chunk();
			chunk.generate_data(x * WorldProperties::chunkSize, z * WorldProperties::chunkSize);
			chunks.insert({ std::make_pair(x, z), chunk });
		}
	}
	for (int z = -worldSize / 2; z < worldSize / 2; z++)
	{
		for (int x = -worldSize / 2; x < worldSize / 2; x++)
		{
			std::pair<int, int> cPos = std::make_pair(x, z);

			if (chunks.find(cPos) == chunks.end())
				continue;

			chunks[cPos].generate_mesh(x * WorldProperties::chunkSize, z * WorldProperties::chunkSize);

			/*chunks[CoordinateHelper::coord_to_index(x, z, 16)].generate_mesh(
				x * WorldProperties::chunkSize, z * WorldProperties::chunkSize);*/
		}
	}
}
BlockType WorldManager::get_block(int x, int y, int z) {
	int cX = glm::floor((float)x / WorldProperties::chunkSize);
	int cY = glm::floor((float)y / WorldProperties::chunkSize);
	int cZ = glm::floor((float)z / WorldProperties::chunkSize);

	std::pair<int, int> cPos = std::make_pair(cX, cZ);

	if (chunks.find(cPos) == chunks.end() || cY < 0 || cY >= 4) {
		return BlockType_Air;
	}

	SubChunk sChunk = chunks[cPos].subChunks[cY];

	//std::cout << "World - " << "X: " << x << ", Y: " << y << ", Z: " << z << std::endl;
	//std::cout << "Block - " << "X: " << x - cX << ", Y: " << y - cY << ", Z: " << z - cZ << std::endl;

	return sChunk.blockStorage.get_block(
		x - cX * WorldProperties::chunkSize, 
		y - cY * WorldProperties::chunkSize, 
		z - cZ * WorldProperties::chunkSize);
}
Block WorldManager::get_block_data(glm::ivec3 position) {
	return get_block_data(position.x, position.y, position.z);
}
Block WorldManager::get_block_data(int x, int y, int z) {
	return Block::data[get_block(x, y, z)];
}
Block WorldManager::get_block_data(BlockType blockType) {
	return Block::data[blockType];
}