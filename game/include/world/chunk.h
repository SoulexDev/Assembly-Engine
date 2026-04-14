#pragma once
#include "sub_chunk.h"

#include <vector>

struct Chunk {
public:
	std::vector<SubChunk> subChunks;

	/// <summary>
	/// Generates this chunk
	/// </summary>
	/// <param name="x">World space X position</param>
	/// <param name="z">World space Z position</param>
	void generate_data(int x, int z);
	void generate_mesh(int x, int z);
};
