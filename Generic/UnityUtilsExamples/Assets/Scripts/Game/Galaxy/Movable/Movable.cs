using System;
using System.Collections.Generic;
using Vision;
using Actions;

namespace Game
{
    public abstract class Movable : Target
    {
        private SubSector currentLocation = null;

        private List<SubSector> path = new List<SubSector>();

        public Movable(SubSector startingLocation)
        {
            startingLocation.addMovable(this);
            currentLocation = startingLocation;
        }

        public SubSector getCurrentLocation()
        {
            return currentLocation;
        }

        public void setTargetOfMovement(SubSector sector)
        {
            if (currentLocation == null)
                throw new Exception("error");
            if (sector == null)
                throw new Exception("error");

            path = SubSector.getPathing(currentLocation, sector, sector.galaxy, getConnectionLevel());
            
        }

        public abstract int getConnectionLevel();

        public void executeJump()
        {
            if (path.Count > 0)
            {
                currentLocation.removeMovable(this);
                currentLocation = path[0];
                currentLocation.addMovable(this);
                path.RemoveAt(0);
            }

        }

        public List<SubSector> getPathing()
        {
            return path;
        }

        public void setPathing(List<SubSector> newPath)
        {
            for (int a = 0; a < newPath.Count - 1; a++)
                if (!newPath[a].getNeighbours().Contains(path[a + 1]))
                    throw new Exception("path incorrect");
            path = newPath;
        }
    }
}