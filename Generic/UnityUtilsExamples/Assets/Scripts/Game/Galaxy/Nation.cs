using Actions;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public sealed class Nation : Target
    {
        private string name = "";
        private static Dictionary<string, Nation> everyNation = new Dictionary<string, Nation>();
        private Galaxy gal;
        public int connectionLevel = 0;
        private List<Sector> ownedSectors = new List<Sector>();
        private List<Sector> knowSectors = new List<Sector>();
        private Color color;

        public Nation(List<string> data, int id) : base (data, id) { }

        public Nation(string nm, Galaxy gal, Color col) : base()
        {
            color = col;
            color.a = 0.4f;
            name = nm;
            everyNation.Add(name, this);
        }

        public override List<string> serialize()
        {
            List<string> dRitorno = base.serialize();
            dRitorno.Add(name);
            dRitorno.Add(connectionLevel+"");
            dRitorno.Add(color.r+"");
            dRitorno.Add(color.b+"");
            dRitorno.Add(color.g+"");
            dRitorno.Add(color.a+"");

            foreach (Sector sc in knowSectors)
                dRitorno.Add(sc.getId()+"");
            dRitorno.Add("#");
            foreach (Sector sc in ownedSectors)
                dRitorno.Add(sc.getId()+"");
            dRitorno.Add("#");

            return dRitorno;
        }

        public override void deserialize(List<string> data)
        {
            ownedSectors.Clear();
            knowSectors.Clear();
            base.deserialize(data);
            name = data[0];
            data.RemoveAt(0);
            connectionLevel = int.Parse(data[0]);
            data.RemoveAt(0);
            color = new Color(float.Parse(data[0]), float.Parse(data[1]), float.Parse(data[2]), float.Parse(data[3]));
            data.RemoveAt(0);
            data.RemoveAt(0);
            data.RemoveAt(0);
            data.RemoveAt(0);

            while (data[0] != "#")
            {
                knowSectors.Add((Sector)getTargetByID(int.Parse(data[0])));
                data.RemoveAt(0);
            }
            data.RemoveAt(0);

            while (data[0] != "#")
            {
                ownedSectors.Add((Sector)getTargetByID(int.Parse(data[0])));
                data.RemoveAt(0);
            }
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
                changed();
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
            changed();
        }

        public void removeSector(Sector sc)
        {
            if (ownedSectors.Contains(sc))
            {
                sc.setOwner(null);
                ownedSectors.Remove(sc);
                changed();
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
            changed();
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

    }
}