#include "../../include/gui/hierarchy_gui.h"
#include "../../lib/imgui/imgui.h"
#include "../../include/core/engine.h"
#include "../../include/core/console.h"

#include <string>

void HierarchyGUI::render(int screenW, int screenH) {
	ImGui::Begin("Hierarchy");

	//ImGui::Text("%s", "Hierarchy");
	//char textBuffer[256] = {'\0'};

	//ImGui::InputText("Search", textBuffer, sizeof(textBuffer));
	ImGui::Separator();

	//if (Engine::objects.size() > 0) {
	//	/*if (ImGui::TreeNodeEx(Engine::objects[0].name.c_str(), ImGuiTreeNodeFlags_DefaultOpen)) {
	//		if (ImGui::TreeNodeEx(Engine::objects[1].name.c_str(), ImGuiTreeNodeFlags_DefaultOpen)) {
	//			if (ImGui::TreeNodeEx(Engine::objects[2].name.c_str(), ImGuiTreeNodeFlags_Leaf)) {

	//				ImGui::TreePop();
	//			}
	//			ImGui::TreePop();
	//		}
	//		ImGui::TreePop();
	//	}*/
	//	//bool nodeOpen = false;
	//	//ImGuiTreeNodeFlags nodeFlags = ImGuiTreeNodeFlags_DefaultOpen;
	//	for (int i = 0; i < Engine::objects.size(); ++i)
	//	{
	//		EngineObject obj = Engine::objects[i];
	//		/*int prevIndex = i - 1;
	//		if (prevIndex < 0)
	//			prevIndex = 0;*/

	//		if (!obj.transform.has_parent())
	//			draw_nodes(obj, obj.transform.children);

	//		/*Console::write("OBJECT: [" + obj.name + "]");
	//		for (int i = 0; i < obj.transform.child_count(); ++i)
	//		{
	//			Console::write("[Child: " + std::to_string(i) + "]	" + obj.transform.children[i].engineObject->name);
	//		}*/
	//	}
	//}

	//delete[] textBuffer;
	ImGui::End();
}
//void HierarchyGUI::draw_nodes(EngineObject& obj, std::vector<Transform> children) {
//	ImGuiTreeNodeFlags nodeFlags = children.size() > 0 ? ImGuiTreeNodeFlags_DefaultOpen : ImGuiTreeNodeFlags_Leaf;
//	//ImGuiTreeNodeFlags nodeFlags = ImGuiTreeNodeFlags_DefaultOpen;
//	if (ImGui::TreeNodeEx(obj.name.c_str(), nodeFlags)) {
//		for (const Transform t : children)
//		{
//			draw_nodes(*t.engineObject, t.children);
//		}
//
//		ImGui::TreePop();
//	}
//}
ImGuiInputTextCallback HierarchyGUI::search_callback() {
	return nullptr;
}