using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace CommanderPodGenerator.Entities;

public class Round
{
    public Round(List<Pod> pods)
    {
        Pods = pods;
    }

    public List<Pod> Pods { get; set; }

    public override string ToString()
    {
        return $"\n\t\t{string.Join("\n\t\t", Pods.Select(x => x.ToString()))}";
    }

    public int GetRepetitions()
    {
        var repetitions = 0;
        foreach (var pod in Pods)
        {
            foreach (var player in pod.Players)
            {
                if (player.PlayedWithIds.Intersect(pod.Players.Select(x => x.Id)).Any())
                    repetitions++;
            }
        }
        return repetitions;
    }

    public void AcceptRound()
    {
        foreach (var pod in Pods)
        {
            foreach (var player in pod.Players)
            {
                player.PlayedWithIds = player.PlayedWithIds.Union(pod.Players.Where(x => x.Id != player.Id).Select(x => x.Id));
            }
        }
    }

    public static Round Generate(List<Player> players)
    {
        var bestRepetitions = int.MaxValue;
        List<Round> bestRounds = [];
        var rng = new Random();
        for (var i = 0; i < 10000; i++)
        {
            var pods = new List<Pod>();
            players = [.. players.OrderBy(_ => rng.Next())];
            var batches = Batch(players, 4);
            batches = batches.Reverse();
            IEnumerable<Player> extraPlayers = null;
            if (batches.First().Count() < 3)
            {
                extraPlayers = batches.First();
                batches = batches.Skip(1);
            }

            IEnumerable<IEnumerable<Player>> modifiedBatches = [];
            foreach (var extraPlayer in extraPlayers)
            {
                // This feels weird but I can't think of how else do to it
                var batchToAddTo = batches.First();
                batches = batches.Skip(1);
                batchToAddTo = batchToAddTo.Append(extraPlayer);
                modifiedBatches= modifiedBatches.Append(batchToAddTo);
            }

            foreach (var modifiedBatch in modifiedBatches)
                batches = batches.Append(modifiedBatch);

            foreach (var batch in batches)
            {
                pods.Add(new Pod(batch));
            }

            var round = new Round(pods);

            var repetitions = round.GetRepetitions();
            if (repetitions == 0)
                return round;
            if (repetitions == bestRepetitions)
            {
                bestRounds.Add(round);
            }
            if (repetitions < bestRepetitions)
            {
                bestRepetitions = repetitions;
                bestRounds.Clear();
                bestRounds.Add(round);
            }
        }

        return bestRounds.ElementAt(rng.Next(0, bestRounds.Count));
    }

    private static IEnumerable<IEnumerable<T>> Batch<T>(IEnumerable<T> list, int batchSize)
    {
        var chunk = new List<T>();
        foreach (var item in list)
        {
            chunk.Add(item);
            if (chunk.Count == batchSize)
            {
                yield return chunk;
                chunk = [];
            }
        }
        if (chunk.Count != 0) yield return chunk;
    }
}
