/**
 * Prvek spojoveho seznamu pro ulozeni sousedu vrcholu grafu
 */
class Link
{
    /** Cislo souseda */
    public int neighbour;
    /** Odkaz na dalsiho souseda */
    public Link next;
    /** Hodnota grafu */
    public double edgeValue;


    /**
	 * Vytvori novy prvek seznamu pro ulozeni souseda vrcholu grafu
	 */
    public Link(int n, Link next)
    {
        this.neighbour = n;
        this.next = next;
    }
}

/**
 * Graf pro ulozeni mapy
 */
class Graph
{
    /** Sousedi jednotlivych vrcholu (hrany) */
    public Link[] edges;

    /**
	 * Inicializuje sousedy jednotlivych vrcholu (hrany)
	 */
    public void Initialize(int vertexCount)
    {
        edges = new Link[vertexCount];
    }

    /**
	 * Prida do grafu novou obousmernou hranu
	 */
    public void AddEdge(int start, int end)
    {
        edges[start] = new Link(end, edges[start]);
        edges[end] = new Link(start, edges[end]);
    }

    /**
     * Ukládá informace o všech sousedech daného vrcholu
     */
    List<int> neighbours(int v)
    {
        List<int> result = new List<int>();
        Link n = edges[v];
        while (n != null)
        {
            result.Add(n.neighbour);
            n = n.next;
        }
        return result;
    }

    /**
     * Zjistí jestli zadaný prvek "possible" je sousedem "node"
     */
    bool IsNeighbour(int node, int possible)
    {
        Link n = edges[node];
        while (n != null)
        {
            if (n.neighbour == possible)
                return true;
            n = n.next;
        }
        return false;
    }

    /**
     * Metoda pro zjištění nejkratší cesty
     */
    public List<int> ShortestPath(int start, int end)
    {
        List<int> result = new List<int>();
        Queue<int> queue = new Queue<int>();
        bool[] visited = new bool[edges.Length];
        int[] previous = new int[edges.Length];

        queue.Enqueue(start);
        visited[start] = true;
        previous[start] = -1;

        while (queue.Count > 0)
        {
            int current = queue.Dequeue();
            if (current == end)
            {
                // Rekonstrukce cesty zpět od koncového vrcholu
                int node = end;
                
                while (node != -1)
                {
                    result.Insert(0, node);
                    node = previous[node];
                }
                break;
            }

            foreach (int neighbor in neighbours(current))
            {
                if (!visited[neighbor])
                {
                    queue.Enqueue(neighbor);
                    visited[neighbor] = true;
                    previous[neighbor] = current;
                }
            }
        }

        return result;
    }


}

/**
 * Hledani nejkratsi cesty v grafu
 */
public class ShortestPathSearch
{

    public static void Main(String[] args)
    {
        Graph g = new Graph();
        g.Initialize(20);
        g.AddEdge(0, 1);
        g.AddEdge(0, 5);
        g.AddEdge(1, 7);
        g.AddEdge(1, 2);
        g.AddEdge(2, 8);
        g.AddEdge(4, 9);
        g.AddEdge(5,10);
        g.AddEdge(15, 10);
        g.AddEdge(7, 8);
        g.AddEdge(7, 12);
        g.AddEdge(8, 9);
        g.AddEdge(8, 12);
        g.AddEdge(8, 13);
        g.AddEdge(9, 13);
        g.AddEdge(9, 14);
        g.AddEdge(12, 13);
        g.AddEdge(13, 14);
        g.AddEdge(12, 17);
        g.AddEdge(12, 18);
        g.AddEdge(13, 18);
        g.AddEdge(13, 19);
        g.AddEdge(14, 19);
        List<int> data = g.ShortestPath(15, 4);
        Console.WriteLine(data.Count);
        
    }
}