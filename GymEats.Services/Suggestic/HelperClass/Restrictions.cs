using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymEats.Services.Suggestic.HelperClass
{
    public class Node
    {
        public string id { get; set; }
        public string name { get; set; }
        public string subcategory { get; set; }
        public string slugname { get; set; }
        public bool isOnProgram { get; set; }
    }

    public class Edge
    {
        public Node node { get; set; }
    }

    public class Restrictions
    {
        public List<Edge> edges { get; set; }
    }

    public class RestrictionData
    {
        public Restrictions restrictions { get; set; }
    }


    public class RestrictionResponseData
    {
        public Restrictions restrictions { get; set; }
        public List<string> subcategories { get; set; }
    }
}
