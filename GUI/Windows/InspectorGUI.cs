using AssemblyEngine.Graphics;
using Hexa.NET.ImGui;
using OpenTK.Mathematics;

namespace AssemblyEngine.GUI
{
    internal class InspectorGUI : GUIWindow
    {
        private string renameBuffer;
        public override void Draw()
        {
            ImGui.Begin("Inspector");

            //check if an item is selected in the hierarchy
            if (EngineItemSelector.selectedObject != null)
            {
                EngineObject obj = EngineItemSelector.selectedObject;

                ImGuiTreeNodeFlags nodeFlags = ImGuiTreeNodeFlags.DefaultOpen | ImGuiTreeNodeFlags.OpenOnArrow;

                //create header for transform component
                if (ImGui.CollapsingHeader("Transform", nodeFlags))
                {
                    //check for right clicks on the component
                    if (ImGui.IsItemClicked(ImGuiMouseButton.Right))
                    {
                        Console.WriteLine("frick youdles");
                    }

                    System.Numerics.Vector3 position = (System.Numerics.Vector3)obj.transform.position;
                    ImGui.DragFloat3("Position", ref position);
                    obj.transform.position = (Vector3)position;
                }

                //iterate through compoenents and draw their headers
                foreach (var component in EngineItemSelector.selectedObject.components)
                {
                    if (ImGui.CollapsingHeader($"{component.name}({component})", nodeFlags))
                    {
                        //check for component rename attempts
                        if (ImGui.IsItemHovered() && ImGui.IsMouseDoubleClicked(ImGuiMouseButton.Left))
                        {
                            EngineItemSelector.componentToRename = component;
                        }
                        if (component == EngineItemSelector.componentToRename)
                        {
                            ImGuiInputTextFlags inputFlags = ImGuiInputTextFlags.EnterReturnsTrue | ImGuiInputTextFlags.AutoSelectAll;
                            ImGui.SetNextItemAllowOverlap();

                            //do rename operations
                            renameBuffer = component.name;
                            if (ImGui.InputText("##rename", ref renameBuffer, 128, inputFlags))
                            {
                                EngineItemSelector.componentToRename = null;

                                if (renameBuffer != string.Empty)
                                    component.name = renameBuffer;
                            }
                        }
                        
                        //draw component custom inspector
                        component.DrawInspector();
                    }
                }

                //check for right clicks in the inspector window
                if (ImGui.IsWindowHovered() && ImGui.IsMouseClicked(ImGuiMouseButton.Right))
                {
                    ImGui.OpenPopup("Add Component");
                }

                //add component menu
                if (ImGui.BeginPopup("Add Component"))
                {
                    if (ImGui.MenuItem("Model Renderer"))
                    {
                        obj.AddComponent<ModelRenderer>("Model Renderer");
                    }
                    ImGui.EndPopup();
                }
            }
            
            ImGui.End();
        }
    }
}