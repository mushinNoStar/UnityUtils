using System.Collections.Generic;
using System;

namespace Networking
{
    public delegate void syncChanged(Syncronizable sinc);
    public abstract class Syncronizable
    {
        private static Dictionary<string, Type> allType = null;
        private static Dictionary<int, Syncronizable> everySyncornizable = new Dictionary<int, Syncronizable>();
        private static int nextID = 0;
        public event syncChanged OnThisChanged;
        private int syncID;

        private static List<List<string>> pendingData = new List<List<string>>();

        protected Syncronizable(List<string> data, int id)
        {
            try {
                deserialize(data);
            }catch (KeyNotFoundException)
            {
                data.Insert(0, id+"");
                data.Insert(0, GetType().Name);
                pendingData.Add(data);

            }
            syncID = id;
            everySyncornizable.Add(id, this);
        }

        protected Syncronizable()
        {
            if (!Server.getServer().isServer())
                throw new System.Exception("not server instances can't create game object out thin air");
            everySyncornizable.Add(nextID, this);
            nextID++;
        }

        public virtual void deserialize(List<string> data)
        {

        }

        public virtual List<string> serialize()
        {
            return new List<string>();
        }

        public int getSyncronizationID()
        {
            return syncID;
        }

        public static Syncronizable getSyncronizable(int syncronizationID)
        {
            if (!everySyncornizable.ContainsKey(syncronizationID))
                return null;
            return everySyncornizable[syncronizationID];
        }

        protected void changed()
        {
            if (OnThisChanged != null)
                OnThisChanged(this);
            if (Server.getServer().isServer())
                Server.getServer().sendSincronizableUpdate(getSyncronizationID(), serialize(), GetType().Name);
        }


        private static Type getTypeFromName(string typeName)
        {
            return allType[typeName];
        }

        private static void loadTypes()
        {
            allType = new Dictionary<string, Type>();
            foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var type in asm.GetTypes())
                {
                    if (type.IsSubclassOf(typeof(Syncronizable)) && !type.IsAbstract)
                        allType.Add(type.Name, type);
                }
            }
        }

        public static void createNewSync(int syncId, List<string> serialized, string typeName)
        {
            if (allType == null)
                loadTypes();

            Type t = allType[typeName];
            var ctors = t.GetConstructors();

            ctors[0].Invoke(new object[] {serialized, syncId});
        }

        public static void updateAll()
        {
            foreach (Syncronizable sinc in everySyncornizable.Values)
                sinc.changed();
        }

        public void tryInstantiatePending()
        {
            for (int a = pendingData.Count; a > 0; a--)
            {
                List<string> data = pendingData[a];
                string type = data[0];
                data.RemoveAt(0);
                int id = int.Parse(data[0]);

                pendingData.RemoveAt(a);

                if (getSyncronizable(id) != null)
                    return;
                createNewSync(id,data,type);
            }
        }
    }
}
