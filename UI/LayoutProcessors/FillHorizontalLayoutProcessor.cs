using OpenTK.Mathematics;

namespace AssemblyEngine.UI
{
    internal class FillHorizontalLayoutProcessor : ILayoutProcessor
    {
        public (Vector2, Vector2)[] ProcessLayout(Vector2 parentPosition, Vector2 parentSize, Group[] groups)
        {
            (Vector2, Vector2)[] result = new (Vector2, Vector2)[groups.Length];

            float split = parentSize.X / groups.Length;

            for (int i = 0; i < groups.Length; i++)
            {
                (Vector2, Vector2) rect = result[i];

                rect.Item1.X = i * split;
                rect.Item1.Y = 0;
                rect.Item1 += parentPosition;

                rect.Item2.X = split;
                rect.Item2.Y = parentSize.Y;

                result[i] = rect;
            }
            return result;
        }
    }
}
