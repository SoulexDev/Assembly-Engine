#pragma once
#include "../../lib/imgui/imgui.h"

#include <vector>
#include <string>
#include <mutex>

struct ConsoleLog {
	ImVec4 color;
	std::string time;
	std::string msg;
	unsigned int msgCount{ 1 };
};
class Console {
private:
	static void internal_write(std::string data, ImVec4 color);
public:
	static std::vector<ConsoleLog> logs;
	static void write(std::string data);
	static void write_warning(std::string data);
	static void write_error(std::string data);
	static void clear();
};