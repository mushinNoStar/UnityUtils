using UnityEngine;
using System.Collections.Generic;
namespace Game
{
    public class SubSectorClone
    {
        public readonly string name;
        public readonly bool isKnown;
        public readonly bool owns;
        public readonly float infoDelay;
        public readonly Color landColor;
        public readonly List<FleetClone> fleets = new List<FleetClone>();

        public SubSectorClone(SubSector subs, Player player)
        {
            if (subs.sector.getOwner() == null)
            {
                landColor = Color.gray;
                landColor.a = 0.3f;
            }
            else
            {
                landColor = subs.sector.getOwner().getColor();
            }
            name = subs.getName();
            if (player.getObservingNation() == null)
            {
                isKnown = true;
                owns = false;
                infoDelay = 0;
                foreach (Fleet f in subs.getFleet())
                {
                    fleets.Add(new FleetClone(f, player));
                    
                }
            }
            else
            {
                isKnown = player.getObservingNation().knowsSector(subs.sector);
                owns = player.getObservingNation().ownsSector(subs.sector);
                infoDelay = player.getObservingNation().getSectorInfoDelay(subs.sector);
                if (isKnown)
                    foreach (Fleet f in subs.getFleet())
                        fleets.Add(new FleetClone(f, player));
            }
        }
    }
}