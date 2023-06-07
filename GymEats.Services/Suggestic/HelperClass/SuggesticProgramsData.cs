using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymEats.Services.Suggestic.HelperClass
{
    public class SuggesticProgramNode
    {
        public string id { get; set; }
        public string databaseId { get; set; }
        public string name { get; set; }
        public string author { get; set; }
        public bool isActive { get; set; }
        public bool isPremium { get; set; }
    }

    public class SuggesticProgramEdge
    {
        public SuggesticProgramNode node { get; set; }
    }

    public class Programs
    {
        public List<SuggesticProgramEdge> edges { get; set; }
    }

    public class SuggesticProgramsData
    {
        public Programs programs { get; set; }
    }

    public class SuggesticProgramsRoot
    {
        public SuggesticProgramsData data { get; set; }
    }
}
