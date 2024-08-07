using CommanderPodGenerator.Entities;

Console.WriteLine("\nStarting up...\n\n\n");
Console.WriteLine("To exit, enter 'exit' at anytime");

var response = "";
IEnumerable<string> names = [];
while (response != "Y")
{
    Console.WriteLine("\nPlease enter the players names separated by commas: ");
    var input = Console.ReadLine();
    if (input.ToLower() == "exit")
        return;
    names = input?.Replace(" ", "").Split(',');
    Console.WriteLine($"You input: \n\t{string.Join("\n\t", names)}\nIs that correct? (Y/N) ");
    response = Console.ReadLine()?.Trim().ToUpper();
    if (response.ToLower() == "exit")
        return;
}

var players = new List<Player>();
foreach (var name in names)
{
    players.Add(new Player(name));
}
var roundCount = 0;
while (response == "Y")
{
    Console.WriteLine("-----------------------------------");
    roundCount++;
    var input = "";
    Round round = null;
    while (input != "Y")
    {
        Console.WriteLine($"\nPairing Round {roundCount}...");
        round = Round.Generate(players);
        Console.WriteLine(round);
        Console.WriteLine($"\nRepetition Rating: {round.GetRepetitions()}");
        Console.WriteLine($"\nAccept this round? (Y/N) ");
        input = Console.ReadLine()?.Trim().ToUpper();
        if (input.ToLower() == "exit")
            return;
    }
    round.AcceptRound();
    Console.WriteLine($"\nMove to next round? (Y/N) ");
    response = Console.ReadLine()?.Trim().ToUpper();
    if (response.ToLower() == "exit")
        return;
}