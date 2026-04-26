using OpenTK.Mathematics;

namespace AssemblyEngine.UI
{
    internal class FillHorizontalLayoutProcessor : ILayoutProcessor
    {
        public void ProcessLayout(Vector2 parentPosition, Vector2 parentSize, Group[] groups)
        {
            float split = parentSize.X / groups.Length;

            for (int i = 0; i < groups.Length; i++)
            {
                Group group = groups[i];

                group.position.X = (int)(i * split);
                group.position.Y = 0;
                group.position += (Vector2i)parentPosition;

                group.size.X = (int)split;
                group.size.Y = (int)parentSize.Y;
            }
        }
    }
}
