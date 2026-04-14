#include "../../include/data/block.h"
#include "../../include/data/block_type.h"
#include "../../include/helpers/atlas_helpers.h"

#include <map>
#include <glm/glm.hpp>
#include "../../include/enums/direction.h"

std::map<BlockType, Block> Block::data;

Block::Block(BlockType blockType) {
	this->blockType = blockType;
	this->opaque = false;
	this->cullsSelf = false;
}
Block Block::set_opaque() {
	opaque = true;
	return *this;
}
Block Block::set_culls_self(){
	cullsSelf = true;
	return *this;
}
Block Block::set_toughness(float value){
	toughness = value;
	return *this;
}
Block Block::set_uv(uint16_t dir, int index) {
	glm::ivec2 uv = AtlasHelpers::uv_from_index(index);
	if ((dir & Direction_All) == Direction_All) {
		uvs[0] = uv;
		uvs[1] = uv;
		uvs[2] = uv;
		uvs[3] = uv;
		uvs[4] = uv;
		uvs[5] = uv;
		return *this;
	}
	if ((dir & Direction_Forward) == Direction_Forward) {
		uvs[0] = uv;
	}
	if ((dir & Direction_Right) == Direction_Right) {
		uvs[1] = uv;
	}
	if ((dir & Direction_Back) == Direction_Back) {
		uvs[2] = uv;
	}
	if ((dir & Direction_Left) == Direction_Left) {
		uvs[3] = uv;
	}
	if ((dir & Direction_Up) == Direction_Up) {
		uvs[4] = uv;
	}
	if ((dir & Direction_Down) == Direction_Down) {
		uvs[5] = uv;
	}
	return *this;
}
void Block::register_block(Block block) {
	data.insert({ block.blockType, block });
}
void Block::register_blocks() {
	register_block((*new Block(BlockType_Nothing)).set_opaque());
	register_block((*new Block(BlockType_Air)));
	register_block((*new Block(BlockType_Stone)).set_opaque().set_culls_self().set_uv(Direction_All, 3));
	register_block((*new Block(BlockType_Dirt)).set_opaque().set_culls_self().set_uv(Direction_All, 1));

	register_block((*new Block(BlockType_Grass)).set_opaque().set_culls_self()
		.set_uv(Direction_XZAxis, 0)
		.set_uv(Direction_Down, 1)
		.set_uv(Direction_Up, 2));
}