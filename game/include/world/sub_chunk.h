#pragma once
#include "../../../include/graphics/renderable.h"
#include "naive_block_storage.h"

struct SubChunk {
public:
	NaiveBlockStorage blockStorage;
	Renderable* renderable;

	/// <summary>
	/// Generate the chunk data
	/// </summary>
	/// <param name="x">Chunk X position in world space</param>
	/// <param name="y">Chunk Y position in world space</param>
	/// <param name="z">Chunk Z position in world space</param>
	void generate_data(int x, int y, int z);

	/// <summary>
	/// Generate the chunk mesh
	/// </summary>
	/// <param name="x">Chunk X position in world space</param>
	/// <param name="y">Chunk Y position in world space</param>
	/// <param name="z">Chunk Z position in world space</param>
	void generate_mesh(int x, int y, int z);
};
