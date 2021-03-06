using Cage.PhaseEngine;
using Cage.PhaseEngine.Engines;
using Cage.PhaseEngine.Sample;
using Cage.PhaseEngine.Strategies;

var _Phases = new[] { Phase.Phase1, Phase.Phase2, Phase.Phase3 };
var _Players = new[] { new Player(1, "Player 1"), new Player(5, "Player 2"), new Player(10, "Player 3") };
var _Presenter = new Presenter(_Phases, _Players);
var _Strategy = default(IPhaseEngineStrategy<Phase, Player>);

Console.WriteLine("All Phase Engine Strategies will go through each Phase sequentially.");
Console.WriteLine();
Console.WriteLine("Pick a Phase Engine Strategy:");
Console.WriteLine(" [1] Ranked - Each Player is given their turn sequentially, in Player order.");
Console.WriteLine(" [2] Round Robin - Each Player is given their turn sequentially, starting in Player order. Each Round the first Player will move to the next Player.");
Console.WriteLine(" [3] Random - Each Player is given their turn sequentially, in a random order.");
Console.WriteLine(" [4] Initiative - Each Player is given their turn sequentially, in Initiative order (P3 = 10, P2 = 5, P1 = 1).");
Console.WriteLine(" [5] Simultaneous - All Players in a Phase take their turn at the same time.");
Console.WriteLine(" [Other] Exit.");

if (!int.TryParse(Console.ReadLine(), out var _Option))
    return;

switch (_Option)
{
    case 1: _Strategy = new RankedPlayerPhaseEngineStrategy<Phase, Player>(_Phases, _Players); break;
    case 2: _Strategy = new RoundRobinPlayerPhaseEngineStrategy<Phase, Player>(_Phases, _Players); break;
    case 3: _Strategy = new RandomPlayerPhaseEngineStrategy<Phase, Player>(_Phases, _Players); break;
    case 4: _Strategy = new PlayerInitiativePhaseEngineStrategy<Phase, Player>(_Phases, _Players); break;
    case 5: _Strategy = new SimultaneousPlayerPhaseEngineStrategy<Phase, Player>(_Phases, _Players); break;
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

var _WaitAndStopAsync = Task.Delay(30000).ContinueWith(task => _InputPort.StopAsync(default));

await _StartAsync;
await _WaitAndStopAsync;
