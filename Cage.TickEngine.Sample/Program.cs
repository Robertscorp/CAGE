using Cage.TickEngine;
using Cage.TickEngine.Engines;
using Cage.TickEngine.Sample;

Console.WriteLine("Tick Engine Sample - How many Players would you like for the battle?");

if (!int.TryParse(Console.ReadLine(), out var _PlayerCount))
    return;

var _Units = new List<Unit>();

foreach (var _PlayerNumber in Enumerable.Range(1, _PlayerCount))
{
    Console.WriteLine($"How many Units should Player #{_PlayerNumber} have?");
    if (!int.TryParse(Console.ReadLine(), out var _UnitCount))
        return;

    var _Player = new Player($"Player {_PlayerNumber}");

    _Units.AddRange(
        Enumerable
            .Range(_Units.Count + 1, _UnitCount)
            .Select(n => new Unit(_Player, $"Unit {n}")));
}

Console.Clear();
Console.CursorVisible = false;

var _Presenter = new Presenter(_Units.ToArray());
var _Interactor = new TickEngineInteractor<Phase, Player, Unit>(_Presenter) { TickDelayInMilliseconds = 250 };
var _InputPort = (ITickEngineInputPort<Unit>)_Interactor;

await _InputPort.StartAsync(_Units.ToArray(), default);

Console.CursorVisible = true;
