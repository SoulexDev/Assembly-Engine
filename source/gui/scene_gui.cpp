#include "../../include/gui/scene_gui.h"
#include "../../include/core/console.h"
#include "../../include/graphics/textures/render_texture.h"
#include "../../lib/imgui/imgui.h"

#include <glad/glad.h>

static RenderTexture* renderTexture;
void SceneGUI::init() {
    renderTexture = new RenderTexture(800, 600);
}
void SceneGUI::render(int screenW, int screenH) {
    ImGui::Begin("Scene");

    const float w = ImGui::GetContentRegionAvail().x;
    const float h = ImGui::GetContentRegionAvail().y;

    renderTexture->rescale_framebuffer(w, h);
    glViewport(0, 0, w, h);

    // we get the screen position of the window
    //ImVec2 pos = ImGui::GetCursorScreenPos();

    // and here we can add our created texture as image to ImGui
    // unfortunately we need to use the cast to void* or I didn't find another way tbh
   /* ImGui::GetWindowDrawList()->AddImage(
        (void*)(renderTexture->texture),
        ImVec2(pos.x, pos.y),
        ImVec2(pos.x + w, pos.y + h),
        ImVec2(0, 1),
        ImVec2(1, 0)
    );*/
    //unsigned int tex = *renderTexture;

    //console->write("Render Texture: " + to_string(tex));

    ImGui::Image((ImTextureID)(intptr_t)(renderTexture->texture), ImVec2(w, h), ImVec2(0, 1), ImVec2(1, 0));

    ImGui::End();
}
void SceneGUI::bind_fbo() {
    renderTexture->bind_framebuffer();
}
void SceneGUI::unbind_fbo() {
    renderTexture->unbind_framebuffer();
}
void SceneGUI::destroy() {
    renderTexture->destroy();
}