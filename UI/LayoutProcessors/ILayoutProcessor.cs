using OpenTK.Mathematics;

namespace AssemblyEngine.UI
{
    internal interface ILayoutProcessor
    {
        public (Vector2, Vector2)[] ProcessLayout(Vector2 parentPosition, Vector2 parentSize, Group[] groups);
    }
}