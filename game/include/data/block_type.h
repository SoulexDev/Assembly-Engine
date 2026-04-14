#pragma once
#include <cstdint>

enum BlockType : std::uint16_t {
	BlockType_Nothing,
	BlockType_Air,
	BlockType_Stone,
	BlockType_Grass,
	BlockType_Dirt
};