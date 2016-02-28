using UnityEngine;

namespace Rappresentation
{
    public class RappresentationData
    {
        private Material material;
        public event VoidMethod leftClickOperation;
        public event VoidMethod rightClickOperation;
        public event VoidMethod middleClickOperation;
        public event VoidMethod overOperation;

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

        public void clicked(int mouseButton)
        {
            if (mouseButton == 0 && leftClickOperation != null)
                leftClickOperation();
            if (mouseButton == 2 && rightClickOperation != null)
                rightClickOperation();

            if (mouseButton == 1 && middleClickOperation != null)
                middleClickOperation();
        }

        public void over()
        {
            if (overOperation != null)
                overOperation();
        }
    } 
}