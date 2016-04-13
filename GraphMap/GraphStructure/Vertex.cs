using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Xml.Serialization;
using Common;

namespace GraphStructure
{
    [Serializable]
    [XmlRoot]
    public class Vertexes
    {
        [XmlElement("Vertex")]
        public List<Vertex> VertexList { get; set; }

        public static List<Vertex> Load(string file)
        {
            List<Vertex> vertexs = (XmlSerializationHelper.DeserializeFromXmlFile(typeof(Vertexes), file) as Vertexes).VertexList;
            vertexs.ForEach(item => item.id = Guid.NewGuid());
            return vertexs;
        }
    }

    [Serializable] 
    public class Vertex
    {
        [XmlIgnore]
        public Guid id { get; set; }

        [XmlElement("name")]
        public string Name { get; set; }

        public List<Neighbor> Neighbors { get; set; }

        [XmlArray("point")]
        [XmlArrayItem("coordinate")]
        public List<float> Points { get; set; }        
    }

    [Serializable]
    public class Neighbor : INeighbor
    {
        [XmlElement("name")]
        public string Name { get; set; }

        [XmlElement("weight")]
        public float Weight { get; set; }
    }  
}
