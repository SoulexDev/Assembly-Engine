using OpenTK.Mathematics;

namespace AssemblyEngine.UI
{
    internal class FillVerticalLayoutProcessor : ILayoutProcessor
    {
        public void ProcessLayout(Vector2 parentPosition, Vector2 parentSize, Group[] groups)
        {
            float split = parentSize.Y / groups.Length;

            for (int i = 0; i < groups.Length; i++)
            {
                Group group = groups[i];

                group.position.X = 0;
                group.position.Y = (int)(i * split);
                group.position += (Vector2i)parentPosition;

                group.size.X = (int)parentSize.X;
                group.size.Y = (int)split;
            }
        }
    }
}
