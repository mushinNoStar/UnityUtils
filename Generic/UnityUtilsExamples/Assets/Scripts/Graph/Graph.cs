using System.Collections.Generic;

namespace Graph
{
    public class Graph<T>
    {
        public List<GraphElement<T>> elements = new List<GraphElement<T>>();
        public List<GraphGroup<T>> groups = new List<GraphGroup<T>>();
        public List<GraphEdge<T>> edges = new List<GraphEdge<T>>();
        public List<GraphCycle<T>> cycles = new List<GraphCycle<T>>();

        public Graph(List<T> objects, List<int> edgesToAdd)
        {
            foreach (T ogg in objects)
            {
                GraphGroup<T> group = new GraphGroup<T>();
                groups.Add(group);
                elements.Add(new GraphElement<T>(ogg, group));
            }
            for (int a = 0; a < edgesToAdd.Count; a += 2)
            {

                GraphEdge<T> edge = new GraphEdge<T>(elements[edgesToAdd[a]], elements[edgesToAdd[a+1]]);
                applyEdge(edge);
            }
            removeDoubles();
        }

        public void next()
        {
            

        }

        private void removeDoubles()
        {
            for (int a = 0; a < edges.Count; a++)
            {
                for (int b = a + 1; b < cycles.Count; b++)
                {
                    if (cycles[a].sharesPoints(cycles[b]))
                    {
                        foreach (GraphElement<T> g in cycles[a].elements)
                            g.cycles.Remove(cycles[a]);
                        cycles.RemoveAt(a);
                        a--;
                        break;
                    }
                }
            }
        }

        private void applyEdge(GraphEdge<T> edge)
        {
            edges.Add(edge);
            if (edge.extremes[0].group != edge.extremes[1].group)
            {
                groups.Remove(edge.extremes[1].group);
                edge.extremes[0].group.merge(edge.extremes[1].group);
                edge.extremes[0].edges.Add(edge);
                edge.extremes[1].edges.Add(edge);
            }
            else
            {
                bool happend = false;
                //foreach (GraphCycle<T> cycle in edge.extremes[0].cycles)//maybe this is incorrect
                for (int a = 0; a < edge.extremes[0].cycles.Count; a++)
                {
                    GraphCycle<T> cycle = edge.extremes[0].cycles[a];
                    if (edge.extremes[1].cycles.Contains(cycle))
                    {
                        
                        cycles.Remove(cycle);
                        if (!happend)
                        {
                            edge.extremes[0].edges.Add(edge);
                            edge.extremes[1].edges.Add(edge);
                        }
                        foreach (GraphCycle<T> b in cycle.splitOnEdge(edge))
                            cycles.Insert(0, b);
                        happend = true;
                        a++;
                    }
                }
                if (happend)
                    return;

                List<GraphElement<T>> path = edge.extremes[0].getPathTo(edge.extremes[1]);
                edge.extremes[0].edges.Add(edge);
                edge.extremes[1].edges.Add(edge);
                cycles.Add(new GraphCycle<T>(path));
            }
            
        }
    }

   
}
