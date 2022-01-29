using Cage.PhaseEngine;
using Cage.PhaseEngine.Engines;
using Cage.PhaseEngine.Sample;
using Cage.PhaseEngine.Strategies;

var _Phases = new[] { Phase.Phase1, Phase.Phase2, Phase.Phase3 };
var _Players = new[] { new Player { Name = "Player 1" }, new Player { Name = "Player 2" }, new Player { Name = "Player 3" } };
var _Presenter = new Presenter();
var _Strategy = new SequentialRankedPhaseEngineStrategy<Phase, Player>(_Phases, _Players);

using var _Interactor = new PhaseEngineInteractor<Phase, Player>(_Presenter);
var _InputPort = (IPhaseEngineInputPort<Phase, Player>)_Interactor;
var _StartAsync = _InputPort.StartAsync(_Strategy, default);

for (var _I = 0; _I < 5; _I++)
{
    await Task.Delay(1500);
    await _InputPort.PauseAsync(default);
    await Task.Delay(1500);
    await _InputPort.ResumeAsync(default);
}

_ = Task.Delay(10500).ContinueWith(task =>
{
    Console.WriteLine("Telling the Phase Engine to stop...");
    return _InputPort.StopAsync(default);
});

await _StartAsync;