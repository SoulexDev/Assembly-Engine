#pragma once
class WorldProperties {
public:
	const static int chunkSize{ 16 };
	static int chunk_size_cubed() {
		return chunkSize * chunkSize * chunkSize;
	}
};