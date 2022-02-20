using Cage.CombatEngine;
using Cage.CombatEngine.Common;
using Cage.CombatEngine.ResourcePools;
using Cage.CombatEngine.Sample;
using Cage.CombatEngine.Sample.Resources;

var _ResourcePoolInteractors = new IResourcePoolInputPort[]
{
    ResourcePoolInteractors.HealthPoolInteractor,
    ResourcePoolInteractors.EnergyPoolInteractor,
    ResourcePoolInteractors.ComboPointsPoolInteractor
};

ConsoleManager.Instance.AddResourcePools(
    ResourcePoolViewModels.HealthPool,
    ResourcePoolViewModels.EnergyPool,
    ResourcePoolViewModels.ComboPointsPool);

ConsoleManager.Instance.DisplayResourcePools();

while (true)
{
    if (!ConsoleManager.Instance.TryGetUserInput("Which Resource Pool would you like to affect?", out var _UserChoice,
        Resource.Health.Name, Resource.Energy.Name, Resource.ComboPoints.Name))
        return;

    ConsoleManager.Instance.ResetResults();

    var _ResourcePoolInteractor = _ResourcePoolInteractors[_UserChoice - 1];

    if (!ConsoleManager.Instance.TryGetUserInput("What Operation would you like to perform on it?", out _UserChoice,
        "Change the Base Capacity",
        "Consume Resource",
        "Restore Resource"))
        return;

    if (_UserChoice == 1)
    {
        if (!TryGetBaseCapacityRequest(out var _Request))
            return;

        await _ResourcePoolInteractor.ChangeBaseCapacityAsync(_Request, default);
    }
    else if (_UserChoice == 2)
    {
        if (!TryGetConsumeResourceRequest(out var _Request))
            return;

        await _ResourcePoolInteractor.ConsumeResourceAsync(_Request, default);
    }
    else if (_UserChoice == 3)
    {
        if (!TryGetRestoreResourceRequest(out var _Request))
            return;

        await _ResourcePoolInteractor.RestoreResourceAsync(_Request, default);
    }
}

bool TryGetBaseCapacityRequest(out ChangeBaseCapacityRequest request)
{
    request = new()
    {
        BaseCapacityChange = -10,
        CapacityChangeStrategy = ResourceCapacityChangeStrategies.DecreaseResourceOnly,
        RemainingResourceRoundingStrategy = DecimalRoundingStrategies.DontRound
    };

    return true;
}

bool TryGetConsumeResourceRequest(out ConsumeResourceRequest request)
{
    request = new()
    {
        AmountToConsume = 10,
        CanExhaustResourcePool = true
    };

    return true;
}

bool TryGetRestoreResourceRequest(out RestoreResourceRequest request)
{
    request = new()
    {
        AmountToRestore = 10
    };

    return true;
}
