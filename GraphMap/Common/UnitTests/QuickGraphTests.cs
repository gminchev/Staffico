using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using QuickGraph;
using QuickGraph.Algorithms;

namespace Common.UnitTests
{
    [TestFixture]
    class QuickGraphTests
    {

        private AdjacencyGraph<string, Edge<string>> _graph;
        private Dictionary<Edge<string>, double> _costs;

        [Test]
        public void QuickPath()
        {
            _graph = new AdjacencyGraph<string, Edge<string>>();
            _costs = new Dictionary<Edge<string>, double>();

            AddEdgeWithCosts("A", "D", 4.0);
            AddEdgeWithCosts("D", "E", 3.0);
            AddEdgeWithCosts("D", "B", 1.0);
            AddEdgeWithCosts("B", "D", 1.0);
            AddEdgeWithCosts("B", "E", 1.0);
            AddEdgeWithCosts("E", "A", 6.0);
            AddEdgeWithCosts("C", "E", 4.0);
            AddEdgeWithCosts("C", "B", 1.0);

            PrintShortestPath("A", "E");
        }

        private void AddEdgeWithCosts(string source, string target, double cost)
        {
            var edge = new Edge<string>(source, target);
            _graph.AddVerticesAndEdge(edge);
            _costs.Add(edge, cost);
        }

        private void PrintShortestPath(string @from, string to)
        {
            var edgeCost = AlgorithmExtensions.GetIndexer(_costs);
            var tryGetPath = _graph.ShortestPathsDijkstra(edgeCost, @from);

            IEnumerable<Edge<string>> path;
            if (tryGetPath(to, out path))
            {
                var res = path;
            }
            else
            {
                Console.WriteLine("No path found from {0} to {1}.");
            }
          
        }

    }
}
