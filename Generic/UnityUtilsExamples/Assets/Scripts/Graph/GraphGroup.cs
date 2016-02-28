using System.Collections.Generic;

namespace Graph
{
    public class GraphGroup<T>
    {
        public List<GraphElement<T>> elements = new List<GraphElement<T>>();

        public void merge(GraphGroup<T> other)
        {
            foreach (GraphElement<T> el in other.elements)
            {
                el.group = this;
                elements.Add(el);
            }
        }
    }

    
}