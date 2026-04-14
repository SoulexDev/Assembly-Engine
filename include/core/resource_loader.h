#include <string>

template<typename T>
class ResourceLoader {
public:
	static T fetch_include(std::string shaderRead);
	static T* load_resource(std::string filePath);
	static T* load_resource_multi_path(std::string pathA, std::string pathB);
	static T read_file(std::string filePath);
};