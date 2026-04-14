#pragma once
#include "../mesh.h"
#include <vector>

class FullscreenQuadMesh {
private:
	const static std::vector<float> vertices;
public:
	static Mesh mesh;
	static void create();
};
