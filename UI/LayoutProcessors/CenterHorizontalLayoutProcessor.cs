using OpenTK.Mathematics;

namespace AssemblyEngine.UI
{
    public class CenterHorizontalLayoutProcessor : ILayoutProcessor
    {
        public void ProcessLayout(Vector2 parentPosition, Vector2 parentSize, Group[] groups)
        {
            float positionX = 0;
            for (int i = 0; i < groups.Length; i++)
            {
                //cache group
                Group group = groups[i];

                //position
                group.position.X = (int)positionX;
                group.position.Y = (int)(-group.internalSize.y * 0.5f);
                group.position += (Vector2i)(parentPosition + parentSize * 0.5f);

                group.size = (Vector2i)group.internalSize;

                positionX += group.internalSize.x;
            }
            for (int i = 0; i < groups.Length; i++)
            {
                groups[i].position.X -= (int)(positionX * 0.5f);
            }
        }
    }
}