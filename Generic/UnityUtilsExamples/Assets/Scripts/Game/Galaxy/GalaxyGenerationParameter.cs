using UnityEngine;
using System.Collections.Generic;

namespace Game
{
    public class GalaxyGenerationParameter
    {
        public int numberOfSystems = 200;
        public int averageNumberOfStarInSector = 5;
        public int plusNumberOfStarInSector = 5;
        public int minusNumberOfStarInSector = 2;

        public int galaxyEdge = 20;
        public float connectionsWidth = 0.03f;
        public float areasEdgeWidth = 0.02f;

        public Material planeMaterial;
        public Material connectionsMaterial;
        public Material areasEdgeMaterial;
        public Material extremeBordersMaterial;

        public GalaxyGenerationParameter(List<Material> mats)
        {
            planeMaterial = mats[0];
            connectionsMaterial = mats[1];
            areasEdgeMaterial = mats[2];
            extremeBordersMaterial = mats[3];
        }
        public GalaxyGenerationParameter()
        {

        }

    }
}
