using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommanderPodGenerator.Entities;

public class Player
{
    public Player(string name)
    {
        Name = name;
    }

    public Guid Id { get; set; } = Guid.NewGuid();

    public string Name { get; set; }

    public IEnumerable<Guid> PlayedWithIds { get; set; } = [];
}
