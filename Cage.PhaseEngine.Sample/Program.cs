using Cage.PhaseEngine;
using Cage.PhaseEngine.Engines;
using Cage.PhaseEngine.Sample;
using Cage.PhaseEngine.Strategies;

var _Phases = new[] { Phase.Phase1, Phase.Phase2, Phase.Phase3 };
var _Players = new[] { new Player { Name = "Player 1" }, new Player { Name = "Player 2" }, new Player { Name = "Player 3" } };
var _Presenter = new Presenter();
var _Strategy = default(IPhaseEngineStrategy<Phase, Player>);// 

Console.WriteLine("All of the Phase Engine Strategies will go through each Phase sequentially ");
Console.WriteLine("Pick a Phase Engine Strategy: ");
Console.WriteLine(" [1] Ranked - Each Player is given their turn sequentially, in player order.");
Console.WriteLine(" [2] Round Robin - Each Player is given their turn sequentially, starting in player order, but each round the second player from the previous round becomes the first player.");
Console.WriteLine(" [3] Simultaneous - Goes through each Phase one at a time, but gives all Players in a Phase a turn at the same time.");
Console.WriteLine(" [Other] Exit.");

if (!int.TryParse(Console.ReadLine(), out var _Option))
    return;

switch (_Option)
{
    case 1: _Strategy = new RankedPlayerPhaseEngineStrategy<Phase, Player>(_Phases, _Players); break;
    case 2: _Strategy = new RoundRobinPlayerPhaseEngineStrategy<Phase, Player>(_Phases, _Players); break;
    case 3: _Strategy = new SimultaneousPlayerPhaseEngineStrategy<Phase, Player>(_Phases, _Players); break;
}

if (_Strategy == null)
    return;

using var _Interactor = new PhaseEngineInteractor<Phase, Player>(_Presenter);
var _InputPort = (IPhaseEngineInputPort<Phase, Player>)_Interactor;
var _StartAsync = _InputPort.StartAsync(_Strategy, default);

//for (var _I = 0; _I < 5; _I++)
//{
//    await Task.Delay(1500);
//    await _InputPort.PauseAsync(default);
//    await Task.Delay(1500);
//    await _InputPort.ResumeAsync(default);
//}

var _WaitAndStopAsync = Task.Delay(30000).ContinueWith(task =>
{
    Console.WriteLine("Telling the Phase Engine to stop...");
    return _InputPort.StopAsync(default);
});

await _StartAsync;
await _WaitAndStopAsync;
