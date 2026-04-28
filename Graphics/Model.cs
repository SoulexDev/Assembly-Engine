using AssemblyEngine.Physics;

namespace AssemblyEngine.Graphics
{
    public class Model
    {
        public List<(Mesh, Material)> meshes = new List<(Mesh, Material)>();
        public AABB boundingBox;

        public Model() { }
        public Model(params (Mesh, Material)[] meshes)
        {
            this.meshes.AddRange(meshes);
        }
    }
}
