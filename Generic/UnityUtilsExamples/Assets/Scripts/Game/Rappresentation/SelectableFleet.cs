using System;
using Vision;
using Actions;
using System.Collections.Generic;

namespace Game
{
    public class SelectableFleet : ISelectable
    {
        public readonly Fleet targetingFleet;

        public SelectableFleet(Fleet fleet)
        {
            targetingFleet = fleet;
        }

        public void OnSelectEnd()
        {
            //throw new NotImplementedException();
        }

        public void OnSelectStart()
        {
           // throw new NotImplementedException();
        }

        public void reciveTarget(object ogg)
        {
    
            if (ogg is SubSector)
            {
                List<string> param = new List<string>();
                List<Target> target = new List<Target>();
                target.Add(targetingFleet);
                target.Add((Target)ogg);
                Player pl = Game.getGame().getRappresentation().getObservingPlayer();
                Actions.Action action = Actions.Action.getAction("MoveAction");
                ActionManager.callRule(action, pl, target, param);
            }
        }

        public bool selectionPersingTroughScenes()
        {
            return true;
        }
    }
}
