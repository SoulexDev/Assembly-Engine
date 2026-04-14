#pragma once
#include "block_storage.h"
#include "../data/block.h"

#include <vector>

struct NaiveBlockStorage : public BlockStorage {
public:
	std::vector<BlockType> blocks;

	NaiveBlockStorage();

	void set_block(int x, int y, int z, BlockType block) override;
	BlockType get_block(int x, int y, int z) override;
};