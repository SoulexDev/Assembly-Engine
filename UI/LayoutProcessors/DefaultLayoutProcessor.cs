using OpenTK.Mathematics;

namespace AssemblyEngine.UI
{
    internal class DefaultLayoutProcessor : ILayoutProcessor
    {
        public (Vector2, Vector2)[] ProcessLayout(Vector2 parentPosition, Vector2 parentSize, Group[] groups)
        {
            (Vector2, Vector2)[] result = new (Vector2, Vector2)[groups.Length];
            for (int i = 0; i < groups.Length; i++)
            {
                Group group = groups[i];
                result[i].Item1 = (Vector2)group.position;
                result[i].Item2 = (Vector2)group.size;
            }
            return result;
        }
    }
}
