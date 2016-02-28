using System.Collections.Generic;

namespace Graph
{
    public class GraphCycle<T>
    {
        public List<GraphElement<T>> elements = new List<GraphElement<T>>();

        public GraphCycle(List<GraphElement<T>> els)
        {
            for (int a = 0; a < elements.Count - 1; a++)
            {
                if (!els[a].isConnected(els[a + 1]))
                    throw new System.ArgumentException("recived elements are not a cycle");
            }
            if (!els[0].isConnected(els[els.Count - 1]))
                throw new System.ArgumentException("recived elements are not a cycle");

            elements = els;

            foreach (GraphElement<T> e in elements)
                e.cycles.Insert(0, this);

        }

        public List<GraphCycle<T>> splitOnEdge(GraphEdge<T> edge)
        {
            List<GraphCycle<T>> diRitorno = new List<GraphCycle<T>>();
            if (!(elements.Contains(edge.extremes[0]) && elements.Contains(edge.extremes[1])))
                throw new System.ArgumentException("that edge does not cut this cycle");

            int t = elements.IndexOf(edge.extremes[0]) - elements.IndexOf(edge.extremes[1]);
            t = System.Math.Abs(t);
            if (t == 1)
                throw new System.ArgumentException("this edge is already in the cycle");

            List<GraphElement<T>> firstHalf = new List<GraphElement<T>>();
            List<GraphElement<T>> secondHalf = new List<GraphElement<T>>();

            int supp = elements.IndexOf(edge.extremes[0]);
            GraphElement<T> target = edge.extremes[1];
            
            if (elements.IndexOf(edge.extremes[1]) < supp)
            {
                supp = elements.IndexOf(edge.extremes[1]);
                target = edge.extremes[0];
            }
            for (int a = 0; a < supp + 1; a++)
            {
                secondHalf.Add(elements[a]);
            }
            while (elements[supp] != target)
            {
                firstHalf.Add(elements[supp]);
                supp++;
            }
            firstHalf.Add(elements[supp]);
            while (supp < elements.Count)
            {
                secondHalf.Add(elements[supp]);
                supp++;
            }
            diRitorno.Add(new GraphCycle<T>(firstHalf));
            UnityEngine.Debug.Log("InBetween");
            diRitorno.Add(new GraphCycle<T>(secondHalf));
            foreach (GraphElement<T> el in elements)
                el.cycles.Remove(this);
            
            return diRitorno;
        }

        public bool sharesPoints(GraphCycle<T> cycle)
        {
            if (cycle.elements.Count != elements.Count)
                return false;
            foreach (GraphElement<T> el in cycle.elements)
                if (!elements.Contains(el))
                    return false;
            return true;
        }
    }
}