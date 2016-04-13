using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Common.UnitTests
{
    [TestFixture]
    class CommonTests
    {
        [Test]
        public void CheckConnectionAndInsert()
        {
            //check connection to db
            string error;
            var conn = new Neo4J("http://neo4j:neo4j@localhost:7474/db/data", out error);

            //check for exists
            var res = conn.Client.Cypher
            .Match("(city:City)")
            .Where((City city) => city.label == "Sofia")
            .Return(city => city.As<City>())
            .Results;

            //delete from db neo4j
            conn.Client.Cypher.Match("(city:City)").Where((City city)=> city.label != "DEL").Delete("city").ExecuteWithoutResults();
            return;

            foreach (ICity newCity in (new List<City>() { new City() { label = "Sofia", x = 25.4f, y = 50.1f, Neighbors = new List<INeighbor>(){ new Neighbor() { Name= "Plovdiv"}, new Neighbor(){ Name = "Veliko Tyrnovo"}} },
                                                         new City() { label = "Plovdiv", x = 125.5f, y = 75f }, 
                                                         new City() { label = "Veliko Tyrnovo", x = 205.4f, y = 150.1f } }))
            {
                //create node
                conn.Client.Cypher
                    .Create("(city:City {newCity})")
                    .WithParam("newCity", newCity)
                    .ExecuteWithoutResults();

                if (newCity.Neighbors != null)
                {
                    foreach (INeighbor neighbor in newCity.Neighbors)
                    {
                        //create connections
                        conn.Client.Cypher
                            .Match("(city1:City)", "(city2:City)")
                            .Where((City city1) => city1.label == newCity.label)
                            .AndWhere((City city2) => city2.label == neighbor.Name)
                            .CreateUnique("city1-[:FRIENDS_WITH]->city2")
                            .ExecuteWithoutResults();
                    }
                }
            }
          
        }

        private class City : ICity
        {
            public string label { get; set; }
            public float x { get; set; }
            public float y { get; set; }
            public Guid id { get; set; }
            public List<INeighbor> Neighbors { get; set; }
        }

        private class Neighbor : INeighbor
        {
            public string Name { get; set; }
            public float Weight { get; set; }
        }
    }
}
