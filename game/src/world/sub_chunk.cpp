#include "../../include/world/sub_chunk.h"
#include "../../include/world/world_properties.h"
#include "../../include/world/chunk_builder.h"
#include "../../include/world/world_manager.h"
#include "../../include/world/world_noise_generator.h"

void SubChunk::generate_data(int x, int y, int z) {
	for (int zi = 0; zi < WorldProperties::chunkSize; zi++)
	{
		for (int yi = 0; yi < WorldProperties::chunkSize; yi++)
		{
			for (int xi = 0; xi < WorldProperties::chunkSize; xi++)
			{
				blockStorage.set_block(xi, yi, zi, WorldNoiseGenerator::get_block(x + xi, y + yi, z + zi));
			}
		}
	}
}
void SubChunk::generate_mesh(int x, int y, int z) {
	renderable = new Renderable(ChunkBuilder::build_naive(x, y, z, blockStorage), WorldManager::opaqueMat);
}