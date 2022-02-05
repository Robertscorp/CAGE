namespace Cage.TickEngine
{

    public interface ITickEngineInputPort<TUnit>
    {

        #region - - - - - - Methods - - - - - -

        Task PauseAsync(CancellationToken cancellationToken);

        Task ResumeAsync(CancellationToken cancellationToken);

        Task StartAsync(TUnit[] units, CancellationToken cancellationToken);

        Task StopAsync(CancellationToken cancellationToken);

        #endregion Methods

    }

}
