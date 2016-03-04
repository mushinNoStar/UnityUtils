using System;
using System.Collections.Generic;
using Game;

namespace Actions
{
    public class MoveAction : Action
    {
        public MoveAction() : base()
        {
            allowedByDefault = true;
            addPossibleTarget(typeof(Game.Fleet));
            addPossibleTarget(typeof(Game.SubSector));
            enabledByDefault = true;
            addPossibleCaller(typeof(Game.Player));
        }

        protected override void apply(Actor caller, List<Target> targets, List<string> otherArgs)
        {
            ((Fleet)targets[0]).setTargetOfMovement((SubSector)targets[1]);
        }
    }
}
