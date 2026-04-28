//using System.Collections;

//namespace AssemblyEngine.ECS
//{
//    using Entity = uint;
//    using ComponentType = ushort;
//    using Signature = BitArray;
    
//    public class ECSManager
//    {
//        private const uint MaxEntities = 4;
//        private const ComponentType MaxComponents = 4;

//        private static Queue<Entity> entities = new Queue<Entity>();
//        private static Signature[] signatures = new Signature[MaxEntities];
//        private static List<IComponent> components = new List<IComponent>();

//        private static int liveEntityCount;

//        public static void Init()
//        {
//            for (Entity e = 0; e < MaxEntities; e++)
//            {
//                entities.Enqueue(e);
//                signatures[e] = new Signature(MaxComponents);
//            }

//            CreateEntity();
//            CreateEntity();
//            CreateEntity();

//            SetSignature(0, new Signature(new bool[] { true, false, false, false }));
//            SetSignature(1, new Signature(new bool[] { true, false, true, true }));
//            SetSignature(2, new Signature(new bool[] { true, true, false, false }));

//            PrintData();
//        }
//        public static Entity CreateEntity()
//        {
//            if (entities.Count >= MaxEntities)
//                return (Entity)entities.Count;

//            Entity id = entities.Dequeue();
//            ++liveEntityCount;

//            //PrintData();

//            return id;
//        }
//        public static void DestroyEntity(Entity entity)
//        {
//            if (entity > MaxEntities)
//            {
//                Console.WriteLine($"Entity: [{entity}] is out of range. It cannot be destroyed");
//                return;
//            }

//            signatures[entity].SetAll(false);
//            entities.Enqueue(entity);
//            --liveEntityCount;

//            //PrintData();
//        }
//        public static void SetSignature(Entity entity, Signature signature)
//        {
//            if (entity > MaxEntities)
//            {
//                Console.WriteLine($"Entity: [{entity}] is out of range. It does not have a signature");
//                return;
//            }

//            signatures[entity] = signature;
//        }
//        public Signature GetSignature(Entity entity)
//        {
//            if (entity > MaxEntities)
//            {
//                Console.WriteLine($"Entity: [{entity}] is out of range. It cannot have a signature");
//                return null;
//            }

//            return signatures[entity];
//        }
//        public static void AddComponentToEntity(Entity entity, ComponentType type)
//        {
//            signatures[entity].Set(type, true);
//        }
//        public static bool TryGetComponentFromEntity<T>(Entity entity, ComponentType type, out T component) where T : IComponent
//        {
//            if (!signatures[entity].Get(type))
//            {
//                component = default;
//                return false;
//            }
//            component = default;
//            return true;
//        }
//        private static void PrintData()
//        {
//            Entity[] ents = entities.ToArray();
//            for (int i = 0; i < ents.Length; i++)
//            {
//                Console.WriteLine($"Entity {ents[i]} has signature: {signatures[i]}");
//            }
//        }
//    }
//}