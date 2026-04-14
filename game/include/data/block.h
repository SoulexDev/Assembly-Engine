#pragma once
#include "block_type.h"

#include <glm/glm.hpp>

#include <vector>
#include <map>
#include <string>
#include <cstdint>

struct Block {
public:
	static std::map<BlockType, Block> data;

	BlockType blockType;
	std::string name;
	bool opaque;
	bool cullsSelf;
	float toughness;
	glm::ivec2 uvs[6];

	Block() {}

	Block(BlockType blockType);
	Block set_opaque();
	Block set_culls_self();
	Block set_toughness(float value);
	Block set_uv(uint16_t direction, int index);

	static void register_block(Block block);
	static void register_blocks();
};
