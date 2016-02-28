using System.Collections.Generic;

namespace Graph
{
    public class GraphElement<T>
    {
        public T element;
        public GraphGroup<T> group;
        public List<GraphEdge<T>> edges = new List<GraphEdge<T>>();
        public List<GraphCycle<T>> cycles = new List<GraphCycle<T>>();
        private float djastrakInfo = 0;
        private GraphElement<T> djastrakInfoPrevious = null;
        private bool djastrakVisited = false;

        public GraphElement(T el, GraphGroup<T> g)
        {
            element = el;
            group = g;
            group.elements.Add(this);
        }

        public bool isConnected(GraphElement<T> el)
        {
            foreach (GraphEdge<T> edge in edges)
                if (edge.Contains(el))
                    return true;
            return false;
        }

        public List<GraphElement<T>> getPathTo(GraphElement<T> target)
        {
            if (target.group != group)
                return null;

            if (target == this)
                UnityEngine.Debug.Log("Incorrect");

            List<GraphElement<T>> path = new List<GraphElement<T>>();
            inizialize(group, this);
            List<GraphElement<T>> S = new List<GraphElement<T>>();
            List<GraphElement<T>> Q = new List<GraphElement<T>>();
            foreach (GraphElement<T> el in group.elements)
                Q.Add(el);
            GraphElement<T> u = null;
            while (Q.Count > 0)
            {
                u = extractMin(Q);
                S.Add(u);
                if (u == target)
                    break;
                foreach (GraphElement<T> v in u.getAdjacent())
                    relax(u, v);
            }

            while (u != this)
            {
                path.Add(u);
                u = u.djastrakInfoPrevious;
            }
            path.Add(u);
            path.Reverse();
            return path;

        }

        private GraphElement<T> extractMin(List<GraphElement<T>> Q)
        {
            int lowest = 0;
            for (int a =0; a < Q.Count; a++)
            {
                if (Q[lowest].djastrakInfo > Q[a].djastrakInfo)
                {
                    lowest = a;
                }
            }
            GraphElement<T> el = Q[lowest];
            Q.RemoveAt(lowest);
            return el;
        }

        public List<GraphElement<T>> getAdjacent()
        {
            List<GraphElement<T>> diRitorno = new List<GraphElement<T>>();
            foreach (GraphEdge<T> edge in edges)
            {
                if (edge.extremes[0] == this)
                    diRitorno.Add(edge.extremes[1]);
                else
                    diRitorno.Add(edge.extremes[0]);
            }
            return diRitorno;

        }

        private void inizialize(GraphGroup<T> g, GraphElement<T> el)
        {
            foreach (GraphElement<T> ele in g.elements)
            {
                ele.djastrakInfo = 1000000000;
                ele.djastrakInfoPrevious = null;
                ele.djastrakVisited = false;
            }
            el.djastrakInfo = 0;
            el.djastrakVisited = true;
        }

        private void relax(GraphElement<T> el2, GraphElement<T> el)
        {
            if (el.djastrakVisited)
                return;
            if (el.djastrakInfo > el2.djastrakInfo + 1)
            {
                el.djastrakInfo = el2.djastrakInfo + 1;
                el.djastrakInfoPrevious = el2;
                el.djastrakVisited = true;
            }
        }
        
    }
}