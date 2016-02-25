using UnityEngine;

namespace Rappresentation
{
    public class RappresentationData
    {
        private Material material;

        public RappresentationData(Material mt)
        {
            material = mt;
        }

        public Material getMaterial()
        {
            return material;
        }

        public void setMaterial(Material mat)
        {
            material = mat;
        }

    } 
}