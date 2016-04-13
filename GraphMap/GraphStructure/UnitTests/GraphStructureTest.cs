using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Common;
using Newtonsoft.Json;

namespace GraphStructure.UnitTests
{
    [TestFixture]
    class GraphStructureTest
    {
        private static Vertexes _Vertexs;

        [Test]
        public void TestSerialize()
        {

            Vertexes vertexs = new Vertexes();
            vertexs.VertexList = new List<Vertex>();

            Vertex v = new Vertex()
            {
                Name = "A",
                Points = new List<float>() { 1.35f, 2.25f },
                Neighbors = new List<Neighbor>() {                      
                                                                                            new Neighbor() { Name = "B", Weight=120.0f},
                                                                                            new Neighbor() { Name = "C", Weight=52.0f}
                                                                                            }
            };

            vertexs.VertexList.Add(v);
            vertexs.VertexList.Add(new Vertex() { Name = "B", Points = new List<float>() { 13.3f, 5.12f } });
            vertexs.VertexList.Add(new Vertex() { Name = "C", Points = new List<float>() { 53.3f, 25.12f } });

            _Vertexs = vertexs;

            string xml = XmlSerializationHelper.SerializeToXml(vertexs);
        }

        [Test]
        public void TestJsonParser()
        {
            TestSerialize();
            string cityList = (new City()).LoadMap(_Vertexs.VertexList);
            string connections = (new Connections()).LoadConnections(_Vertexs.VertexList);
        }
    }
}
