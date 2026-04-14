#pragma once
#include "../data/block_type.h"

struct BlockStorage {
public:
	virtual void set_block(int x, int y, int z, BlockType block) {};
	virtual BlockType get_block(int x, int y, int z) { return BlockType_Nothing; };
};