using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Barriers
{
    public class Cell
    {
        public string Name { get; set; }
        public Relation[]? Relation { get; set; }
        public bool Bomb { get; set; }

        public Cell(Panel panel)
        {
            Name = panel.Name;
            Relation = new Relation[2];
        }
    }

    public class Relation
    {
        public Color Color { get; set; }
        public Cell? Second { get; set; }
    }
}
