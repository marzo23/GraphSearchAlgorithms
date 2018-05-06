using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graph
{
    class Program
    {
        static string robotMapPath = @"C:\Users\L440\source\repos\ryanair\ryanair\bin\Debug\map.txt";
        public static string airportJsonFile = @"C:\Users\L440\source\repos\ryanair\ryanair\bin\Debug\airports.json";
        static void Main(string[] args)
        {
            
            bool[,] boolMap = GetBoolMapFromTxt(robotMapPath);
            ListGraph<MapElement> robotGraph = BooleanMapToGraph(boolMap);
            Console.WriteLine("//////////////////////////////////GRAFO ROBOT//////////////////////////////////\n\n");
            Node<MapElement> inicio = robotGraph.NodeList.Find(f => f.Element.Id == 73);
            robotGraph.RUNALL(robotGraph, inicio, 21);


            List<Airport> airports = new List<Airport>();
            foreach (string json in File.ReadAllLines(airportJsonFile))
                airports.Add(JsonConvert.DeserializeObject<Airport>(json));
            bool[,] relations = GetRelations(airports);
            double?[,] distances = GetDistances(airports, relations);
            ListGraph<Airport> grafo = new MatrixGraph<Airport> { EdgesMatrix = distances, NodeDataList = airports }.GenerateListGraphFromMatrixGraph();

            Console.WriteLine("\n\n\n\n//////////////////////////////////GRAFO RYANAIR//////////////////////////////////\n\n");
            Node<Airport> inicioRyan = grafo.NodeList.Find(f => f.Element.Id == 73);
            grafo.RUNALL(grafo, inicioRyan, 21);

            Console.ReadLine();
        }

        //////////////////////////////////RUNING SEARCH ALGORITHMS+		grafo	{Graph.ListGraph<Graph.Airport>}	Graph.ListGraph<Graph.Airport>

        
        public static void RUNING_ALGORITHMS_ROBOT(ListGraph<MapElement> graph, Node<MapElement> inicio)
        {
            if (inicio != null)
            {
                
            }
        }


        //////////////////////////////////MAPING RYAN AIR AND ROBOT GRAPHS

        public static bool[,] GetRelations(List<Airport> airports)
        {
            bool[,] relations = new bool[airports.Count, airports.Count];

            for (int i = 0; i < airports.Count; i++)
                for (int j = 0; j < airports.Count; j++)
                    relations[i, j] = false;

            foreach (Airport airport in airports)
            {
                foreach (string route in airport.routes)
                {
                    Airport airportAux = airports.Find(a => a.iataCode.Equals(route));
                    if (airportAux != null)
                        relations[airport.Id, airportAux.Id] = true;
                }
            }
            return relations;
        }

        public static double?[,] GetDistances(List<Airport> airports, bool[,] relations)
        {
            double?[,] distances = new double?[airports.Count, airports.Count];
            for (int i = 0; i < airports.Count; i++)
            {
                for (int j = 0; j < airports.Count; j++)
                {
                    if (relations[i, j])
                    {

                        distances[i, j] = DistanceByHarvesine(airports[i].coordinates, airports[j].coordinates);
                    }
                    else
                        distances[i, j] = 0;
                }
            }
            return distances;
        }

        public static bool[,] GetBoolMapFromTxt(string path)
        {
            string[] aux = File.ReadAllLines(path);
            bool[,] map = null;
            if (aux != null)
            {
                int x = aux[0].Length;
                map = new bool[x, aux.Length];
                for (int j = 0; j < aux.Length; j++)
                {
                    if (aux[j].Length != x)
                        return null;
                    for (int i = 0; i < x; i++)
                    {
                        if (aux[j][i] == '0')
                            map[i, j] = false;
                        else
                            map[i, j] = true;
                    }
                }
            }
            return map;
        }

        public static ListGraph<MapElement> BooleanMapToGraph(bool[,] map)
        {
            int x = map.GetLength(0);
            int y = map.GetLength(1);
            int id = 0;
            ListGraph<MapElement> grafo = new ListGraph<MapElement>();
            for (int i = 0; i < y; i++)
            {
                for (int j = 0; j < x; j++)
                {
                    if (map[j, i])
                    {
                        MapElement auxNode = new MapElement { Id = id, x = j, y = i };
                        grafo.NodeList.Add(new Node<MapElement> { Element = auxNode, ElementList = new List<NodeRelation<MapElement>>() });
                    }
                    else
                    {
                        grafo.NodeList.Add(null);
                    }
                    id++;
                }
            }
            for (int i = 0; i < grafo.NodeList.Count; i++)
            {
                if (grafo.NodeList[i] == null)
                    continue;
                if (i > 0 && i % x > 0)
                    if (grafo.NodeList[i - 1] != null)
                        grafo.NodeList[i].ElementList.Add(new NodeRelation<MapElement> { Node = grafo.NodeList[i - 1] , Edge = 1});
                if (i + 1 < grafo.NodeList.Count && (i + 1) % x > 0)
                    if (grafo.NodeList[i + 1] != null)
                        grafo.NodeList[i].ElementList.Add(new NodeRelation<MapElement> { Node = grafo.NodeList[i + 1], Edge = 1 });
                if (i + x < grafo.NodeList.Count)
                    if (grafo.NodeList[i + x] != null)
                        grafo.NodeList[i].ElementList.Add(new NodeRelation<MapElement> { Node = grafo.NodeList[i + x], Edge = 1 });
                if (i - x > 0)
                    if (grafo.NodeList[i - x] != null)
                        grafo.NodeList[i].ElementList.Add(new NodeRelation<MapElement> { Node = grafo.NodeList[i - x], Edge = 1 });
            }
            int k = 0;
            while (k < grafo.NodeList.Count)
            {
                if (grafo.NodeList[k] == null || grafo.NodeList[k].Element == null)
                    grafo.NodeList.RemoveAt(k);
                else
                    k++;
            }
            return grafo;
        }

        public static double DistanceByHarvesine(Coordinates p1, Coordinates p2)
        {
            int r = 6378;
            double rad = Convert.ToSingle(Math.PI / 180);
            double p1x = rad * float.Parse(p1.latitude);
            double p1y = rad * float.Parse(p1.longitude);
            double p2x = rad * float.Parse(p2.latitude);
            double p2y = rad * float.Parse(p2.longitude);

            double x = Math.Abs(p1x - p2x);
            double y = Math.Abs(p1y - p2y);
            double a = Math.Pow(Math.Sin(x / 2), 2) + Math.Cos(p1x) * Math.Cos(p2x) * Math.Pow(Math.Sin(y / 2), 2);
            return Math.Abs(2 * r * Math.Asin(Math.Sqrt(a)));
        }

    }


    public class Airport : IElement<Airport>
    {
        public int Id { get; set; }
        public string iataCode { get; set; }
        public string name { get; set; }
        public Coordinates coordinates { get; set; }
        public List<string> routes { get; set; }

        public override string ToString()
        {
            return this.name + "- Latitude: " + this.coordinates.latitude + "; Longitude: " + this.coordinates.longitude + ";";
        }
        
        public List<Airport> GetElementList(string str)
        {
            throw new NotImplementedException();
        }

        public double? HeuristicByDestination(Airport destination)
        {
            return Program.DistanceByHarvesine(this.coordinates, destination.coordinates);
        }
    }

    public class Coordinates
    {
        public string latitude { get; set; }
        public string longitude { get; set; }
    }
}
