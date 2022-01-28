using Cage.PhaseEngine;
using Cage.PhaseEngine.Engines;
using Cage.PhaseEngine.Sample;
using Cage.PhaseEngine.Strategies;

var _Phases = new[] { Phase.Phase1, Phase.Phase2, Phase.Phase3 };
var _Players = new[] { new Player { Name = "Player 1" }, new Player { Name = "Player 2" }, new Player { Name = "Player 3" } };
var _Presenter = new Presenter();
var _Strategy = new SequentialRankedPhaseEngineStrategy<Phase, Player>(_Phases, _Players);
var _Interactor = (IPhaseEngineInputPort<Phase, Player>)new PhaseEngineInteractor<Phase, Player>(_Presenter);

var _StartAsync = _Interactor.StartAsync(_Strategy, default);

for (var _I = 0; _I < 5; _I++)
{
    await Task.Delay(1500);
    await _Interactor.PauseAsync(default);
    await Task.Delay(1500);
    await _Interactor.ResumeAsync(default);
}

_ = Task.Delay(10500).ContinueWith(task =>
{
    Console.WriteLine("Telling the Phase Engine to stop...");
    return _Interactor.StopAsync(default);
});

await _StartAsync;