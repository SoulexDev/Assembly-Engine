using Hexa.NET.ImGui;
using OpenTK.Mathematics;

namespace AssemblyEngine.GUI
{
    internal class InspectorGUI : GUIWindow
    {
        public override void Draw()
        {
            ImGui.Begin("Inspector");
            if (EngineItemSelector.selectedObject != null)
            {
                EngineObject obj = EngineItemSelector.selectedObject;

                //int componentIndex = 0;

                if (ImGui.CollapsingHeader("Transform", ImGuiTreeNodeFlags.OpenOnArrow))
                {
                    if (ImGui.IsItemClicked(ImGuiMouseButton.Right))
                    {
                        Console.WriteLine("frick youdles");
                    }
                    System.Numerics.Vector3 position = (System.Numerics.Vector3)obj.transform.position;
                    ImGui.DragFloat3("Position: ", ref position);
                    obj.transform.position = (Vector3)position;
                }

                foreach (var component in EngineItemSelector.selectedObject.components)
                {
                    if (ImGui.CollapsingHeader(component.name))
                    {
                        component.DrawInspector();
                        //draw component specific stuff here
                    }
                    //if (ImGui.BeginChild((componentIndex++).ToString(), ImGuiChildFlags.Borders, ImGuiWindowFlags.MenuBar))
                    //{
                    //    DrawComponentWindow(component.name);
                        
                    //    ImGui.EndChild();
                    //}
                }
            }
            ImGui.End();
        }
    }
}