using System;
using UnityEngine;

namespace Rappresentation
{
    public class RappresentatioObject : IRappresentation
    {
        private RappresentationData rappresentationData;

        public RappresentatioObject(Material mat)
        {
            rappresentationData = new RappresentationData(mat);
        }

        public RappresentationData getRappresentationData()
        {
            return rappresentationData;
        }

        public virtual void hide()
        {

        }

        public virtual bool isVisible()
        {
            return false;
        }

        public void setRappresentationData(RappresentationData data)
        {
            rappresentationData = data;
            update();
        }

        public virtual void show()
        {

        }

        public virtual void update()
        {

        }
    }
}