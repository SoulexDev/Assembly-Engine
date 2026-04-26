using OpenTK.Mathematics;

namespace AssemblyEngine.UI
{
    internal interface ILayoutProcessor
    {
        public void ProcessLayout(Vector2 parentPosition, Vector2 parentSize, Group[] groups);
    }
}