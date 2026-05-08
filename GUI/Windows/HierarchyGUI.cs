using AssemblyEngine.SceneManagement;
using Hexa.NET.ImGui;

namespace AssemblyEngine.GUI
{
    internal class HierarchyGUI : GUIWindow
    {
        private string renameBuffer;
        public override void Draw()
        {
            ImGui.Begin("Hierarchy");
            ImGui.Separator();

            ImGuiTreeNodeFlags nodeFlags = ImGuiTreeNodeFlags.DefaultOpen | ImGuiTreeNodeFlags.OpenOnArrow;

            foreach (var scene in SceneManager.loadedScenes)
            {
                if (ImGui.TreeNodeEx(scene.name, nodeFlags))
                {
                    foreach (var engineObject in scene.engineObjects)
                    {
                        if (engineObject.transform.parent == null)
                        {
                            DrawNodes(engineObject);
                        }
                    }

                    ImGui.TreePop();
                }
            }

            if (ImGui.IsWindowHovered() && ImGui.IsMouseClicked(ImGuiMouseButton.Right))
            {
                ImGui.OpenPopup("Create Object");
            }
            if (ImGui.BeginPopup("Create Object"))
            {
                ImGui.Text("Create Object");
                if (ImGui.MenuItem("Empty"))
                {
                    EngineObjectFactory.Instantiate("New Object");
                }
                if (ImGui.BeginMenu("3D"))
                {
                    if (ImGui.MenuItem("Cube"))
                    {

                    }
                    if (ImGui.MenuItem("Inverted Cube"))
                    {

                    }
                    if (ImGui.MenuItem("Plane"))
                    {

                    }
                    ImGui.EndMenu();
                }
                ImGui.EndPopup();
            }

            ImGui.End();
        }
        private void DrawNodes(EngineObject engineObject)
        {
            ImGui.PushID(engineObject.objectGuid.ToString());

            ImGuiTreeNodeFlags nodeFlags = engineObject.transform.children.Count > 0 ? ImGuiTreeNodeFlags.DefaultOpen : ImGuiTreeNodeFlags.Leaf;

            nodeFlags |= engineObject == EngineItemSelector.selectedObject ? ImGuiTreeNodeFlags.Selected : ImGuiTreeNodeFlags.None;

            nodeFlags |= ImGuiTreeNodeFlags.OpenOnArrow;
            
            if (ImGui.TreeNodeEx(engineObject.name, nodeFlags))
            {
                if (ImGui.IsItemClicked())
                {
                    EngineItemSelector.selectedObject = engineObject;
                }
                if (ImGui.IsItemHovered() && ImGui.IsMouseDoubleClicked(ImGuiMouseButton.Left))
                {
                    EngineItemSelector.objectToRename = engineObject;
                    renameBuffer = engineObject.name;
                }
                if (engineObject == EngineItemSelector.objectToRename)
                {
                    ImGuiInputTextFlags inputFlags = ImGuiInputTextFlags.EnterReturnsTrue | ImGuiInputTextFlags.AutoSelectAll;
                    ImGui.SetNextItemAllowOverlap();
                    if (ImGui.InputText("##rename", ref renameBuffer, 128, inputFlags))
                    {
                        EngineItemSelector.objectToRename = null;

                        if (renameBuffer != string.Empty)
                            engineObject.name = renameBuffer;
                    }
                }
                foreach (Transform child in engineObject.transform.children)
                {
                    DrawNodes(child.engineObject);
                }
            
            	ImGui.TreePop();
            }

            ImGui.PopID();
        }
    }
}