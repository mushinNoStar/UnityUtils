using System.Collections.Generic;
using System;

namespace Game
{
    public abstract class Scene
    {
        private static Dictionary<string, Scene> allScenes = new Dictionary<string, Scene>();

        public abstract void tick();
        public abstract void OnEnd();
        public abstract void OnStart();

        private Scene()
        {
            allScenes.Add(GetType().Name, this);
        }

        public static Scene getScene(string sceneName)
        {
            return allScenes[sceneName];
        }

        private static void loadScenes()
        {
            allScenes = new Dictionary<string, Scene>();
            List<Type> listOfType = new List<Type>();
            foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var type in asm.GetTypes())
                {
                    if (type.BaseType == typeof(Action))
                        listOfType.Add(type);
                }
            }
            foreach (Type t in listOfType)
            {
                if (t.IsAbstract)
                    continue;
                var ctors = t.GetConstructors();
                ctors[0].Invoke(new object[] { });
            }
        }
    }
}