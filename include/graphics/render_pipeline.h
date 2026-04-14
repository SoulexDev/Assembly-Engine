#pragma once
#include "../core/camera.h"
#include "renderable.h"
#include "shader.h"
#include "textures/texture2d.h"
#include "textures/depth_texture.h"
#include "post_processing/post_effect.h"

#include <vector>

#include <SDL3/SDL.h>
#include <SDL3/SDL_opengl.h>

class RenderPipeline {
public:
	static SDL_Window* window;
	static SDL_GLContext glContext;

	static RenderTexture renderTex;

	static std::pair<RenderTexture, RenderTexture> buffers;
	//static RenderTexture buffer1;

	static std::vector<Renderable> renderables;
	static std::vector<PostEffect> postEffects;

	static DepthTexture shadowTex;
	static Camera sunCam;
	static Shader shadowShader;
	
	static int init();
	static void push_post_effect(PostEffect postEffect);
	static void pop_post_effect();
	static void clear_post_effects();
	static void render();
	static void on_event(SDL_Event* event);
	static void post_process();
	static void destroy();
	static void resize_window();
};