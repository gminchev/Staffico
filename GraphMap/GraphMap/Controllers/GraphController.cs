using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GraphStructure;
using GraphMap.Models;
using Common;
using System.Configuration;

namespace GraphMap.Controllers
{
    public class GraphController : Controller
    {
        /// <summary>
        /// Loads the grapth.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns></returns>
        public string LoadGrapth(string fileName)
        {
            List<Vertex> vertexs = Vertexes.Load(fileName);
            TempData["vertex"] = vertexs;

            string jsonResult = string.Format("{{\"city\":{0},\"connections\":{1}}}", (new City()).LoadMap(vertexs), (new Connections()).LoadConnections(vertexs));
            return jsonResult;
        }        

        /// <summary>
        /// Add the map in the graph database neo4j.
        /// </summary>
        public string AddLoadedMapIntoNeo4J()
        {
            List<City> citys = LoadCities();

            string connectionString = ConfigurationManager.AppSettings.Get("ne4jDBConnection");
            if (string.IsNullOrEmpty(connectionString))
            {
                return "err:Missing set for connection string in Web.confing file.";
            }

            string errorMessage;
            var client = new Neo4J(connectionString, out errorMessage);

            if (!string.IsNullOrEmpty(errorMessage))
            {
                return string.Format("err:Can not connect to the database.{0}", errorMessage);
            }

            client.CreateNodes(citys, "city:City", "newCity", out errorMessage);

            if (!string.IsNullOrEmpty(errorMessage))
            {
                return string.Format("err:Can not create nodes in database.{0}", errorMessage);
            }

            return "The map was successfully uploaded into the database.";          
        }

        public string ShortPath(string from, string to)
        {
            List<City> citys = LoadCities();

            var sp = new ShortPath();
            string path = sp.GetShortestPath(citys.ToList<ICity>(), from, to);

            List<Vertex> vertexs = TempData.Peek("vertex") as List<Vertex>;

            string jsonResult = string.Format("{{\"city\":{0},\"connections\":{1}}}", (new City()).LoadMap(vertexs), (new Connections()).LoadConnections(vertexs, path));
            return jsonResult;
        }
  
        private List<City> LoadCities()
        {
            List<Vertex> vertexs = TempData.Peek("vertex") as List<Vertex>;
            List<City> citys = new List<City>();
            vertexs.ForEach(vertex => citys.Add(new City()
            {
                label = vertex.Name,
                Neighbors = vertex.Neighbors.ToList<INeighbor>(),
                id = vertex.id,
                x = vertex.Points.FirstOrDefault(),
                y = vertex.Points.LastOrDefault()
            }));
            return citys;
        }
    }
}