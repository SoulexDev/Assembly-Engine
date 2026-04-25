using OpenTK.Mathematics;

namespace AssemblyEngine.UI
{
    public class CenterHorizontalLayoutProcessor : ILayoutProcessor
    {
        public (Vector2, Vector2)[] ProcessLayout(Vector2 parentPosition, Vector2 parentSize, Group[] groups)
        {
            //positions, size
            (Vector2, Vector2)[] result = new (Vector2, Vector2)[groups.Length];

            float positionX = 0;
            for (int i = 0; i < groups.Length; i++)
            {
                //cache group
                Group group = groups[i];

                //cache position
                Vector2 positon = result[i].Item1;

                //cache size
                Vector2 size = result[i].Item2;

                //position
                positon.X = positionX;
                positon.Y = -group.size.y * 0.5f;
                positon += parentPosition + parentSize * 0.5f;

                //size
                size.X = group.size.x;
                size.Y = group.size.y;

                positionX += group.size.x;

                result[i].Item1 = positon;
                result[i].Item2 = size;
            }
            for (int i = 0; i < groups.Length; i++)
            {
                result[i].Item1.X -= positionX * 0.5f;
            }
            return result;
        }
    }
}