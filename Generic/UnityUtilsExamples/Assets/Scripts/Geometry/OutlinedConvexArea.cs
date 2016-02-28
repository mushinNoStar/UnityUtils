using Rappresentation;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.ObjectModel;

namespace Geometry
{
    public class OutlinedConvexArea : IRappresentable
    {
        private PlaneArea area;
        private List<PlaneSegment> segments = new List<PlaneSegment>();
        private float borderWidth = 1;
        private Material baseMaterial ;

        public OutlinedConvexArea(List<IVertex> verices, Material areaMat, Material segMat, float bWidth)
        {
            borderWidth = bWidth;
            area = new PlaneArea(verices, areaMat);
            baseMaterial = segMat;
            generateBorders(segMat);
        }

        public bool contains(IVertex v)
        {
            if (area.getVertices().Contains(v))
                return true;
            return false;
        }

        public PlaneArea getArea()
        {
            return area;
        }

        public IRappresentation getRappresentation()
        {
            return ((IRappresentable)area).getRappresentation();
        }

        public RappresentationData getRappresentationData()
        {
            return ((IRappresentable)area).getRappresentationData();
        }

        public void hide()
        {
            ((IRappresentable)area).hide();
            foreach (PlaneSegment sgm in segments)
                sgm.hide();
        }

        public bool isVisible()
        {
            return ((IRappresentable)area).isVisible();

        }

        public void setRappresentation(IRappresentation rapp)
        {
            ((IRappresentable)area).setRappresentation(rapp);
        }

        public void setRappresentationData(RappresentationData data)
        {
            ((IRappresentable)area).setRappresentationData(data);
        }

        public void show()
        {
            ((IRappresentable)area).show();
            foreach (PlaneSegment sgm in segments)
                sgm.show();
        }

        private void generateBorders(Material bordersMaterial)
        {
           
            segments.Clear();
            for (int a = 0; a < area.getOutermostVertices().Count -1; a++)
            {
                IVertex v1 = area.getOutermostVertices()[a];
                IVertex v2 = area.getOutermostVertices()[a + 1];
                PlaneSegment sgm;
                sgm = new PlaneSegment(v1, v2, borderWidth, bordersMaterial);
               

                segments.Add(sgm);
            }
            IVertex v3 = area.getOutermostVertices()[0];
            IVertex v4 = area.getOutermostVertices()[area.getOutermostVertices().Count-1];
            PlaneSegment asd;
           
                asd = new PlaneSegment(v3, v4, borderWidth, bordersMaterial);
            segments.Add(asd);
        }

        public ReadOnlyCollection<PlaneSegment> getSegments()
        {
            return segments.AsReadOnly();
        }

        public float getWidth()
        {
            return borderWidth;
        }

        public void setWidth(float f)
        {
            borderWidth = f;
        }

        public void setVerices(List<IVertex> verticesList)
        {
            area.setVerices(verticesList);
            generateBorders(baseMaterial);
        }

        public void removeVertex(IVertex vertex)
        {
            area.removeVertex(vertex);
            generateBorders(baseMaterial);
        }

        public void addVertex(IVertex vertex)
        {
            area.addVertex(vertex);
            generateBorders(baseMaterial);
        }

        public void addVertex(List<IVertex> listVertices)
        {
            area.addVertex(listVertices);
            generateBorders(baseMaterial);
        }
    }
}