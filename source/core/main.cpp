//TODO: type shi...
//implement render pipepline
// - post processing
// - material sorting to reduce state switches !!
// - shadow mapping
// - shader editor
// - uniform buffer objects
// - buffer sub data manipulation, to allow quick mesh updates
#include "../../include/core/main.h"
#include "../../include/graphics/shader.h"
#include "../../include/graphics/textures/render_texture.h"
#include "../../include/core/console.h"
#include "../../include/graphics/textures/texture2d.h"
#include "../../include/core/resource_loader.h"
#include "../../include/graphics/renderable.h"
#include "../../include/core/camera.h"
#include "../../include/core/time.h"
#include "../../include/core/input.h"
#include "../../include/gui/gui_manager.h"
#include "../../include/core/engine_object.h"
#include "../../include/core/engine.h"
#include "../../include/graphics/models/cube.h"

#include "../../include/graphics/render_pipeline.h"

#include <string>
#include <vector>

#define SDL_MAIN_USE_CALLBACKS 1  /* use the callbacks instead of main() */

#include <glad/glad.h>
#include <SDL3/SDL.h>
#include <SDL3/SDL_main.h>
#include <SDL3/SDL_opengl.h>

#include <glm/glm.hpp>
#include <glm/gtc/matrix_transform.hpp>
#include <glm/gtc/type_ptr.hpp>

#include "../../lib/imgui/imgui_impl_sdl3.h"
#include "../../game/include/world/world_manager.h"
#include "../../game/include/player/player_movement.h"

PlayerMovement player;

SDL_AppResult SDL_AppInit(void** appstate, int argc, char* argv[])
{
    if (RenderPipeline::init() == -1)
        return SDL_APP_FAILURE;

    Input::init();
    player = *new PlayerMovement();
    WorldManager::generate();

    /*for (int i = 0; i < Engine::objects.size(); ++i)
    {
        EngineObject obj = Engine::objects[i];
        for (int i = 0; i < obj.components.size(); i++)
        {
            obj.components[i].init();
        }
    }*/

    return SDL_APP_CONTINUE;
}

SDL_AppResult SDL_AppEvent(void* appstate, SDL_Event* event)
{
    //ImGui_ImplSDL3_ProcessEvent(event);
    if (event->type == SDL_EVENT_QUIT) {
        return SDL_APP_SUCCESS;
    }
    Input::on_event(event);
    RenderPipeline::on_event(event);

    return SDL_APP_CONTINUE;
}

SDL_AppResult SDL_AppIterate(void* appstate)
{
    //set the current keyboard state
    Input::set_current_state();

    //update basic engine features
    Time::update();
    Input::update();

    player.update();

    RenderPipeline::render();

    //store the current keyboard state for next frame
    Input::set_last_state();

    return SDL_APP_CONTINUE;
}

void SDL_AppQuit(void* appstate, SDL_AppResult result)
{
    /*for (int i = 0; i < Engine::objects.size(); ++i)
    {
        EngineObject obj = Engine::objects[i];
        for (int i = 0; i < obj.components.size(); i++)
        {
            obj.components[i].destroy();
        }
    }*/

    RenderPipeline::destroy();

    SDL_Quit();
}