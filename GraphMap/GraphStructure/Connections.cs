using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace GraphStructure
{
    public class Connections
    {
        /// <summary>
        /// Gets or sets from.
        /// </summary>
        /// <value>From.</value>
        public Guid from { get; set; }

        /// <summary>
        /// Gets or sets to.
        /// </summary>
        /// <value>To.</value>
        public Guid to { get; set; }

        /// <summary>
        /// Gets or sets the color.
        /// </summary>
        /// <value>The color.</value>
        public string color { get; set; }

        /// <summary>
        /// Loads the connections.
        /// </summary>
        /// <param name="vertexs">The vertexs.</param>
        /// <returns></returns>
        public string LoadConnections(List<Vertex> vertexs, string searchResult = "")
        {
            string color = "000000";

            List<Connections> object2Json = new List<Connections>();
            foreach (Vertex edge in vertexs)
            {
                if (edge.Neighbors == null)
                    continue;
                
                string[] temp = new string[]{};
                if (!string.IsNullOrEmpty(searchResult))
                {
                    temp = searchResult.Split(',');
                }

                foreach (Neighbor neighbor in edge.Neighbors)
                {
                    color = "000000";

                    if (temp.Length > 0)
                    {
                        for(int i=0; i < temp.Length; i++)
                        {
                            if (temp[i].Contains(edge.Name) && temp[i].Contains(neighbor.Name))
                            {
                                color = "ff0000";
                            }
                        }
                    }

                    object2Json.Add(new Connections() { from = edge.id, to = vertexs.Find(v => v.Name.Equals(neighbor.Name)).id, color = color });
                }               
            }

            return JsonConvert.SerializeObject(object2Json);
        }
    }
}
