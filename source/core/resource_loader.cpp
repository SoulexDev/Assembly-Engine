#include "../../include/core/resource_loader.h"
#include "../../include/graphics/textures/texture2d.h"
#include "../../include/graphics/shader.h"
#include "../../include/graphics/mesh.h"

#include <iostream>
#include <fstream>
#include <sstream>
#include <string>
#include <regex>
#include <filesystem>
#include <vector>

const std::vector<std::string> includePaths = {
	"./internal",
	"./includes"
};

//Modified version of the original code by Wyck https://stackoverflow.com/a/78886109
std::string ResourceLoader<std::string>::fetch_include(std::string shaderRead) {
	std::regex includeRegex(R"(#include\s*["<](.*?)[">])");
	std::smatch match;
	std::string output = shaderRead;
	std::string::const_iterator includeSearch(output.cbegin());

	while (std::regex_search(includeSearch, output.cend(), match, includeRegex)) {
		std::string includeDir = match[1].str();
		std::string content;
		std::string filePath;
		if (std::filesystem::exists(includeDir)) {
			filePath = includeDir;
		}
		else {
			for (const auto& dir : includePaths) {
				std::filesystem::path path = std::filesystem::path(dir) / includeDir;
				if (std::filesystem::exists(path)) {
					filePath = includeDir;
					break;
				}
			}
		}
		if (!filePath.empty()) {
			content = read_file(filePath);
		}
		else {
			std::cerr << "Error: File not found - " << includeDir << std::endl;
		}

		auto matchPos = match.position(0) + (includeSearch - output.cbegin());
		output.replace(matchPos, match.length(0), content);

		includeSearch = output.cbegin() + matchPos;
	}

	return output;
}
Texture2D* ResourceLoader<Texture2D>::load_resource(std::string filePath) {
	return new Texture2D(filePath);
}
Shader* ResourceLoader<Shader>::load_resource(std::string filePath) {
	std::string vertPath = filePath;
	std::string fragPath = vertPath.substr(0, vertPath.length() - 5) + ".frag";

	std::string vertRead = ResourceLoader<std::string>::read_file(vertPath);
	vertRead = fetch_include(vertRead);
	if (vertRead.empty()) {
		std::cout << "File is empty: " << vertRead << std::endl;
		return nullptr;
	}
	std::cout << vertRead << std::endl;

	std::string fragRead = ResourceLoader<std::string>::read_file(fragPath);
	fragRead = fetch_include(fragRead);
	if (fragRead.empty()) {
		std::cout << "File is empty: " << fragRead << std::endl;
		return nullptr;
	}
	std::cout << fragRead << std::endl;

	/*string geoRead = "";
	if (geoFilePath) {
		geoRead = read_shader_file(geoFilePath);
	}*/

	unsigned int shaderProgram;
	if (Shader::create_shader_program(&shaderProgram, vertRead.c_str(), fragRead.c_str(), nullptr)) {
		return new Shader(shaderProgram);
	}
	else {
		return nullptr;
	}
}
Shader* ResourceLoader<Shader>::load_resource_multi_path(std::string vertPath, std::string fragPath) {
	std::string vertRead = ResourceLoader<std::string>::read_file(vertPath);
	vertRead = fetch_include(vertRead);
	if (vertRead.empty()) {
		std::cout << "File is empty: " << vertRead << std::endl;
		return nullptr;
	}
	std::cout << vertRead << std::endl;

	std::string fragRead = ResourceLoader<std::string>::read_file(fragPath);
	fragRead = fetch_include(fragRead);
	if (fragRead.empty()) {
		std::cout << "File is empty: " << fragRead << std::endl;
		return nullptr;
	}
	std::cout << fragRead << std::endl;

	/*string geoRead = "";
	if (geoFilePath) {
		geoRead = read_shader_file(geoFilePath);
	}*/

	unsigned int shaderProgram;
	if (Shader::create_shader_program(&shaderProgram, vertRead.c_str(), fragRead.c_str(), nullptr)) {
		return new Shader(shaderProgram);
	}
	else {
		return nullptr;
	}
}
std::string ResourceLoader<std::string>::read_file(std::string filePath) {
	std::cout << "Reading: " << filePath << std::endl;

	std::ifstream file(filePath, std::ios::binary | std::ios::ate);

	if (!file) {
		std::cerr << "Failed to read file " << filePath << ". File doesn't exist." << std::endl;
		return nullptr;
	}

	std::streamsize size = file.tellg();
	file.seekg(0, std::ios::beg);

	char* buffer = new char[size + 1]; // +1 for null terminator
	if (!file.read(buffer, size)) {
		delete[] buffer;
		return nullptr;
	}
	buffer[size] = '\0'; // Null-terminate

	return buffer;
}