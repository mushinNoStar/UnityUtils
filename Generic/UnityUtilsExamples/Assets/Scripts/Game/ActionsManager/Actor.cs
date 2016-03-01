using System.Collections.Generic;
using System;
using Networking;

namespace Actions
{
    public class Actor : Syncronizable
    {
        private static Dictionary<string, Type> actorType = null;
        private static Dictionary<int, Actor> existingActors = new Dictionary<int, Actor>();
        private static int nextId = 0;

        private List<Action> enabledActions = new List<Action>();
        private int myID = -1;

        public Actor(List<string> data, int id) : base(data, id)
        {
            existingActors.Add(nextId, this);
        }

        public Actor() : base()
        {
            existingActors.Add(nextId, this);
            myID = nextId;
            nextId++;
            enabledActions = Action.getDefaultEnabledActions(this);
        }

        public override List<string> serialize()
        {
            List<string> diRitorno = base.serialize();
            diRitorno.Add(myID+"");
            foreach (Action act in enabledActions)
                diRitorno.Add(act.getName()+"");
            diRitorno.Add("#");
            return diRitorno;
        }

        public override void deserialize(List<string> data)
        {
            base.deserialize(data);
            myID = int.Parse(data[0]);
            data.RemoveAt(0);
            while (data[0] != "#")
            {
                enabledActions.Add(Action.getAction(data[0]));
                data.RemoveAt(0);
            }
            data.RemoveAt(0);
        }

        public int getId()
        {
            return myID;
        }

        /// <summary>
        /// return the actor associated to an id.
        /// if the key is missing, it returns null
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Actor getActorByID(int id)
        {
            if (existingActors.ContainsKey(id))
                return existingActors[id];
            return null;
        }

        /// <summary>
        /// return if this particular istance can call a particular action.
        /// if return false if the action is disable for this type too.
        /// </summary>
        /// <param name="actionName"></param>
        /// <returns></returns>
        public bool canCallAction(string actionName)
        {
            Action act = Action.getAction(actionName);
            if (enabledActions.Contains(act) && act.canBeCalledBy(this.GetType()))
                return true;
            return false;
        }

        /// <summary>
        /// return a type of actor from his type name
        /// </summary>
        /// <param name="typeName"></param>
        /// <returns></returns>
        public static Type getActorType(string typeName)
        {
            if (actorType == null)
                loadTypes();
            return actorType[typeName];
        }

        private static void loadTypes()
        {
            actorType = new Dictionary<string, Type>();
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
                if (t.Name == typeof(Actor).Name)
                    continue;
                actorType.Add(t.Name, t);
            }



        }

        /// <summary>
        /// add a action to the one that can be called.
        /// </summary>
        /// <param name="act"></param>
        public void addEnabledAction(Action act)
        {
            enabledActions.Add(act);
            changed();

        }

        /// <summary>
        /// remove a action from those that can be called.
        /// </summary>
        /// <param name="act"></param>
        public void removeEnabledAction(Action act)
        {
            enabledActions.Remove(act);
            changed();

        }
    }
}
