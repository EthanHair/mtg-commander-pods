using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommanderPodGenerator.Entities;

public class Pod
{
    public Pod(IEnumerable<Player> players)
    {
        Players = players;
    }

    public IEnumerable<Player> Players { get; set; } = [];

    public override string ToString()
    {
        return $"[ {string.Join(", ", Players.Select(x => x.Name))} ]";
    }
}
