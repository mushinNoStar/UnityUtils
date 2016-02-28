namespace Graph
{
    public class GraphEdge<T>
    {
        public GraphElement<T>[] extremes = new GraphElement<T>[2];

        public GraphEdge(GraphElement<T> em1, GraphElement<T> em2)
        {
            if (em1 == em2)
                throw new System.ArgumentException("they must be different");

            extremes[0] = em1;
            extremes[1] = em2;
        }

        public bool Contains(GraphElement<T> el)
        {
            if (extremes[0] == el || extremes[1] == el)
                return true;
            return false;
        }
    }
    
}
