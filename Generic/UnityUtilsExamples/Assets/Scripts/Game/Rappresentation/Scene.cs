using System.Collections.Generic;
using System;

namespace Game
{
    /// <summary>
    /// a scene is a class that determine what the screen will show in a particular situation.
    /// this should not determine which kind of information are aviable to the player, that 
    /// belongs to the game rules. instead it should just allow to see such information.
    /// </summary>
    public abstract class Scene
    {
        private static Dictionary<string, Scene> allScenes = null;

        /// <summary>
        /// called every game tick.
        /// </summary>
        public abstract void tick();
        /// <summary>
        /// called when a scene is no longer the one visible.
        /// </summary>
        public abstract void OnEnd();
        /// <summary>
        /// called when a scene is the one that will be seen.
        /// </summary>
        public abstract void OnStart();

        protected Scene()
        {
            allScenes.Add(GetType().Name, this);
        }

        /// <summary>
        /// scenes cannot be instantiaded, they must be taken from here.
        /// </summary>
        /// <param name="sceneName"></param>
        /// <returns></returns>
        public static Scene getScene(string sceneName)
        {
            if (allScenes == null)
                loadScenes();
            return allScenes[sceneName];
        }

        /// <summary>
        /// looks in the assemblies and load one of each scene class.
        /// </summary>
        private static void loadScenes()
        {

            allScenes = new Dictionary<string, Scene>();
            List<Type> listOfType = new List<Type>();
            foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var type in asm.GetTypes())
                {
                    if (type.BaseType == typeof(Scene))
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