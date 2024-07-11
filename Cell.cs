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
        public List<Relation> Relations { get; set; }
        public bool Bomb { get; set; }

        public Cell(Panel panel)
        {
            Name = panel.Name;
            Relations = new List<Relation>();
        }

        public (int, int) GetCoords()
        {
            int x = int.Parse(Name[5].ToString());
            int y = int.Parse(Name[7].ToString());
            return new (x, y);
        }
    }

    public class Relation
    {
        public Color Color { get; set; }
        public Cell? Second { get; set; }
    }
}
