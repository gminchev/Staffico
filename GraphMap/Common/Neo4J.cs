using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Neo4jClient;
using Neo4jClient.Cypher;

namespace Common
{
    public class Neo4J
    {
        private readonly IGraphClient _graphClient;

        public Neo4J(string connectionString, out string connectionError)
        {
            connectionError = string.Empty;

            try
            {
                _graphClient = new GraphClient(new Uri(connectionString));
                _graphClient.Connect();
            }
            catch (Exception ex)
            {
                connectionError = string.Format("Error: {0};StackTrace: {1}; InnerException: {2}", ex.Message, ex.StackTrace, ex.InnerException);
            }
        }

        public IGraphClient Client
        { 
            get { return _graphClient; } 
        }

        /// <summary>
        /// Creates the nodes.
        /// </summary>
        /// <typeparam name="T">The type of the T.</typeparam>
        /// <param name="items">The items.</param>
        /// <param name="createText">The create text.</param>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <param name="error">The error.</param>
        /// <returns></returns>
        public bool CreateNodes<T>(List<T> items, string createText, string parameterName, out string error)
        {
            error = string.Empty;

            try
            {
                foreach (T t in items)
                {
                    //Check is this node/vertex is unique.
                    if (Exists<T>(t, createText))
                    {
                        continue;
                    }

                    _graphClient.Cypher
                     .Create(string.Format("({0} {{{1}}})", createText, parameterName))
                     .WithParam(parameterName, t)
                     .ExecuteWithoutResults();
                }

                foreach (T t in items)
                {
                    RelateTwoExistingNodes<T>(t);
                }

                return true;
            }
            catch (Exception ex)
            {
                error = string.Format("Error:{0}; InnerEx: {1}; Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace);
                return false;
            }
        }

        /// <summary>
        /// Existses the specified t.
        /// </summary>
        /// <typeparam name="T">The type of the T.</typeparam>
        /// <param name="t">The t.</param>
        /// <param name="createText">The create text.</param>
        /// <returns></returns>
        private bool Exists<T>(T t, string createText)
        {
            if (t is ICity)
            {
                ICity newCity = t as ICity;
                int count = _graphClient.Cypher
                          .Match(string.Format("({0})", createText))
                          .Where((ICity city) => city.label == newCity.label)
                          .Return(city => city.As<City>())
                          .Results.Count();
               
                return count > 0;

            }

           return false;
        }
  
        /// <summary>
        /// Relates the two existing nodes.
        /// </summary>
        /// <typeparam name="T">The type of the T.</typeparam>
        /// <param name="t">The t.</param>
        private void RelateTwoExistingNodes<T>(T t)
        {
            if (t is ICity)
            {
                ICity city = t as ICity;
                if (city != null)
                {
                    if (city.Neighbors != null)
                    {
                        foreach (INeighbor neighbor in city.Neighbors)
                        {
                            _graphClient.Cypher
                                        .Match("(city1:City)", "(city2:City)")
                                        .Where((ICity city1) => city1.label == city.label)
                                        .AndWhere((ICity city2) => city2.label == neighbor.Name)
                                        .CreateUnique("city1-[:FRIENDS_WITH]->city2")
                                        .ExecuteWithoutResults();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Result class
        /// </summary>
        private class City : ICity
        {
            public Guid id
            {
                get;
                set;
            }

            public string label
            {
                get;
                set;
            }

            public float x
            {
                get;
                set;
            }

            public float y
            {
                get;
                set;
            }

            public List<INeighbor> Neighbors
            {
                get;
                set;
            }
        }
    }
}
