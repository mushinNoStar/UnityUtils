using System.Collections.Generic;

namespace Actions
{
    public class ActionManager
    {
        
        /// <summary>
        /// apply the rule, if it cannot be called, it's dropped.
        /// </summary>
        /// <param name="RuleName"></param>
        /// <param name="callerId"></param>
        /// <param name="targetsId"></param>
        /// <param name="otherParam"></param>
        public static void callRule(string RuleName, int callerId, List<int> targetsId, List<string> otherParam)
        {
            //if (Server.getServer().isServer()) //if i'm the server, bypass every controll.
            executeRule(RuleName, callerId, targetsId, otherParam);
            //else //else send this to the server.
              //  Server.getServer().sendActionRequest(RuleName, callerId, targetsId, otherParam);
        }

        /// <summary>
        /// apply the rule, if it cannot be called, it's dropped
        /// </summary>
        /// <param name="action"></param>
        /// <param name="actor"></param>
        /// <param name="targets"></param>
        /// <param name="otherParm"></param>
        public static void callRule(Action action, Actor actor, List<Target> targets, List<string> otherParm)
        {
            string actName = action.getName();
            int id = actor.getId();
            List<int> list = new List<int>();
            foreach (Target t in targets)
                list.Add(t.getId());
            callRule(actName, id, list, otherParm);
        }

        /// <summary>
        /// return if the rule can be called with some particular settings.
        /// </summary>
        /// <param name="action"></param>
        /// <param name="actor"></param>
        /// <param name="targets"></param>
        /// <param name="otherParm"></param>
        /// <returns></returns>
        public static bool canBeCalled(Action action, Actor actor, List<Target> targets, List<string> otherParm)
        {
            return (action.everythingShatishied(actor, targets, otherParm));
        }

        /// <summary>
        /// return the list of things that prevent the rule activation.
        /// </summary>
        /// <param name="action"></param>
        /// <param name="actor"></param>
        /// <param name="targets"></param>
        /// <param name="otherParm"></param>
        /// <returns></returns>
        public List<string> getPreventCondition(Action action, Actor actor, List<Target> targets, List<string> otherParm)
        {
            List<string> diRitorno = new List<string>();
            if (action.canBeCalledBy(actor.GetType()))
            {
                diRitorno.Add(action.getName()+" cannot be called by "+actor.GetType().Name);
                return diRitorno;
            }
            if (actor.canCallAction(action.getName()))
            {
                diRitorno.Add(actor.getId() + " cannot call rule "+ action.getName());
                return diRitorno;
            }

            foreach (Requirement rq in action.getNotShatishiedRequirements(actor, targets, otherParm))
                diRitorno.Add(rq.getMessage());
            return diRitorno;

        }

        public static void executeRule(string RuleName, int callerId, List<int> targetsId, List<string> otherParam)
        {
            Actor act = Actor.getActorByID(callerId);
            if (act == null)
                throw new System.ArgumentException("Action caller cannot be null");

            Action action = Action.getAction(RuleName);
            if (action == null)
                throw new System.ArgumentException("Cannot call null rule");

            List<Target> targetList = new List<Target>();
            foreach (int id in targetsId)
            {
                Target t = Target.getTargetByID(id);
                if (t != null)
                    targetList.Add(t);
            }

            if (!action.everythingShatishied(act, targetList, otherParam))
                throw new System.ArgumentException("some arguments are incorrect");

            action.call(act, targetList, otherParam);
        }
    }
}