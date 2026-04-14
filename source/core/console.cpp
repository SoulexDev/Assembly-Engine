#include "../../include/core/console.h"

#include <SDL3/SDL.h>

#include <string>
#include <vector>

std::vector<ConsoleLog> Console::logs;

ConsoleLog lastLog;
void Console::internal_write(std::string data, ImVec4 color) {
	if (lastLog.msg == data) {
		lastLog.msgCount += 1;
		return;
	}

	ConsoleLog consoleLog;
	consoleLog.color = color;
	consoleLog.msg = data;
	consoleLog.msgCount = 1;

	SDL_Time ticks;

	if (SDL_GetCurrentTime(&ticks)) {
		SDL_DateTime dateTime;

		if (SDL_TimeToDateTime(ticks, &dateTime, true)) {
			int hour = dateTime.hour;
			if (hour > 12)
				hour -= 12;

			consoleLog.time =
				"[" + std::to_string(hour) +
				":" + std::to_string(dateTime.minute) +
				":" + std::to_string(dateTime.second) + "]";
		}
	}

	logs.push_back(consoleLog);

	lastLog = consoleLog;
}
void Console::write(std::string data) {
	internal_write(data, ImVec4(1.0f, 1.0f, 1.0f, 1.0f));
}
void Console::write_warning(std::string data) {
	internal_write(data, ImVec4(0.15f, 1.0f, 0.1f, 1.0f));
}
void Console::write_error(std::string data) {
	internal_write(data, ImVec4(1.0f, 0.1f, 0.15f, 1.0f));
}
void Console::clear() {
	logs.clear();
	lastLog.msg = "";
}