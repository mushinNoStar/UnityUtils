using UnityEngine;
using System.Collections.Generic;

using System;
namespace Game
{
    [Serializable]
    public class GalaxyGenerationParameter
    {
        public int numberOfSystems = 10;
        public int averageNumberOfStarInSector = 5;
        public int plusNumberOfStarInSector = 5;
        public int minusNumberOfStarInSector = 2;
        public float subSectorPos = 1f;
        public int maxInSystemConnections = 1;

        public int galaxyEdge = 30;
        public float connectionsWidth = 0.03f;
        public float areasEdgeWidth = 0.02f;

        public int numberOfPlayer = 8;

        public GalaxyGenerationParameter()
        {

        }

    }
}
