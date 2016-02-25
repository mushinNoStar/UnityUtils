using System;
using UnityEngine;

namespace Rappresentation
{
    public class Rappresentable : IRappresentable
    {
        private IRappresentation rappresentation;

        public RappresentationData getRappresentationData()
        {
            return rappresentation.getRappresentationData();
        }

        public void hide()
        {
            rappresentation.hide();
        }

        public bool isVisible()
        {
            return rappresentation.isVisible();
        }

        public void setRappresentationData(RappresentationData data)
        {
            rappresentation.setRappresentationData(data);
        }

        public void show()
        {
            rappresentation.show();
        }

        public IRappresentation getRappresentation()
        {
            return rappresentation;
        }

        public void setRappresentation(IRappresentation rapp)
        {
            rappresentation = rapp;
        }

        protected void update()
        {
            rappresentation.update();
        }
    }
}