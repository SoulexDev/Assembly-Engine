using Hexa.NET.ImGui;

namespace AssemblyEngine.GUI
{
    internal class HierarchyGUI : GUIWindow
    {
        public override void Draw()
        {
            ImGui.Begin("Hierarchy");
            ImGui.Separator();

            foreach (var engineObject in ASECore.engineObjects)
            {
                if (engineObject.transform.parent == null)
                    DrawNodes(engineObject);
            }

            ImGui.End();
        }
        private void DrawNodes(EngineObject engineObject)
        {
            ImGuiTreeNodeFlags nodeFlags = engineObject.transform.children.Count > 0 ? ImGuiTreeNodeFlags.DefaultOpen : ImGuiTreeNodeFlags.Leaf;

            nodeFlags |= engineObject == EngineItemSelector.selectedObject ? ImGuiTreeNodeFlags.Selected : ImGuiTreeNodeFlags.None;

            nodeFlags |= ImGuiTreeNodeFlags.OpenOnArrow;

            if (ImGui.TreeNodeEx(engineObject.name, nodeFlags))
            {
                if (ImGui.IsItemClicked())
                {
                    EngineItemSelector.selectedObject = engineObject;
                }
                foreach (Transform child in engineObject.transform.children)
                {
                    DrawNodes(child.engineObject);
                }
            
            	ImGui.TreePop();
            }
        }
    }
}