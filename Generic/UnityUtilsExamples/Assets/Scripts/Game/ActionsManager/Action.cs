using System.Collections.Generic;
using System;

namespace Actions
{
    public abstract class Action
    {
        private static Dictionary<string, Action> allActions = null;

        private string actionName = "BaseAction";
        private List<Type> possibleCallers = new List<Type>();
        private List<Type> allowedTargets = new List<Type>();

        /// <summary>
        /// this is the list of requirements that caller and target must shatisfy in order to be executed
        /// </summary>
        protected List<Requirement> requirements = new List<Requirement>();
        /// <summary>
        /// if a action is enable by default, every actor instantiated in the list of allowed types
        /// will include this as allowed action.
        /// </summary>
        protected bool enabledByDefault = false;
        /// <summary>
        /// if target in the possible target list should be allowed by default, set this to true.
        /// </summary>
        protected bool allowedByDefault = true;

        /// <summary>
        /// return true if the type of the actor is one of those that can call this action.
        /// </summary>
        /// <param name="actorType"></param>
        /// <returns></returns>
        public bool canBeCalledBy(Type actorType)
        {
            bool foundOne = false;
            foreach (Type t in possibleCallers)
            {
                if (actorType == t)
                {
                    foundOne = true;
                    break;
                }
            }

            if (!foundOne)
                return false;

            return true;
        }

        /// <summary>
        /// returns the list of requirements that are not shatisfied by a particular activation settings
        /// </summary>
        /// <param name="caller"></param>
        /// <param name="targets"></param>
        /// <param name="otherArgs"></param>
        /// <returns></returns>
        public List<Requirement> getNotShatishiedRequirements(Actor caller, List<Target> targets, List<string> otherArgs)
        {
            List<Requirement> diRitorno = new List<Requirement>();
            foreach (Requirement rq in requirements)
                if (!rq.isShatishied(caller, targets, otherArgs))
                    diRitorno.Add(rq);
            return diRitorno;
        }

        /// <summary>
        /// add a type to the list of the possible callers.
        /// it does not influence already instantiated callers.
        /// </summary>
        /// <param name="t"></param>
        public void addPossibleCaller(Type t)
        {
            if (t.IsSubclassOf(typeof(Actor)))
                possibleCallers.Add(t);
        }

        /// <summary>
        /// remove a type from the list of possible callers.
        /// it does not influence already instantiated callers.
        /// </summary>
        /// <param name="t"></param>
        public void removePossibleCaller(Type t)
        {
            if (t.IsSubclassOf(typeof(Actor)))
                possibleCallers.Remove(t);
        }

        /// <summary>
        /// remove a possible target from the possible target list
        /// does not influence already instantiatied targets.
        /// </summary>
        /// <param name="t"></param>
        public void removePossibleTarget(Type t)
        {
            if (t.IsSubclassOf(typeof(Target)))
                allowedTargets.Remove(t);
        }

        /// <summary>
        /// add a possible target from the possible target list
        /// does not influence already instantiatied targets.
        /// </summary>
        /// <param name="t"></param>
        public void addPossibleTarget(Type t)
        {
            if (t.IsSubclassOf(typeof(Target)))
                allowedTargets.Add(t);
        }

        /// <summary>
        /// return if the target type is one of the one that can be taken as target of this action.
        /// </summary>
        /// <param name="targetType"></param>
        /// <returns></returns>
        public bool canBeAppliedAt(Type targetType)
        {
            bool foundOne = false;
            foreach (Type t in allowedTargets)
            {
                if (targetType == t)
                {
                    foundOne = true;
                    break;
                }
            }
            if (!foundOne)
                return false;
            //throw new ArgumentException(actor.getType().Name +" cannot call "+getName()+" action")
            return true;
        }

        /// <summary>
        /// check if every possible condition is shatisfied.
        /// </summary>
        /// <param name="caller"></param>
        /// <param name="targets"></param>
        /// <param name="otherArgs"></param>
        /// <returns></returns>
        public bool everythingShatishied(Actor caller, List<Target> targets, List<string> otherArgs)
        {
            if (!caller.canCallAction(actionName))
                return false;

            foreach (Target t in targets)
                if (!t.canBeInteractedBy(this, caller))
                    return false;
            
            if (getNotShatishiedRequirements(caller, targets, otherArgs).Count != 0)
                return false;

            return true;
        }

        /// <summary>
        /// effetuate the action. if it cannot be called for any reason, it will not be executed.
        /// </summary>
        /// <param name="caller"></param>
        /// <param name="targets"></param>
        /// <param name="otherArgs"></param>
        public void call(Actor caller, List<Target> targets, List<string> otherArgs)
        {
            if (everythingShatishied(caller, targets, otherArgs))
                apply(caller, targets, otherArgs);
        }

        protected Action()
        {
            Action.allActions.Add(actionName, this);
            actionName = GetType().Name;
        }

        /// <summary>
        /// return the action from his name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Action getAction(string name)
        {
            if (Action.allActions == null)
                Action.loadActions();
            return Action.allActions[name];
        }

        private static void loadActions()
        {
            allActions = new Dictionary<string, Action>();
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

        /// <summary>
        /// return the name of this particular action.
        /// </summary>
        /// <returns></returns>
        public string getName()
        {
            return actionName;
        }

        /// <summary>
        /// you must implement this, when this is called requirements are alaready checked.
        /// </summary>
        /// <param name="caller"></param>
        /// <param name="targets"></param>
        /// <param name="otherArgs"></param>
        protected abstract void apply(Actor caller, List<Target> targets, List<string> otherArgs);

        /// <summary>
        /// return the list of actions that by default can be exectuted by an actor
        /// </summary>
        /// <param name="actor"></param>
        /// <returns></returns>
        public static List<Action> getDefaultEnabledActions(Actor actor)
        {
            List<Action> diRitorno = new List<Action>();
            if (Action.allActions == null)
                Action.loadActions();
            foreach (Action action in allActions.Values)
            {
                if (action.canBeCalledBy(actor.GetType()) && action.enabledByDefault)
                    diRitorno.Add(action);
            }

            return diRitorno;
        }

        /// <summary>
        /// return the list of actions that by default can be applied to a target
        /// </summary>
        /// <param name="actor"></param>
        /// <returns></returns>
        public static List<Action> getDefaultAllowedActions(Target target)
        {
            if (allActions == null)
                loadActions();
            List<Action> diRitorno = new List<Action>();
            if (Action.allActions == null)
                Action.loadActions();

            foreach (Action action in allActions.Values)
            {
                if (action.canBeAppliedAt(target.GetType()) && action.allowedByDefault)
                    diRitorno.Add(action);
            }

            return diRitorno;
        }
    }
}