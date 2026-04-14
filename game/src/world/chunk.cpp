#include "../../include/world/chunk.h"
#include "../../include/world/world_properties.h"

void Chunk::generate_data(int x, int z) {
	for (int i = 0; i < 4; i++)
	{
		SubChunk sChunk = *new SubChunk();
		sChunk.generate_data(x, i * WorldProperties::chunkSize, z);
		subChunks.push_back(sChunk);
	}
	/*SubChunk sChunk = *new SubChunk();
	sChunk.generate_data(x, 0, z);
	subChunks.push_back(sChunk);*/
}
void Chunk::generate_mesh(int x, int z) {
	for (int i = 0; i < 4; i++)
	{
		subChunks[i].generate_mesh(x, i * WorldProperties::chunkSize, z);
	}
	//subChunks[0].generate_mesh(x, 0, z);
}