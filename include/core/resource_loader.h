#include <string>

template<class T>
class ResourceLoader {
private:
	static std::string fetch_include(std::string shaderRead);
public:
	static T* load_resource(std::string filePath);
	static T read_file(std::string filePath);

	static T* load_resource_multi_path(std::string pathA, std::string pathB);
};