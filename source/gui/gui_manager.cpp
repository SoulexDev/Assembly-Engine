#include "../../include/gui/gui_manager.h"
#include "../../include/gui/console_gui.h"
#include "../../include/gui/hierarchy_gui.h"
#include "../../include/gui/scene_gui.h"
#include "../../include/graphics/render_pipeline.h"

#include <SDL3/SDL.h>
#include <SDL3/SDL_opengl.h>

#include "../../lib/imgui/imgui.h"
#include "../../lib/imgui/imgui_impl_sdl3.h"
#include "../../lib/imgui/imgui_impl_opengl3.h"

ConsoleGUI* console_gui;
HierarchyGUI* hierarchy_gui;
SceneGUI* scene_gui;

ImGuiIO* io;

void GUIManager::init() {
	IMGUI_CHECKVERSION();
	ImGuiContext* imGuiContext = ImGui::CreateContext();
	io = &ImGui::GetIO(); (void)io;
	io->ConfigFlags |= ImGuiConfigFlags_NavEnableKeyboard;
	io->ConfigFlags |= ImGuiConfigFlags_NavEnableGamepad;
	io->ConfigFlags |= ImGuiConfigFlags_DockingEnable;
	io->ConfigFlags |= ImGuiConfigFlags_ViewportsEnable;

	ImGui::StyleColorsDark();
	setup_imgui_style();

	ImGui_ImplSDL3_InitForOpenGL(RenderPipeline::window, RenderPipeline::glContext);
	ImGui_ImplOpenGL3_Init("#version 460");

	console_gui = new ConsoleGUI();
	console_gui->init();

	hierarchy_gui = new HierarchyGUI();
	hierarchy_gui->init();
	//inspector_gui->assign_console(console);

	scene_gui = new SceneGUI();
	scene_gui->init();

	//console = static_cast<GUIWindow>(new ConsoleGUI());

	//guiWindows.push_back(console);
}
void GUIManager::update_platform() {
	if (io->ConfigFlags & ImGuiConfigFlags_ViewportsEnable)
	{
		SDL_Window* backup_current_window = SDL_GL_GetCurrentWindow();
		SDL_GLContext backup_current_context = SDL_GL_GetCurrentContext();
		ImGui::UpdatePlatformWindows();
		ImGui::RenderPlatformWindowsDefault();
		SDL_GL_MakeCurrent(backup_current_window, backup_current_context);
	}
}
void GUIManager::draw(int screenW, int screenH) {
	ImGui_ImplOpenGL3_NewFrame();
	ImGui_ImplSDL3_NewFrame();
	ImGui::NewFrame();
	ImGui::DockSpaceOverViewport(0, ImGui::GetMainViewport(), ImGuiDockNodeFlags_PassthruCentralNode);

	if (ImGui::BeginMainMenuBar()) {
		if (ImGui::BeginMenu("File")) {
			if (ImGui::MenuItem("New")) {

			}
			if (ImGui::MenuItem("Open")) {

			}
			if (ImGui::MenuItem("Open Recent")) {

			}
			ImGui::EndMenu();
		}
		if (ImGui::BeginMenu("Options")) {
			if (ImGui::MenuItem("Addons")) {

			}
			ImGui::EndMenu();
		}
		ImGui::EndMainMenuBar();
	}

	console_gui->render(screenW, screenH);
	hierarchy_gui->render(screenW, screenH);
	scene_gui->render(screenW, screenH);

	ImGui::Render();
}
void GUIManager::render_data() {
	ImGui_ImplOpenGL3_RenderDrawData(ImGui::GetDrawData());
}
void GUIManager::bind_fbos() {
	scene_gui->bind_fbo();
}
void GUIManager::unbind_fbos() {
	scene_gui->unbind_fbo();
}
void GUIManager::destroy() {
	scene_gui->destroy();

	ImGui_ImplOpenGL3_Shutdown();
	ImGui_ImplSDL3_Shutdown();
	ImGui::DestroyContext();
}
void GUIManager::setup_imgui_style()
{
	ImGuiStyle& style = ImGui::GetStyle();

	style.Alpha = 1.0f;
	style.DisabledAlpha = 0.6f;
	style.WindowPadding = ImVec2(6.0f, 3.0f);
	style.WindowRounding = 0.0f;
	style.WindowBorderSize = 1.0f;
	style.WindowMinSize = ImVec2(32.0f, 32.0f);
	style.WindowTitleAlign = ImVec2(0.5f, 0.5f);
	style.WindowMenuButtonPosition = ImGuiDir_None;
	style.ChildRounding = 0.0f;
	style.ChildBorderSize = 1.0f;
	style.PopupRounding = 0.0f;
	style.PopupBorderSize = 1.0f;
	style.FramePadding = ImVec2(5.0f, 5.0f);
	style.FrameRounding = 1.0f;
	style.FrameBorderSize = 0.0f;
	style.ItemSpacing = ImVec2(7.0f, 1.0f);
	style.ItemInnerSpacing = ImVec2(1.0f, 1.0f);
	style.CellPadding = ImVec2(4.0f, 4.0f);
	style.IndentSpacing = 6.0f;
	style.ColumnsMinSpacing = 6.0f;
	style.ScrollbarSize = 13.0f;
	style.ScrollbarRounding = 16.0f;
	style.GrabMinSize = 20.0f;
	style.GrabRounding = 2.0f;
	style.TabRounding = 2.0f;
	style.TabBorderSize = 0.0f;
	//style.TabMinWidthForCloseButton = 0.0f;
	style.ColorButtonPosition = ImGuiDir_Right;
	style.ButtonTextAlign = ImVec2(0.5f, 0.5f);
	style.SelectableTextAlign = ImVec2(0.0f, 0.0f);

	style.Colors[ImGuiCol_Text] = ImVec4(0.85882354f, 0.92941177f, 0.8862745f, 0.88f);
	style.Colors[ImGuiCol_TextDisabled] = ImVec4(0.85882354f, 0.92941177f, 0.8862745f, 0.28f);
	style.Colors[ImGuiCol_WindowBg] = ImVec4(0.12941177f, 0.13725491f, 0.16862746f, 1.0f);
	style.Colors[ImGuiCol_ChildBg] = ImVec4(0.0f, 0.0f, 0.0f, 0.0f);
	style.Colors[ImGuiCol_PopupBg] = ImVec4(0.2f, 0.21960784f, 0.26666668f, 0.9f);
	style.Colors[ImGuiCol_Border] = ImVec4(0.5372549f, 0.47843137f, 0.25490198f, 0.162f);
	style.Colors[ImGuiCol_BorderShadow] = ImVec4(0.0f, 0.0f, 0.0f, 0.0f);
	style.Colors[ImGuiCol_FrameBg] = ImVec4(0.2f, 0.21960784f, 0.26666668f, 1.0f);
	style.Colors[ImGuiCol_FrameBgHovered] = ImVec4(0.45490196f, 0.19607843f, 0.29803923f, 0.78f);
	style.Colors[ImGuiCol_FrameBgActive] = ImVec4(0.45490196f, 0.19607843f, 0.29803923f, 1.0f);
	style.Colors[ImGuiCol_TitleBg] = ImVec4(0.23137255f, 0.2f, 0.27058825f, 1.0f);
	style.Colors[ImGuiCol_TitleBgActive] = ImVec4(0.5019608f, 0.07450981f, 0.25490198f, 1.0f);
	style.Colors[ImGuiCol_TitleBgCollapsed] = ImVec4(0.2f, 0.21960784f, 0.26666668f, 0.75f);
	style.Colors[ImGuiCol_MenuBarBg] = ImVec4(0.2f, 0.21960784f, 0.26666668f, 0.47f);
	style.Colors[ImGuiCol_ScrollbarBg] = ImVec4(0.2f, 0.21960784f, 0.26666668f, 1.0f);
	style.Colors[ImGuiCol_ScrollbarGrab] = ImVec4(0.08627451f, 0.14901961f, 0.15686275f, 1.0f);
	style.Colors[ImGuiCol_ScrollbarGrabHovered] = ImVec4(0.45490196f, 0.19607843f, 0.29803923f, 0.78f);
	style.Colors[ImGuiCol_ScrollbarGrabActive] = ImVec4(0.45490196f, 0.19607843f, 0.29803923f, 1.0f);
	style.Colors[ImGuiCol_CheckMark] = ImVec4(0.70980394f, 0.21960784f, 0.26666668f, 1.0f);
	style.Colors[ImGuiCol_SliderGrab] = ImVec4(0.46666667f, 0.76862746f, 0.827451f, 0.14f);
	style.Colors[ImGuiCol_SliderGrabActive] = ImVec4(0.70980394f, 0.21960784f, 0.26666668f, 1.0f);
	style.Colors[ImGuiCol_Button] = ImVec4(0.46666667f, 0.76862746f, 0.827451f, 0.14f);
	style.Colors[ImGuiCol_ButtonHovered] = ImVec4(0.45490196f, 0.19607843f, 0.29803923f, 0.86f);
	style.Colors[ImGuiCol_ButtonActive] = ImVec4(0.45490196f, 0.19607843f, 0.29803923f, 1.0f);
	style.Colors[ImGuiCol_Header] = ImVec4(0.45490196f, 0.19607843f, 0.29803923f, 0.76f);
	style.Colors[ImGuiCol_HeaderHovered] = ImVec4(0.45490196f, 0.19607843f, 0.29803923f, 0.86f);
	style.Colors[ImGuiCol_HeaderActive] = ImVec4(0.5019608f, 0.07450981f, 0.25490198f, 1.0f);
	style.Colors[ImGuiCol_Separator] = ImVec4(0.42745098f, 0.42745098f, 0.49803922f, 0.5f);
	style.Colors[ImGuiCol_SeparatorHovered] = ImVec4(0.5176471f, 0.21960784f, 0.3372549f, 0.88412017f);
	style.Colors[ImGuiCol_SeparatorActive] = ImVec4(0.45490196f, 0.19607843f, 0.29803923f, 1.0f);
	style.Colors[ImGuiCol_ResizeGrip] = ImVec4(0.46666667f, 0.76862746f, 0.827451f, 0.04f);
	style.Colors[ImGuiCol_ResizeGripHovered] = ImVec4(0.45490196f, 0.19607843f, 0.29803923f, 0.78f);
	style.Colors[ImGuiCol_ResizeGripActive] = ImVec4(0.45490196f, 0.19607843f, 0.29803923f, 1.0f);
	style.Colors[ImGuiCol_Tab] = ImVec4(0.70980394f, 0.21960784f, 0.26666668f, 0.82832617f);
	style.Colors[ImGuiCol_TabHovered] = ImVec4(0.70980394f, 0.21960784f, 0.26666668f, 1.0f);
	style.Colors[ImGuiCol_TabActive] = ImVec4(0.70980394f, 0.21960784f, 0.26666668f, 1.0f);
	style.Colors[ImGuiCol_TabUnfocused] = ImVec4(0.06666667f, 0.101960786f, 0.14509805f, 0.9724f);
	style.Colors[ImGuiCol_TabUnfocusedActive] = ImVec4(0.45490196f, 0.19607843f, 0.29803923f, 1.0f);
	style.Colors[ImGuiCol_PlotLines] = ImVec4(0.85882354f, 0.92941177f, 0.8862745f, 0.63f);
	style.Colors[ImGuiCol_PlotLinesHovered] = ImVec4(0.45490196f, 0.19607843f, 0.29803923f, 1.0f);
	style.Colors[ImGuiCol_PlotHistogram] = ImVec4(0.85882354f, 0.92941177f, 0.8862745f, 0.63f);
	style.Colors[ImGuiCol_PlotHistogramHovered] = ImVec4(0.45490196f, 0.19607843f, 0.29803923f, 1.0f);
	style.Colors[ImGuiCol_TableHeaderBg] = ImVec4(0.1882353f, 0.1882353f, 0.2f, 1.0f);
	style.Colors[ImGuiCol_TableBorderStrong] = ImVec4(0.30980393f, 0.30980393f, 0.34901962f, 1.0f);
	style.Colors[ImGuiCol_TableBorderLight] = ImVec4(0.22745098f, 0.22745098f, 0.24705882f, 1.0f);
	style.Colors[ImGuiCol_TableRowBg] = ImVec4(0.0f, 0.0f, 0.0f, 0.0f);
	style.Colors[ImGuiCol_TableRowBgAlt] = ImVec4(1.0f, 1.0f, 1.0f, 0.06f);
	style.Colors[ImGuiCol_TextSelectedBg] = ImVec4(0.45490196f, 0.19607843f, 0.29803923f, 0.43f);
	style.Colors[ImGuiCol_DragDropTarget] = ImVec4(1.0f, 1.0f, 0.0f, 0.9f);
	style.Colors[ImGuiCol_NavHighlight] = ImVec4(0.25882354f, 0.5882353f, 0.9764706f, 1.0f);
	style.Colors[ImGuiCol_NavWindowingHighlight] = ImVec4(1.0f, 1.0f, 1.0f, 0.7f);
	style.Colors[ImGuiCol_NavWindowingDimBg] = ImVec4(0.8f, 0.8f, 0.8f, 0.2f);
	style.Colors[ImGuiCol_ModalWindowDimBg] = ImVec4(0.8f, 0.8f, 0.8f, 0.35f);
}