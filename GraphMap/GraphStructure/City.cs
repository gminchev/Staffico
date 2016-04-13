using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.ComponentModel;
using Common;

namespace GraphStructure
{
    public class City : ICity
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>The id.</value>
        public Guid id { get; set; }

        /// <summary>
        /// Gets or sets the label.
        /// </summary>
        /// <value>The label.</value>
        public string label { get; set; }

        /// <summary>
        /// Gets or sets the x.
        /// </summary>
        /// <value>The x.</value>
        public float x { get; set; }

        /// <summary>
        /// Gets or sets the y.
        /// </summary>
        /// <value>The y.</value>
        public float y { get; set; }

        /// <summary>
        /// Gets or sets the _fixed.
        /// </summary>
        /// <value>The _fixed.</value>
        public Point @fixed { get; set; }

        /// <summary>
        /// Gets or sets the neighbors.
        /// </summary>
        /// <value>The neighbors.</value>
        public List<INeighbor> Neighbors { get; set; }

        /// <summary>
        /// Loads the map.
        /// </summary>
        /// <param name="vertexs">The vertexs.</param>
        /// <returns></returns>
        public string LoadMap(List<Vertex> vertexs)
        {
            List<City> object2Json = new List<City>();
            foreach (Vertex edge in vertexs)
            {
                object2Json.Add(new City() { id = edge.id, label = edge.Name, x = edge.Points.First(), y = edge.Points.Last(), @fixed = new Point() { x = true, y = true } });                
            }

            return JsonConvert.SerializeObject(object2Json);
        }
    }

    public class Point
    {
        public bool x { get; set; }
        public bool y { get; set; }
    }
}
