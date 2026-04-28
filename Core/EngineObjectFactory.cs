namespace AssemblyEngine
{
    public class EngineObjectFactory
    {
        public static EngineObject Instantiate()
        {
            EngineObject obj = new EngineObject();
            ASECore.engineObjects.Add(obj);
            return obj;
        }
    }
}
