using System;
using System.Collections.Generic;

namespace Game
{
    public class Fleet : Movable
    {
        public readonly Nation nation;
        private int lineShip;

        public Fleet(SubSector creationSector, Nation owner,int lineSips) : base (creationSector)
        {
            nation = owner;
            nation.addFleet(this);
            lineShip = lineSips;
        }

        public int getLineShips()
        {
            return lineShip;
        }

        public void merge(Fleet fl)
        {
            if (fl.nation == nation)
            {
                lineShip += fl.lineShip;
            }
            fl.removeFleet();
        }

        /// <summary>
        /// return the sum of every ship in the fleet.
        /// </summary>
        /// <returns></returns>
        public int size()
        {
            return lineShip;
        }

        public void removeFleet()
        {
            nation.removeFleet(this);
            getCurrentLocation().removeMovable(this);
        }

        public override int getConnectionLevel()
        {
            return nation.connectionLevel;
        }

        public class SerializableFleet
        {
            public int lineShip;
            public int sector;
            public int subSector;

            public List<int> sectorPathing = new List<int>();
            public List<int> subSectorPathing = new List<int>();

            public SerializableFleet() { }

            public SerializableFleet(Fleet f)
            {
                lineShip = f.getLineShips();
                sector = f.nation.galaxy.getSectors().IndexOf(f.getCurrentLocation().sector);
                sector = f.getCurrentLocation().sector.getSubSectors().IndexOf(f.getCurrentLocation());

                foreach (SubSector sbs in f.getPathing())
                {
                    sectorPathing.Add(f.nation.galaxy.getSectors().IndexOf(sbs.sector));
                    subSectorPathing.Add(sbs.sector.getSubSectors().IndexOf(sbs));
                }

            }

            public void setUpFleet(Fleet fl)
            {
                List<SubSector> path = new List<SubSector>();
                for (int a = 0; a < sectorPathing.Count; a++)
                {
                    SubSector sbs = fl.nation.galaxy.getSectors()[sectorPathing[a]].getSubSectors()[subSectorPathing[a]];
                    path.Add(sbs);
                }
                fl.setPathing(path);
            }
        }
    }
}