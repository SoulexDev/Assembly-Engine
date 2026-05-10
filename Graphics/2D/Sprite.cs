using OpenTK.Mathematics;

namespace AssemblyEngine.Graphics
{
    public enum BillboardType { None = 1, Free = 2, XAxis = 4, YAxis = 6, ZAxis = 8 }
    public sealed class Sprite : Component, IRenderer
    {
        public Texture2D texture;
        public BillboardType billboardType;
        public Vector2 anchor = Vector2.One * 0.5f;
        public int pixelsPerUnit = 16;

        private Vector2 size;
        private Vector3 transformedScale => new Vector3(transform.scale.X * size.X, transform.scale.Y * size.Y, transform.scale.Z);

        private Material _materialInstance;
        public Material material
        {
            get {  return _materialInstance; }
            set
            {
                _materialInstance = value.Clone();
            }
        }
        public Sprite()
        {
            material = ASECore.defaultSpriteMaterial;
        }
        public override void Init()
        {
            RenderPipeline.AddRenderer(this);
        }
        public override void LateUpdate()
        {
            if (billboardType != BillboardType.None)
                transform.rotation = CalculateBillboard();
        }
        public override void OnDestroy()
        {
            RenderPipeline.RemoveRenderer(this);
        }
        public Sprite SetTexture(Texture2D texture)
        {
            this.texture = texture;
            material.AssignTexture("uMainTex", texture);
            size = new Vector2(texture.width, texture.height) / pixelsPerUnit;
            return this;
        }
        public Sprite SetBillboardType(BillboardType billboardType)
        {
            this.billboardType = billboardType;
            return this;
        }
        public Sprite SetAnchor(Vector2 anchor)
        {
            this.anchor = anchor;
            return this;
        }
        public Sprite SetPixelsPerUnit(int pixelsPerUnit)
        {
            this.pixelsPerUnit = pixelsPerUnit;

            if (texture != null)
                size = new Vector2(texture.width, texture.height) / pixelsPerUnit;

            return this;
        }
        public void Draw(Camera camera)
        {
            Matrix4.CreateScale(transformedScale, out Matrix4 modelMatrix);
            modelMatrix *= Matrix4.CreateFromQuaternion(transform.rotation);
            modelMatrix *= Matrix4.CreateTranslation(GetAnchoredPosition());

            material.Use();

            material.shader.SetMatrix4("uModel", modelMatrix);
            material.shader.SetMatrix4("uView", camera.viewMatrix);
            material.shader.SetMatrix4("uProjection", camera.projectionMatrix);

            material.shader.SetVector("uViewPos", camera.transform.position);
            material.shader.SetFloat("uTime", Time.time);

            QuadMesh.mesh.Draw();
        }
        public void Draw(Camera camera, Shader shader)
        {
            Matrix4.CreateScale(transformedScale, out Matrix4 modelMatrix);
            modelMatrix *= Matrix4.CreateFromQuaternion(transform.rotation);
            modelMatrix *= Matrix4.CreateTranslation(GetAnchoredPosition());

            shader.Use();

            shader.SetMatrix4("uModel", modelMatrix);
            shader.SetMatrix4("uView", camera.viewMatrix);
            shader.SetMatrix4("uProjection", camera.projectionMatrix);

            shader.SetVector("uViewPos", camera.transform.position);
            shader.SetFloat("uTime", Time.time);

            QuadMesh.mesh.Draw();
        }
        private Vector3 GetAnchoredPosition()
        {
            Vector2 convertedAnchor = anchor * 2 - Vector2.One;
            Vector3 scale = transformedScale;
            return transform.position - new Vector3(scale.X * convertedAnchor.X, scale.Y * convertedAnchor.Y, 0) * 0.5f;
        }
        private Quaternion CalculateBillboard()
        {
            Vector3 normal = (Camera.main.transform.position - transform.position).Normalized();

            normal = billboardType switch
            {
                BillboardType.Free => normal,
                BillboardType.XAxis => new Vector3(0, normal.Y, normal.Z).Normalized(),
                BillboardType.YAxis => new Vector3(normal.X, 0, normal.Z).Normalized(),
                BillboardType.ZAxis => new Vector3(normal.X, normal.Y, 0).Normalized(),
                _ => normal
            };

            //Vector3 right = Vector3.Cross(normal, Vector3.UnitY).Normalized();
            //right.NormalizeFast();

            //Vector3 up = Vector3.Cross(normal, right).Normalized();
            //up.NormalizeFast();

            return Quaternion.FromMatrix(new Matrix3(Matrix4.LookAt(Vector3.Zero, normal, Vector3.UnitY)));

            //return Quaternion.FromMatrix(new Matrix3(
            //    normal.X, up.X, right.X,
            //    normal.Y, up.Y, right.Y,
            //    normal.Z, up.Z, right.Z));

            //Vector3 right = Vector3.Cross(forward, Vector3.UnitY).Normalized();
            //float w = (float)MathHelper.Sqrt(forward.LengthSquared) + Vector3.Dot(forward, Vector3.UnitY);
            //return new Quaternion(right, w);
        }
    }
}
