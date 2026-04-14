#pragma once
#include <glm/glm.hpp>

class AtlasHelpers {
public:
	const static int textureSize{ 64 };
	const static int atlasTileSize{ 16 };

	static float get_uv_scale() {
		return (float)atlasTileSize / textureSize;
	}
	static int get_tile_count() {
		return textureSize / atlasTileSize;
	}
	static glm::ivec2 uv_from_index(int index) {
		int tileCount = get_tile_count();
		return glm::ivec2(index % tileCount, index / tileCount);
		//return glm::ivec2(0, 1);
	}
};