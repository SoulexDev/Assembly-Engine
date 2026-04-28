namespace AssemblyEngine
{
    public sealed class EngineObject
    {
        public Transform transform;
        internal List<Component> components = new List<Component>();

        internal EngineObject()
        {
            transform = new Transform();
        }
        public T AddComponent<T>(string name) where T : Component
        {
            T component = (T)Activator.CreateInstance(typeof(T));

            component.name = name;
            component.engineObject = this;

            components.Add(component);

            component.Init();

            return component;
        }
        public void RemoveComponent<T>(string name) where T :Component
        {
            Component c = components.Find(c => c.name == name && c.GetType() == typeof(T));
            if (c != null)
            {
                components.Remove(c);
            }
        }
    }
}
