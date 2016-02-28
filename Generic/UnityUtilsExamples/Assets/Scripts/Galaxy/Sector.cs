using System;
using Geometry;
using UnityEngine;
using Actions;
using System.Collections.Generic;
using Vision;

namespace Galaxy
{
    public class Sector : IVertex
    {
        private Vector2 position;
        private OutlinedConvexArea area;
        private string name;
        private List<Sector> nearSectors = new List<Sector>();
        private List<PlaneSegment> segments = new List<PlaneSegment>();
        private List<PlaneSegment> borders = new List<PlaneSegment>();

        public Sector(Vector2 pos) : base()
        {
            position = pos;
            name = Tools.Utils.randomName();
        }

        public Sector(float x, float y) : base()
        {
            position = new Vector2(x, y);
            name = Tools.Utils.randomName();
        }

        public Vector2 get2dPosition()
        {
            return position;
        }

        public void setArea(OutlinedConvexArea ar)
        {
           /* if (ar != null)
                ar.getRappresentationData().leftClickOperation -= selectThis;    
            area = ar;
            area.getRappresentationData().leftClickOperation += selectThis;*/
        }

        public Rappresentation.RappresentationData getRappresentationData()
        {
            return area.getRappresentationData();
        }

        public void addSegment(PlaneSegment connectedSegment)
        {
            if (connectedSegment.getEndingPoint() != this && connectedSegment.getStartingPoint() != this)
                return;
            segments.Add(connectedSegment);
        }

        public void addConnection(Sector sc)
        {
            nearSectors.Add(sc);
        }

        public List<PlaneSegment> getBorders()
        {
            return borders;
        }

        public OutlinedConvexArea getArea()
        {
            return area;
        }

        public List<Sector> getNearSectors()
        {
            return nearSectors;
        }

        public void addBorder(PlaneSegment border)
        {
            borders.Add(border);
        }

        protected void OnSelectEnd()
        {
            foreach (PlaneSegment pln in borders)
            {
                pln.getRappresentationData().getMaterial().color = Color.blue;
            }
            //area.getRappresentationData().getMaterial().color = Color.blue;
        }

        protected void OnSelectStart()
        {
            foreach (PlaneSegment pln in borders)
            {
                pln.getRappresentationData().getMaterial().color = Color.yellow;
            }
            //area.getRappresentationData().getMaterial().color = Color.yellow;

        }
    }
}
