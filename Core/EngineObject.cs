namespace AssemblyEngine
{
    public sealed class EngineObject
    {
        public Transform transform;
        internal List<Component> components = new List<Component>();

        internal event Action<Component> componentAdded;
        internal event Action<Component> componentRemoved;

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

            componentAdded?.Invoke(component);

            componentAdded += component.OnComponentAdded;
            componentRemoved += component.OnComponentRemoved;

            return component;
        }
        public T GetComponent<T>(string name) where T : Component
        {
            Component component = components.Find(c => c.name == name);
            if (component != null)
                return component as T;
            else
                return null;
        }
        public List<Component> GetComponentsOfType<T>() where T : Component
        {
            return components.FindAll(c => c.GetType().BaseType == typeof(T));
        }
        public bool HasComponentOfType<T>() where T : Component
        {
            return components.Exists(c => c.GetType() == typeof(T));
        }
        public void RemoveComponent<T>(string name) where T :Component
        {
            Component c = components.Find(c => c.name == name && c.GetType() == typeof(T));
            if (c != null)
            {
                components.Remove(c);
                componentAdded -= c.OnComponentAdded;
                componentRemoved -= c.OnComponentRemoved;
            }
        }
    }
}
