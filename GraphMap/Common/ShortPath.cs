using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuickGraph;
using QuickGraph.Algorithms;

namespace Common
{
    public class ShortPath
    {
        private AdjacencyGraph<string, Edge<string>> _graph;
        private Dictionary<Edge<string>, double> _costs;

        private void LoadCosts(List<ICity> vertexes)
        {
            _graph = new AdjacencyGraph<string, Edge<string>>();
            _costs = new Dictionary<Edge<string>, double>();

            foreach (ICity vertex in vertexes)
            {
                if (vertex.Neighbors != null) 
                {
                    foreach (INeighbor neighbor in vertex.Neighbors)
                    {
                        AddVertexesWithCosts(vertex.label, neighbor.Name, neighbor.Weight);
                    }
                }
            }
        }

        private void AddVertexesWithCosts(string source, string target, double cost)
        {
           var edge = new Edge<string>(source, target);
           _graph.AddVerticesAndEdge(edge);
           _costs.Add(edge, cost);
        }

        public string GetShortestPath(List<ICity> vertexes, string from, string to)
        {
            LoadCosts(vertexes);

            var edgeCost = AlgorithmExtensions.GetIndexer(_costs);
            var tryGetPath = _graph.ShortestPathsDijkstra(edgeCost, from);

            IEnumerable<Edge<string>> path;
            if (tryGetPath(to, out path))
            {
                object[] arr = path.ToArray();
                return string.Join(",", arr);
            }
            else
            {
                return string.Format("No path found from {0} to {1}.", from, to);
            }

        }
    }
}
