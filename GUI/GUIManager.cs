using AssemblyEngine.Graphics;
using Hexa.NET.ImGui;
using Hexa.NET.ImGui.Backends.OpenGL3;
using Hexa.NET.ImGui.Backends.SDL3;
using System.Numerics;
using static SDL3.SDL;

namespace AssemblyEngine.GUI
{
    public class GUIManager
    {
        private static List<GUIWindow> guiWindows = new List<GUIWindow>();
        private static ImGuiContextPtr imguiContextPtr;

        private static bool guiEnabled = false;

        public static void Init()
        {
            imguiContextPtr = ImGui.CreateContext();

            var io = ImGui.GetIO();
            io.ConfigFlags |= ImGuiConfigFlags.NavEnableKeyboard;
            io.ConfigFlags |= ImGuiConfigFlags.NavEnableGamepad;
            io.ConfigFlags |= ImGuiConfigFlags.DockingEnable;
            io.ConfigFlags |= ImGuiConfigFlags.ViewportsEnable;

            ImGui.StyleColorsDark();
            SetStyle();

            ImGuiImplSDL3.SetCurrentContext(imguiContextPtr);
            unsafe
            {
                if (!ImGuiImplSDL3.InitForOpenGL(new SDLWindowPtr { Handle = (SDLWindow*)RenderPipeline.window.ToPointer() }, RenderPipeline.glContext.ToPointer()))
                {
                    Console.WriteLine("ImGui unable to initialize for SDL3");
                }
            }

            ImGuiImplOpenGL3.SetCurrentContext(imguiContextPtr);
            if (!ImGuiImplOpenGL3.Init("#version 460 core"))
            {
                Console.WriteLine("ImGui unable to initialize for OpenGL");
            }

            //testing
            HierarchyGUI hierarchyGUI = new HierarchyGUI();
            InspectorGUI inspectorGUI = new InspectorGUI();
        }
        public static void EnableGUI()
        {
            guiEnabled = true;
        }
        public static void DisableGUI()
        {
            guiEnabled = false;
        }
        public static unsafe void HandleEvents(SDL_Event* sdlEvent)
        {
            ImGuiImplSDL3.ProcessEvent(new SDLEventPtr((SDLEvent*)sdlEvent));
        }
        public static void Render()
        {
            if (!guiEnabled)
                return;

            ImGuiImplOpenGL3.NewFrame();
            ImGuiImplSDL3.NewFrame();
            ImGui.NewFrame();
            ImGui.DockSpaceOverViewport(ImGui.GetMainViewport(), ImGuiDockNodeFlags.PassthruCentralNode);

            if (ImGui.BeginMainMenuBar())
            {
                if (ImGui.BeginMenu("File"))
                {
                    if (ImGui.MenuItem("New"))
                    {

                    }
                    if (ImGui.MenuItem("Open"))
                    {

                    }
                    if (ImGui.MenuItem("Open Recent"))
                    {

                    }
                    ImGui.EndMenu();
                }
                if (ImGui.BeginMenu("Options"))
                {
                    if (ImGui.MenuItem("Addons"))
                    {

                    }
                    ImGui.EndMenu();
                }
                ImGui.EndMainMenuBar();
            }
            foreach (GUIWindow guiWindow in guiWindows)
            {
                guiWindow.Draw();
            }

            ImGui.Render();
            ImGui.EndFrame();

            ImGuiImplOpenGL3.RenderDrawData(ImGui.GetDrawData());

            var io = ImGui.GetIO();
            if ((io.ConfigFlags & ImGuiConfigFlags.ViewportsEnable) != 0)
            {
                nint backupCurrentWindow = SDL_GL_GetCurrentWindow();
                nint backupCurrentContext = SDL_GL_GetCurrentContext();
                ImGui.UpdatePlatformWindows();
                ImGui.RenderPlatformWindowsDefault();
                SDL_GL_MakeCurrent(backupCurrentWindow, backupCurrentContext);
            }
        }
        private static void SetStyle()
        {
            ImGuiStylePtr style = ImGui.GetStyle();

            style.Alpha = 1.0f;
            style.DisabledAlpha = 0.6f;
            style.WindowPadding = new Vector2(6.0f, 3.0f);
            style.WindowRounding = 0.0f;
            style.WindowBorderSize = 1.0f;
            style.WindowMinSize = new Vector2(32.0f, 32.0f);
            style.WindowTitleAlign = new Vector2(0.5f, 0.5f);
            style.WindowMenuButtonPosition = ImGuiDir.None;
            style.ChildRounding = 0.0f;
            style.ChildBorderSize = 1.0f;
            style.PopupRounding = 0.0f;
            style.PopupBorderSize = 1.0f;
            style.FramePadding = new Vector2(5.0f, 5.0f);
            style.FrameRounding = 1.0f;
            style.FrameBorderSize = 0.0f;
            style.ItemSpacing = new Vector2(7.0f, 1.0f);
            style.ItemInnerSpacing = new Vector2(1.0f, 1.0f);
            style.CellPadding = new Vector2(4.0f, 4.0f);
            style.IndentSpacing = 6.0f;
            style.ColumnsMinSpacing = 6.0f;
            style.ScrollbarSize = 13.0f;
            style.ScrollbarRounding = 16.0f;
            style.GrabMinSize = 20.0f;
            style.GrabRounding = 2.0f;
            style.TabRounding = 2.0f;
            style.TabBorderSize = 0.0f;
            style.TabCloseButtonMinWidthSelected = 0.0f;
            style.ColorButtonPosition = ImGuiDir.Right;
            style.ButtonTextAlign = new Vector2(0.5f, 0.5f);
            style.SelectableTextAlign = new Vector2(0.0f, 0.0f);
            style.Colors[(int)ImGuiCol.Text] = new Vector4(0.85882354f, 0.92941177f, 0.8862745f, 0.88f);
            style.Colors[(int)ImGuiCol.TextDisabled] = new Vector4(0.85882354f, 0.92941177f, 0.8862745f, 0.28f);
            style.Colors[(int)ImGuiCol.WindowBg] = new Vector4(0.12941177f, 0.13725491f, 0.16862746f, 1.0f);
            style.Colors[(int)ImGuiCol.ChildBg] = new Vector4(0.0f, 0.0f, 0.0f, 0.0f);
            style.Colors[(int)ImGuiCol.PopupBg] = new Vector4(0.2f, 0.21960784f, 0.26666668f, 0.9f);
            style.Colors[(int)ImGuiCol.Border] = new Vector4(0.5372549f, 0.47843137f, 0.25490198f, 0.162f);
            style.Colors[(int)ImGuiCol.BorderShadow] = new Vector4(0.0f, 0.0f, 0.0f, 0.0f);
            style.Colors[(int)ImGuiCol.FrameBg] = new Vector4(0.2f, 0.21960784f, 0.26666668f, 1.0f);
            style.Colors[(int)ImGuiCol.FrameBgHovered] = new Vector4(0.45490196f, 0.19607843f, 0.29803923f, 0.78f);
            style.Colors[(int)ImGuiCol.FrameBgActive] = new Vector4(0.45490196f, 0.19607843f, 0.29803923f, 1.0f);
            style.Colors[(int)ImGuiCol.TitleBg] = new Vector4(0.23137255f, 0.2f, 0.27058825f, 1.0f);
            style.Colors[(int)ImGuiCol.TitleBgActive] = new Vector4(0.5019608f, 0.07450981f, 0.25490198f, 1.0f);
            style.Colors[(int)ImGuiCol.TitleBgCollapsed] = new Vector4(0.2f, 0.21960784f, 0.26666668f, 0.75f);
            style.Colors[(int)ImGuiCol.MenuBarBg] = new Vector4(0.2f, 0.21960784f, 0.26666668f, 0.47f);
            style.Colors[(int)ImGuiCol.ScrollbarBg] = new Vector4(0.2f, 0.21960784f, 0.26666668f, 1.0f);
            style.Colors[(int)ImGuiCol.ScrollbarGrab] = new Vector4(0.08627451f, 0.14901961f, 0.15686275f, 1.0f);
            style.Colors[(int)ImGuiCol.ScrollbarGrabHovered] = new Vector4(0.45490196f, 0.19607843f, 0.29803923f, 0.78f);
            style.Colors[(int)ImGuiCol.ScrollbarGrabActive] = new Vector4(0.45490196f, 0.19607843f, 0.29803923f, 1.0f);
            style.Colors[(int)ImGuiCol.CheckMark] = new Vector4(0.70980394f, 0.21960784f, 0.26666668f, 1.0f);
            style.Colors[(int)ImGuiCol.SliderGrab] = new Vector4(0.46666667f, 0.76862746f, 0.827451f, 0.14f);
            style.Colors[(int)ImGuiCol.SliderGrabActive] = new Vector4(0.70980394f, 0.21960784f, 0.26666668f, 1.0f);
            style.Colors[(int)ImGuiCol.Button] = new Vector4(0.46666667f, 0.76862746f, 0.827451f, 0.14f);
            style.Colors[(int)ImGuiCol.ButtonHovered] = new Vector4(0.45490196f, 0.19607843f, 0.29803923f, 0.86f);
            style.Colors[(int)ImGuiCol.ButtonActive] = new Vector4(0.45490196f, 0.19607843f, 0.29803923f, 1.0f);
            style.Colors[(int)ImGuiCol.Header] = new Vector4(0.45490196f, 0.19607843f, 0.29803923f, 0.76f);
            style.Colors[(int)ImGuiCol.HeaderHovered] = new Vector4(0.45490196f, 0.19607843f, 0.29803923f, 0.86f);
            style.Colors[(int)ImGuiCol.HeaderActive] = new Vector4(0.5019608f, 0.07450981f, 0.25490198f, 1.0f);
            style.Colors[(int)ImGuiCol.Separator] = new Vector4(0.42745098f, 0.42745098f, 0.49803922f, 0.5f);
            style.Colors[(int)ImGuiCol.SeparatorHovered] = new Vector4(0.5176471f, 0.21960784f, 0.3372549f, 0.88412017f);
            style.Colors[(int)ImGuiCol.SeparatorActive] = new Vector4(0.45490196f, 0.19607843f, 0.29803923f, 1.0f);
            style.Colors[(int)ImGuiCol.ResizeGrip] = new Vector4(0.46666667f, 0.76862746f, 0.827451f, 0.04f);
            style.Colors[(int)ImGuiCol.ResizeGripHovered] = new Vector4(0.45490196f, 0.19607843f, 0.29803923f, 0.78f);
            style.Colors[(int)ImGuiCol.ResizeGripActive] = new Vector4(0.45490196f, 0.19607843f, 0.29803923f, 1.0f);
            style.Colors[(int)ImGuiCol.Tab] = new Vector4(0.70980394f, 0.21960784f, 0.26666668f, 0.82832617f);
            style.Colors[(int)ImGuiCol.TabHovered] = new Vector4(0.70980394f, 0.21960784f, 0.26666668f, 1.0f);
            style.Colors[(int)ImGuiCol.TabSelected] = new Vector4(0.70980394f, 0.21960784f, 0.26666668f, 1.0f);
            style.Colors[(int)ImGuiCol.TabDimmed] = new Vector4(0.06666667f, 0.101960786f, 0.14509805f, 0.9724f);
            style.Colors[(int)ImGuiCol.TabDimmedSelected] = new Vector4(0.45490196f, 0.19607843f, 0.29803923f, 1.0f);
            style.Colors[(int)ImGuiCol.PlotLines] = new Vector4(0.85882354f, 0.92941177f, 0.8862745f, 0.63f);
            style.Colors[(int)ImGuiCol.PlotLinesHovered] = new Vector4(0.45490196f, 0.19607843f, 0.29803923f, 1.0f);
            style.Colors[(int)ImGuiCol.PlotHistogram] = new Vector4(0.85882354f, 0.92941177f, 0.8862745f, 0.63f);
            style.Colors[(int)ImGuiCol.PlotHistogramHovered] = new Vector4(0.45490196f, 0.19607843f, 0.29803923f, 1.0f);
            style.Colors[(int)ImGuiCol.TableHeaderBg] = new Vector4(0.1882353f, 0.1882353f, 0.2f, 1.0f);
            style.Colors[(int)ImGuiCol.TableBorderStrong] = new Vector4(0.30980393f, 0.30980393f, 0.34901962f, 1.0f);
            style.Colors[(int)ImGuiCol.TableBorderLight] = new Vector4(0.22745098f, 0.22745098f, 0.24705882f, 1.0f);
            style.Colors[(int)ImGuiCol.TableRowBg] = new Vector4(0.0f, 0.0f, 0.0f, 0.0f);
            style.Colors[(int)ImGuiCol.TableRowBgAlt] = new Vector4(1.0f, 1.0f, 1.0f, 0.06f);
            style.Colors[(int)ImGuiCol.TextSelectedBg] = new Vector4(0.45490196f, 0.19607843f, 0.29803923f, 0.43f);
            style.Colors[(int)ImGuiCol.DragDropTarget] = new Vector4(1.0f, 1.0f, 0.0f, 0.9f);
            style.Colors[(int)ImGuiCol.NavCursor] = new Vector4(0.25882354f, 0.5882353f, 0.9764706f, 1.0f);
            style.Colors[(int)ImGuiCol.NavWindowingHighlight] = new Vector4(1.0f, 1.0f, 1.0f, 0.7f);
            style.Colors[(int)ImGuiCol.NavWindowingDimBg] = new Vector4(0.8f, 0.8f, 0.8f, 0.2f);
            style.Colors[(int)ImGuiCol.ModalWindowDimBg] = new Vector4(0.8f, 0.8f, 0.8f, 0.35f);
        }
        internal static void AddWindow(GUIWindow guiWindow)
        {
            guiWindows.Add(guiWindow);
        }
        internal static void RemoveWindow(GUIWindow guiWindow)
        {
            guiWindows.Remove(guiWindow);
        }
    }
}