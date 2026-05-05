namespace AssemblyEngine
{
    public class EngineObjectFactory
    {
        public static EngineObject Instantiate(string name)
        {
            EngineObject obj = new EngineObject(name);
            ASECore.engineObjects.Add(obj);
            return obj;
        }
    }
}
