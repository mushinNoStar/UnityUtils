using Actions;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Game
{
    public sealed class Nation : Target
    {
        private string name = "";
        private static Dictionary<string, Nation> everyNation = new Dictionary<string, Nation>();
        public readonly Galaxy galaxy;
        public int connectionLevel = 0;
        private List<Sector> ownedSectors = new List<Sector>();
        private List<Sector> knowSectors = new List<Sector>();
        private Color color;
        public List<Fleet> fleets = new List<Fleet>();

        public Nation(string nm, Galaxy gal, Color col) : base()
        {
            color = col;
            color.a = 0.4f;
            name = nm;
            everyNation.Add(name, this);
            galaxy = gal;
        }

        public void addFleet(Fleet fl)
        {
            fleets.Add(fl);
        }

        public void removeFleet(Fleet fl)
        {
            fleets.Remove(fl);
        }

        public void tick()
        {
            foreach (Fleet f in fleets)
                f.executeJump();
        }

        public static List<Nation> getNations()
        {
            List<Nation> nat = new List<Nation>();
            foreach (Nation nt in everyNation.Values)
            {
                nat.Add(nt);
            }
            return nat;
        }

        public bool ownsSector(Sector sc)
        {
            return (ownedSectors.Contains(sc));
        }

        public bool knowsSector(Sector sc)
        {
            return (knowSectors.Contains(sc));
        }

        public Color getColor()
        {
            return color;
        }

        public void addKnownSector(Sector sc)
        {
            if (!knowSectors.Contains(sc))
            {
                knowSectors.Add(sc);
            }
        }

        public void setOwnedSector(Sector sc)
        {
            if (ownedSectors.Contains(sc))
                return;
            addKnownSector(sc);
            foreach (Sector r in sc.getNeighbours())
                addKnownSector(r);
            sc.setOwner(this);
            ownedSectors.Add(sc);
        }

        public void removeSector(Sector sc)
        {
            if (ownedSectors.Contains(sc))
            {
                sc.setOwner(null);
                ownedSectors.Remove(sc);
            }
        }

        public string getName()
        {
            return name;
        }

        public void setName(string nm)
        {
            everyNation.Remove(name);
            everyNation.Add(name, this);
        }

        /// <summary>
        /// return the nation from the name
        /// </summary>
        /// <param name="nationName"></param>
        /// <returns></returns>
        public static Nation getNation(string nationName)
        {
            return everyNation[nationName];
        }

        /// <summary>
        /// this is the time that passes for accurate info about the sector
        /// it is a game visibility definition
        /// </summary>
        /// <returns></returns>
        public int getSectorInfoDelay(Sector sc)
        {
            if (ownsSector(sc))
                return 0;
            if (!knowsSector(sc))
                return 100;
            foreach (Sector asd in ownedSectors)
                if (asd.getNeighbours().Contains(sc))
                    return 5;
            return 10;
        }

        public void checkConsistency()
        {
            foreach (Sector sc in ownedSectors)
                if (sc.getOwner() != this)
                    throw new Exception("nation owns and not owns sector");
        }

        [Serializable]
        public class SerializableNation
        {
            public int connectionLevel = 0;
            public float[] color;
            public List<int> knownSectors = new List<int>();
            public List<int> ownedSectors = new List<int>();
            public List<Fleet.SerializableFleet> fleets = new List<Fleet.SerializableFleet>();
            public string name;

            public SerializableNation() { }

            public SerializableNation(Nation nt)
            {
                name = nt.name;
                connectionLevel = nt.connectionLevel;
                foreach (Sector sc in nt.knowSectors)
                {
                   
                    knownSectors.Add(nt.galaxy.getSectors().IndexOf(sc));
                }
                foreach (Sector sc in nt.ownedSectors)
                    ownedSectors.Add(nt.galaxy.getSectors().IndexOf(sc));
                color = new float[] {nt.color.r, nt.color.g, nt.color.b, nt.color.a};

                foreach (Fleet fl in nt.fleets)
                    fleets.Add(new Fleet.SerializableFleet(fl));

            }

            public void setUpNation(Nation n)
            {
                n.connectionLevel = connectionLevel;
                foreach (int i in knownSectors)
                    n.addKnownSector(n.galaxy.getSectors()[i]);
                foreach (int i in ownedSectors)
                    n.setOwnedSector(n.galaxy.getSectors()[i]);
                foreach (Fleet.SerializableFleet fl in fleets)
                {
                    Fleet f = new Fleet(n.galaxy.getSectors()[fl.sector].getSubSectors()[fl.subSector], n, fl.lineShip);
                    fl.setUpFleet(f);
                }
            }
        }
    }
}