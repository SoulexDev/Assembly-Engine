#include "../../include/graphics/render_pipeline.h"
#include "../../include/core/engine.h"
#include "../../include/gui/gui_manager.h"
#include "../../include/core/resource_loader.h"
#include "../../include/graphics/models/fullscreen_quad_mesh.h"
#include "../../include/core/input.h"
#include "../../include/graphics/textures/depth_texture.h"

#include <iostream>
#include <SDL3/SDL.h>
#include <SDL3/SDL_opengl.h>


SDL_Window* RenderPipeline::window{ nullptr };
SDL_GLContext RenderPipeline::glContext{ nullptr };

RenderTexture RenderPipeline::renderTex;

std::pair<RenderTexture, RenderTexture> RenderPipeline::buffers;

std::vector<Renderable> RenderPipeline::renderables;
std::vector<PostEffect> RenderPipeline::postEffects;

DepthTexture RenderPipeline::shadowTex;
Camera RenderPipeline::sunCam;
Shader RenderPipeline::shadowShader;

int RenderPipeline::init() {
    //Initialize SDL window and opengl
    if (SDL_Init(SDL_INIT_VIDEO | SDL_INIT_AUDIO) == false) {
        SDL_Log("Couldn't initialize video or audio: %s", SDL_GetError());
        return -1;
    }

    SDL_GL_LoadLibrary(nullptr);

    /* Create the window */
    window = SDL_CreateWindow("Fantasy Engine", Engine::screenWidth, Engine::screenHeight, SDL_WINDOW_OPENGL | SDL_WINDOW_RESIZABLE);
    if (window == nullptr) {
        SDL_Log("Couldn't create window: %s", SDL_GetError());
        return -1;
    }

    SDL_GL_SetAttribute(SDL_GL_CONTEXT_MAJOR_VERSION, 4);
    SDL_GL_SetAttribute(SDL_GL_CONTEXT_MINOR_VERSION, 6);
    SDL_GL_SetAttribute(SDL_GL_CONTEXT_PROFILE_MASK, SDL_GL_CONTEXT_PROFILE_CORE);

    glContext = SDL_GL_CreateContext(window);

    if (glContext == NULL) {
        SDL_Log("Couldn't create OpenGL context: %s", SDL_GetError());
        return -1;
    }

    //load opengl function pointers
    if (!gladLoadGLLoader((GLADloadproc)SDL_GL_GetProcAddress)) {
        SDL_Log("Failed to initialize GLAD: %s", SDL_GetError());
        return -1;
    }

    //VSync, btw
    if (!SDL_GL_SetSwapInterval(1)) {
        SDL_Log("Could not set vsync: %s", SDL_GetError());
        return -1;
    }

    //Initialize GUI
    //GUIManager::init();

    FullscreenQuadMesh::create();
    renderTex = *new RenderTexture(Engine::screenWidth, Engine::screenHeight);

    shadowTex = *new DepthTexture(2048, 2048);
    sunCam = *new Camera();
    sunCam.projectionType = CameraProjectionType_Orthographic;
    //sunCam.projectionMatrix = glm::ortho(-32.0f, 32.0f, -32.0f, 32.0f, 1.0f, 100.0f);
    sunCam.cameraNear = 1.0f;
    sunCam.cameraFar = 100.0f;
    sunCam.orthoWidth = 32.0f;
    sunCam.orthoHeight = 32.0f;
    sunCam.transform.set_position(glm::vec3(-16.0f, 64.0f, -16.0f));
    sunCam.transform.set_rotation(
        glm::angleAxis(glm::radians(45.0f), glm::vec3(1.0f, 0.0f, 0.0f)) * 
        glm::angleAxis(glm::radians(135.0f), glm::vec3(0.0f, 1.0f, 0.0f)));
    //sunCam.transform.set_position(glm::vec3(-1.0f, 16.0f, -1.0f));
    //sunCam.viewMatrix = glm::lookAt(glm::vec3(16.0f, 32.0f, 16.0f), glm::vec3(0.0f, 0.0f, 0.0f), glm::vec3(0.0f, 1.0f, 0.0f));

    buffers = std::make_pair(
        *new RenderTexture(Engine::screenWidth, Engine::screenHeight), 
        *new RenderTexture(Engine::screenWidth, Engine::screenHeight));

    Shader* gammaShader = ResourceLoader<Shader>::load_resource_multi_path(
        "H:/GitRepos/AssemblyEngine/resources/shaders/internal/post_effect.vert",
        "H:/GitRepos/AssemblyEngine/resources/shaders/internal/gamma_correction_effect.frag");

    if (gammaShader != nullptr) {
        push_post_effect(*new PostEffect(*gammaShader));
    }

    shadowShader = *ResourceLoader<Shader>::load_resource("H:/GitRepos/AssemblyEngine/resources/shaders/internal/shadow.vert");

    //Create camera
    Camera::main = *new Camera();
    Camera::main.init();

    return 0;
}
void RenderPipeline::push_post_effect(PostEffect postEffect) {
    postEffects.push_back(postEffect);
}
void RenderPipeline::pop_post_effect() {
    postEffects.pop_back();
}
void RenderPipeline::clear_post_effects() {
    postEffects.clear();
}
void RenderPipeline::render() {
    //int windowW;
    //int windowH;
    //SDL_GetWindowSize(window, &windowW, &windowH);

    //GUI
    /*GUIManager::draw(windowW, windowH);
    GUIManager::bind_fbos();*/

    sunCam.transform.set_position(Camera::main.transform.get_position() + glm::vec3(-1.0f, 1.0f, -1.0f) * 16.0f);
    glEnable(GL_DEPTH_TEST);

    //Shadows
    glViewport(0, 0, shadowTex.width, shadowTex.height);

    shadowTex.bind_framebuffer();
    glClear(GL_DEPTH_BUFFER_BIT);

    //sunCam.update();
    sunCam.set_matrices();

    //render scene
    glDisable(GL_CULL_FACE);
    for (Renderable renderable : renderables)
    {
        renderable.draw(sunCam, shadowShader);
    }
    glEnable(GL_CULL_FACE);

    shadowTex.unbind_framebuffer();

    glViewport(0, 0, Engine::screenWidth, Engine::screenHeight);

    if (postEffects.size() > 0)
        buffers.first.bind_framebuffer();

    //Clear background
    glClearColor(0.4, 0.4, 0.5, 1.0);
    glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);

    //Update camera
    Camera::main.set_matrices();

    //render scene
    for (Renderable renderable : renderables)
    {
        renderable.draw(Camera::main);
    }

    if (postEffects.size() > 0)
        buffers.first.unbind_framebuffer();

    glDisable(GL_DEPTH_TEST);

    //post processing
    if (postEffects.size() > 0)
        post_process();

    /*GUIManager::unbind_fbos();
    GUIManager::render_data();
    GUIManager::update_platform();*/

    SDL_GL_SwapWindow(window);
}
void RenderPipeline::post_process() {
    std::swap(buffers.first, buffers.second);
    bool isOutput = false;
    for (int i = 0; i < postEffects.size(); ++i)
    {
        isOutput = i == postEffects.size() - 1;
        //isOutput = false;
        if (!isOutput)
            buffers.first.bind_framebuffer();

        postEffects[i].render(buffers.second.texture);

        if (!isOutput)
            buffers.first.unbind_framebuffer();

        std::swap(buffers.first, buffers.second);
    }

    //postEffects[0].render(shadowTex.texture);
}
void RenderPipeline::on_event(SDL_Event* event) {
    if (event->type == SDL_EVENT_WINDOW_RESIZED) {
        resize_window();
    }
}
void RenderPipeline::resize_window() {
    int w, h;
    SDL_GetWindowSize(window, &w, &h);
    glViewport(0, 0, w, h);
}
void RenderPipeline::destroy() {
    //GUIManager::destroy();
    for (Renderable renderable : renderables) {
		renderable.destroy();
    }
    SDL_GL_UnloadLibrary();
}