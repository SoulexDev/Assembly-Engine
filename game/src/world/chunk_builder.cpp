#include "../../include/world/chunk_builder.h"
#include "../../include/world/world_properties.h"
#include "../../include/world/world_manager.h"
#include "../../include/enums/direction.h"
#include "../../../include/graphics/vertex_attribute.h"
#include "../../include/data/block.h"
#include "../../include/data/block_type.h"
#include "../../include/world/naive_block_storage.h"
#include "../../include/helpers/atlas_helpers.h"

#include <glm/glm.hpp>
#include <vector>
#include <iostream>
#include <string>
#include <map>

const std::vector<glm::ivec2> ChunkBuilder::voxelUvs = {
	glm::ivec2(0, 0),
	glm::ivec2(1, 0),
	glm::ivec2(0, 1),
	glm::ivec2(1, 1)
};
const std::vector<glm::ivec3> ChunkBuilder::voxelVertices = {
	glm::ivec3(1, 1, 1),//0
	glm::ivec3(0, 1, 1),//1
	glm::ivec3(0, 0, 1),//2
	glm::ivec3(1, 0, 1),//3
	glm::ivec3(0, 1, 0),//4
	glm::ivec3(1, 1, 0),//5
	glm::ivec3(1, 0, 0),//6
	glm::ivec3(0, 0, 0),//7
};
const std::vector<std::vector<int>> ChunkBuilder::voxelTriangles = {
	std::vector<int> { 3, 2, 0, 1 },
	std::vector<int> { 6, 3, 5, 0 },
	std::vector<int> { 7, 6, 4, 5 },
	std::vector<int> { 2, 7, 1, 4 },
	std::vector<int> { 4, 5, 1, 0 },
	std::vector<int> { 2, 3, 7, 6 }
};
const std::vector<glm::ivec3> ChunkBuilder::faceIndices =
{
	glm::ivec3(7, 6, 5),
	glm::ivec3(5, 4, 3),
	glm::ivec3(1, 0, 7),
	glm::ivec3(3, 2, 1)
};
const std::vector<float> ChunkBuilder::aoValues =
{
	0.15f,
	0.3f,
	0.6f,
	1.0f
};
const std::vector<std::vector<glm::ivec3>> ChunkBuilder::faceAos =
{
	std::vector<glm::ivec3>
	{
		glm::ivec3(1, 1, 1),
		glm::ivec3(0, 1, 1),
		glm::ivec3(-1, 1, 1),
		glm::ivec3(-1, 0, 1),
		glm::ivec3(-1, -1, 1),
		glm::ivec3(0, -1, 1),
		glm::ivec3(1, -1, 1),
		glm::ivec3(1, 0, 1),
	},
	std::vector<glm::ivec3>
	{
		glm::ivec3(1, 1, -1),
		glm::ivec3(1, 1, 0),
		glm::ivec3(1, 1, 1),
		glm::ivec3(1, 0, 1),
		glm::ivec3(1, -1, 1),
		glm::ivec3(1, -1, 0),
		glm::ivec3(1, -1, -1),
		glm::ivec3(1, 0, -1),
	},
	std::vector<glm::ivec3>
	{
		glm::ivec3(-1, 1, -1),
		glm::ivec3(0, 1, -1),
		glm::ivec3(1, 1, -1),
		glm::ivec3(1, 0, -1),
		glm::ivec3(1, -1, -1),
		glm::ivec3(0, -1, -1),
		glm::ivec3(-1, -1, -1),
		glm::ivec3(-1, 0, -1),
	},
	std::vector<glm::ivec3>
	{
		glm::ivec3(-1, 1, 1),
		glm::ivec3(-1, 1, 0),
		glm::ivec3(-1, 1, -1),
		glm::ivec3(-1, 0, -1),
		glm::ivec3(-1, -1, -1),
		glm::ivec3(-1, -1, 0),
		glm::ivec3(-1, -1, 1),
		glm::ivec3(-1, 0, 1),
	},
	std::vector<glm::ivec3> 
	{
		glm::ivec3(-1, 1, 1),
		glm::ivec3(0, 1, 1),
		glm::ivec3(1, 1, 1),
		glm::ivec3(1, 1, 0),
		glm::ivec3(1, 1, -1),
		glm::ivec3(0, 1, -1),
		glm::ivec3(-1, 1, -1),
		glm::ivec3(-1, 1, 0),
	},
	std::vector<glm::ivec3>
	{
		glm::ivec3(-1, -1, -1),
		glm::ivec3(0, -1, -1),
		glm::ivec3(1, -1, -1),
		glm::ivec3(1, -1, 0),
		glm::ivec3(1, -1, 1),
		glm::ivec3(0, -1, 1),
		glm::ivec3(-1, -1, 1),
		glm::ivec3(-1, -1, 0),
	}
};
const std::vector<glm::ivec3> ChunkBuilder::faceNormals = {
	glm::ivec3(0.0f, 0.0f, 1.0f),
	glm::ivec3(1.0f, 0.0f, 0.0f),
	glm::ivec3(0.0f, 0.0f, -1.0f),
	glm::ivec3(-1.0f, 0.0f, 0.0f),
	glm::ivec3(0.0f, 1.0f, 0.0f),
	glm::ivec3(0.0f, -1.0f, 0.0f),
};
Mesh ChunkBuilder::build_naive(int x, int y, int z, NaiveBlockStorage blockStorage) {
	std::vector<float> vertices;
	std::vector<unsigned int> indices;

	uint16_t bitmask = 0;

	glm::ivec3 worldPos;
	glm::vec3 vertex;
	glm::ivec3 normal;
	glm::ivec3 aoIndices;

	int aos[4];

	int vertexIndex = 0;

	for (int zi = 0; zi < WorldProperties::chunkSize; zi++)
	{
		for (int yi = 0; yi < WorldProperties::chunkSize; yi++)
		{
			for (int xi = 0; xi < WorldProperties::chunkSize; xi++)
			{
				Block thisBlock = WorldManager::get_block_data(blockStorage.get_block(xi, yi, zi));

				if (thisBlock.blockType == BlockType_Air || thisBlock.blockType == BlockType_Nothing)
					continue;

				bitmask = 0;
				worldPos.x = xi + x;
				worldPos.y = yi + y;
				worldPos.z = zi + z;

				Block forward = WorldManager::get_block_data(worldPos.x, worldPos.y, worldPos.z + 1);
				Block right   = WorldManager::get_block_data(worldPos.x + 1, worldPos.y, worldPos.z);
				Block back    = WorldManager::get_block_data(worldPos.x, worldPos.y, worldPos.z - 1);
				Block left    = WorldManager::get_block_data(worldPos.x - 1, worldPos.y, worldPos.z);
				Block up      = WorldManager::get_block_data(worldPos.x, worldPos.y + 1, worldPos.z);
				Block down    = WorldManager::get_block_data(worldPos.x, worldPos.y - 1, worldPos.z);

				bitmask |= (forward.opaque || (forward.cullsSelf && forward.blockType == thisBlock.blockType) ? Direction_Forward : 0);
				bitmask |= (right.opaque   || (right.cullsSelf   && right.blockType   == thisBlock.blockType) ? Direction_Right   : 0);
				bitmask |= (back.opaque    || (back.cullsSelf    && back.blockType    == thisBlock.blockType) ? Direction_Back    : 0);
				bitmask |= (left.opaque    || (left.cullsSelf    && left.blockType    == thisBlock.blockType) ? Direction_Left    : 0);
				bitmask |= (up.opaque      || (up.cullsSelf      && up.blockType      == thisBlock.blockType) ? Direction_Up      : 0);
				bitmask |= (down.opaque    || (down.cullsSelf    && down.blockType    == thisBlock.blockType) ? Direction_Down    : 0);

				if ((bitmask & Direction_All) == Direction_All) {
					continue;
				}

				for (int dir = 0; dir < 6; dir++)
				{
					Direction checkDir = (Direction)(1 << dir);

					if ((bitmask & checkDir) == 0) {
						normal = faceNormals[dir];

						for (int i = 0; i < 4; i++)
						{
							aoIndices = faceIndices[i];
							Block side1 = WorldManager::get_block_data(faceAos[dir][aoIndices.x] + worldPos);
							Block side2 = WorldManager::get_block_data(faceAos[dir][aoIndices.z] + worldPos);
							Block corner = WorldManager::get_block_data(faceAos[dir][aoIndices.y] + worldPos);

							int aoState = get_ao_state(side1.opaque ? 1 : 0, side2.opaque ? 1 : 0, corner.opaque ? 1 : 0);

							aos[i] = aoState;
						}

						if (glm::min(aos[0], aos[3]) > glm::min(aos[1], aos[2]))
						{
							//flipped
							indices.push_back(vertexIndex + 2);
							indices.push_back(vertexIndex + 3);
							indices.push_back(vertexIndex);
							indices.push_back(vertexIndex + 3);
							indices.push_back(vertexIndex + 1);
							indices.push_back(vertexIndex);
						}
						else
						{
							//normal
							indices.push_back(vertexIndex + 3);
							indices.push_back(vertexIndex + 1);
							indices.push_back(vertexIndex + 2);
							indices.push_back(vertexIndex + 2);
							indices.push_back(vertexIndex + 1);
							indices.push_back(vertexIndex);
						}

						for (int i = 0; i < 4; i++)
						{
							vertex = voxelVertices[voxelTriangles[dir][i]];

							//position
							vertices.push_back(worldPos.x + vertex.x);
							vertices.push_back(worldPos.y + vertex.y);
							vertices.push_back(worldPos.z + vertex.z);

							//normal
							vertices.push_back(normal.x);
							vertices.push_back(normal.y);
							vertices.push_back(normal.z);
							
							//uv
							vertices.push_back((thisBlock.uvs[dir].x + voxelUvs[i].x) * AtlasHelpers::get_uv_scale());
							vertices.push_back((thisBlock.uvs[dir].y + voxelUvs[i].y) * AtlasHelpers::get_uv_scale());

							//vertex color
							vertices.push_back(aoValues[aos[i]]);
						}

						vertexIndex += 4;
					}
				}
			}
		}
	}
	return *new Mesh(vertices, indices, std::vector<VertexAttribute>{
		VertexAttribute::Vector3, 
		VertexAttribute::Vector3, 
		VertexAttribute::Vector2, 
		VertexAttribute::Float}, 
		GL_TRIANGLES, GL_STATIC_DRAW);
}
int ChunkBuilder::get_ao_state(int side1, int side2, int corner) {
	if (side1 + side2 == 2)
		return 0;

	return 3 - (side1 + side2 + corner);
}