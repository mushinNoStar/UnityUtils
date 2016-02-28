using System;
using UnityEngine;

namespace Rappresentation
{
    public abstract class RappresentatioObject : IRappresentation
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

        public abstract void hide();

        public abstract bool isVisible();

        public void setRappresentationData(RappresentationData data)
        {
            rappresentationData = data;
            update();
        }

        public abstract void show();

        public virtual void update()
        {

        }

        public void clicked(int mouseButton)
        {
            rappresentationData.clicked(mouseButton);
        }

        public void over()
        {
            rappresentationData.over();
        }
    }
}