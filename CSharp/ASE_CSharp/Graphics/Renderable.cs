using System;
using System.Collections.Generic;
using System.Text;

namespace ASE.Graphics
{
    public struct Renderable
    {
        private Transform transform;
        public Mesh mesh;

        public Renderable(Transform transform, Mesh mesh)
        {
            this.transform = transform;
            this.mesh = mesh;
        }
        public Renderable(Transform transform, Mesh mesh, Material material)
        {
            this.transform = transform;
            this.mesh = mesh;
        }
    }
}
