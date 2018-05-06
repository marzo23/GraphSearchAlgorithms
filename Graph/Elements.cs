using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graph
{
    public interface IModel<T>
    {
        int Id { get; set; }
        double? HeuristicByDestination(T destination);
    }

    public interface IElement<T>: IModel<T>
    {
        List<T> GetElementList(string str);
        
    }


    public class BasicElement : IElement<BasicElement>
    {
        public int Id { get; set; }
        public string Value { get; set; }

        public List<BasicElement> GetElementList(string columns)
        {
            List<BasicElement> list = new List<BasicElement>();
            int i = 0;
            foreach (string col in columns.Split(','))
            {
                list.Add(new BasicElement { Id = i++, Value = col });
            }
            return list;
        }

        public List<BasicElement> GetElementList(int nodeCount)
        {
            List<BasicElement> list = new List<BasicElement>();
            for (int i = 0; i < nodeCount; i++)
            {
                list.Add(new BasicElement { Id = i++, Value = null });
            }
            return list;
        }

        public double? HeuristicByDestination(BasicElement destination)
        {
            return destination.Id - this.Id;
        }
    }

    public class MapElement : IElement<MapElement>
    {
        public int Id { get; set; }
        public int x { get; set; }
        public int y { get; set; }

        public List<MapElement> GetElementList(string map)
        {
            throw new NotImplementedException();
        }

        public double? HeuristicByDestination(MapElement destination)
        {
            Console.WriteLine("Destination x: {0}, Destination y: {1}", destination.x, destination.y);
            Console.WriteLine("origen x: {0}, origen y: {1}", x, y);
            return Math.Sqrt(Math.Pow(destination.x - this.x, 2) + Math.Pow(destination.y - this.y, 2));
        }
    }

    public class InformationElement<Model> : IElement<InformationElement<Model>> where Model : IModel<Model>
    {
        public int Id { get; set; }

        public Model Information { get; set; }

        public List<InformationElement<Model>> GetElementList(string jsonArray)
        {
            if (jsonArray == null || jsonArray.Equals(string.Empty))
                return null;
            List<InformationElement<Model>> list = new List<InformationElement<Model>>();
            Model[] models = JsonConvert.DeserializeObject<Model[]>(jsonArray);
            int id = 0;
            foreach (Model model in models)
            {
                list.Add(new InformationElement<Model> { Information = model, Id = id++});
            }
            return list;

        }

        public double? HeuristicByDestination(InformationElement<Model> destination)
        {
            return Information.HeuristicByDestination(destination.Information);
        }
    }
}
