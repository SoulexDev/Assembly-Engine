#include "../../include/world/naive_block_storage.h"
#include "../../include/helpers/coordinate_helper.h"
#include "../../include/world/world_properties.h"

NaiveBlockStorage::NaiveBlockStorage() {
	blocks.resize(WorldProperties::chunk_size_cubed());
	//blocks = new Block[WorldProperties::chunkSize * WorldProperties::chunkSize * WorldProperties::chunkSize];
}
void NaiveBlockStorage::set_block(int x, int y, int z, BlockType block) {
	blocks[CoordinateHelper::coord_to_index(x, y, z, WorldProperties::chunkSize)] = block;
}
BlockType NaiveBlockStorage::get_block(int x, int y, int z) {
	if (x < 0 || y < 0 || z < 0 || 
		x >= WorldProperties::chunkSize || y >= WorldProperties::chunkSize || z >= WorldProperties::chunkSize)
		return BlockType_Air;

	return blocks[CoordinateHelper::coord_to_index(x, y, z, WorldProperties::chunkSize)];
}