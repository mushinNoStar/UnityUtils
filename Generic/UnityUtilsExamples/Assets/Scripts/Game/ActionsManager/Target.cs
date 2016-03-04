using System.Collections.Generic;
using System;

namespace Actions
{
    public class Target
    {
        private static Dictionary<string, Type> targetTypes = null;
        private static Dictionary<int, Target> existingTargets = new Dictionary<int, Target>();
        private static int nextId = 0;

        private int myID = -1;
        private List<Action> allowedActions = new List<Action>();
        private List<Actor> forbiddenActors = new List<Actor>();
        private List<Action> forbiddenActions = new List<Action>();

        public Target()
        {
            existingTargets.Add(nextId, this);
            myID = nextId;
            nextId++;
            allowedActions = Action.getDefaultAllowedActions(this);
        }

        /// <summary>
        /// return if this particular instance can be targeted by an action.
        /// </summary>
        /// <param name="actionName"></param>
        /// <returns></returns>
        public bool canBeInteractedBy(string actionName)
        {
            Action act = Action.getAction(actionName);
            if (allowedActions.Contains(act) && act.canBeAppliedAt(this.GetType()))
                return true;
            return false;
        }

        /// <summary>
        /// return if this particular instance can be 
        /// targeted by a particular actor with a particular action
        /// </summary>
        /// <param name="action"></param>
        /// <param name="actor"></param>
        /// <returns></returns>
        public bool canBeInteractedBy(Action action, Actor actor)
        {
            if (!canBeInteractedBy(action.getName()))
                return false;
            if (forbiddenActions.Contains(action))
            {
                if (forbiddenActors[forbiddenActions.IndexOf(action)] == actor)
                    return false;
            }
            return true;
        }

        /// <summary>
        /// set a particular actor and a particular action as forbidden to call that action on this target.
        /// </summary>
        /// <param name="act"></param>
        /// <param name="action"></param>
        public void blockActor(Actor act, Action action)
        {
            forbiddenActions.Add(action);
            forbiddenActors.Add(act);
        }

        /// <summary>
        /// remove from the black list one particular action and a actor.
        /// </summary>
        /// <param name="act"></param>
        /// <param name="action"></param>
        public void removeBlockActor(Actor act, Action action)
        {
            for (int a = 0; a < forbiddenActors.Count; a++)
            {
                if (forbiddenActors[a] == act && forbiddenActions[a] == action)
                {
                    forbiddenActions.RemoveAt(a);
                    forbiddenActors.RemoveAt(a);

                    return;
                }
            }

        }

        /// <summary>
        /// return the unique id of this target.
        /// </summary>
        /// <returns></returns>
        public int getId()
        {
            return myID;
        }

        public static Target getTargetByID(int id)
        {
            return existingTargets[id];
        }

        private static void loadTypes()
        {
            targetTypes = new Dictionary<string, Type>();
            List<Type> listOfType = new List<Type>();
            foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var type in asm.GetTypes())
                {
                    if (type.BaseType == typeof(Target))
                        listOfType.Add(type);
                }
            }
            foreach (Type t in listOfType)
            {
                if (t.Name == typeof(Target).Name)
                    continue;
                targetTypes.Add(t.Name, t);
            }
        }
        /// <summary>
        /// add one particular action in to the list of actions that can interact with this target
        /// </summary>
        /// <param name="act"></param>
        public void addEnabledAction(Action act)
        {
            allowedActions.Add(act);
        }
        /// <summary>
        /// remove a particular action from the list of action that can interact with this target.
        /// </summary>
        /// <param name="act"></param>
        public void removeEnabledAction(Action act)
        {
            allowedActions.Remove(act);
        }
    }
}
