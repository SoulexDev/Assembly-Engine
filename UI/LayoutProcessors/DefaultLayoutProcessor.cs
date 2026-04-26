using OpenTK.Mathematics;

namespace AssemblyEngine.UI
{
    internal class DefaultLayoutProcessor : ILayoutProcessor
    {
        public void ProcessLayout(Vector2 parentPosition, Vector2 parentSize, Group[] groups)
        {
            for (int i = 0; i < groups.Length; i++)
            {
                Group group = groups[i];
                group.position = (Vector2i)group.internalPosition;
                group.size = (Vector2i)group.internalSize;
            }
        }
    }
}
