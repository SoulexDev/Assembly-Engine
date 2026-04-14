#include "../../include/gui/console_gui.h"
#include "../../include/core/console.h"
#include "../../lib/imgui/imgui.h"

ImVec2 normalize_coordinates(ImVec2 input, int w, int h) {
	input.x -= w / 2;
	input.y += h / 2;
	return input;
}
void ConsoleGUI::init() {

}
void ConsoleGUI::render(int screenW, int screenH) {
	ImGui::Begin("Console");

	//::
	if (ImGui::Button("Clear")) {
		Console::clear();
	}

	for (ConsoleLog log : Console::logs)
	{
		std::string text = "x" + std::to_string(log.msgCount) + " " + log.time + ": " + log.msg;
		ImGui::Separator();
		ImGui::TextColored(log.color, text.c_str());
	}
	/*for (int i = 0; i < console->logs.size() * sizeof(ConsoleLog); ++i)
	{
		ConsoleLog log = console->logs[i];


	}*/

	ImGui::End();
}