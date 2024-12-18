namespace AdventOfCode.Helpers;

internal class Graph<T> where T : class
{
    public Dictionary<T, Node<T>> Nodes { get; private set; } = [];

    public bool AddNode(T v, IEnumerable<(T, int)> neighbors)
    {
        bool added = false;
        if (!Nodes.TryGetValue(v, out var node))
        {
            added = true;
            node = new Node<T>(v);
            Nodes[node.Value] = node;
        }

        foreach ((T n, int e) in neighbors)
        {
            if (Nodes.TryGetValue(n, out var neighbor))
            {
                node.AddEdge(neighbor, e);
            }
            else
            {
                neighbor = new Node<T>(n);
                node.AddEdge(neighbor, e);
            }
        }
        return added;
    }

    public bool Contains(T v) => Nodes.ContainsKey(v);

    public int ShortestPath(T start, T end) => ShortestPath(n => n == start, n => n == end);

    public int ShortestPath(Func<T, bool> isStart, Func<T, bool> isEnd)
    {
        var unvisited = Nodes.Keys.ToDictionary(n => n, n => isStart(n) ? 0 : -1);

        while (unvisited.Count > 0)
        {
            var node = unvisited
                .Where(x => x.Value != -1)
                .MinBy(x => x.Value)
                .Key ?? throw new InvalidOperationException("No nodes left");

            if (isEnd(node))
            {
                return unvisited[node];
            }

            foreach (var edge in Nodes[node].Edges)
            {
                if (unvisited.ContainsKey(edge.Neighbor.Value))
                {
                    var newScore = unvisited[node] + edge.Cost;
                    if (unvisited[edge.Neighbor.Value] == -1 || newScore < unvisited[edge.Neighbor.Value])
                    {
                        unvisited[edge.Neighbor.Value] = newScore;
                    }
                }
            }

            unvisited.Remove(node);
        }

        throw new InvalidOperationException($"No path from start to end");
    }
}

internal record Node<T>(T Value)
{
    public T Value { get; init; } = Value;

    public List<Edge<T>> Edges { get; } = [];

    public void AddEdge(Node<T> node, int weight)
    {
        Edges.Add(new Edge<T>(node, weight));
    }
}


internal record Edge<T>(Node<T> Neighbor, int Cost);