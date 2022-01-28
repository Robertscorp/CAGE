namespace Cage.PhaseEngine.Extensions
{
    public static class WaitHandleExtensions
    {

        #region - - - - - - Methods - - - - - -

        public static Task<bool> WaitAsync(this WaitHandle waitHandle, int? timeoutInMilliseconds = null)
        {
            if (waitHandle == null)
                throw new ArgumentNullException(nameof(waitHandle));

            var _TaskCompletionSource = new TaskCompletionSource<bool>();
            var _RegisteredWaitHandle = ThreadPool.RegisterWaitForSingleObject(
                waitHandle,
                callBack: (state, timedOut) => _ = _TaskCompletionSource.TrySetResult(!timedOut),
                state: null,
                millisecondsTimeOutInterval: timeoutInMilliseconds ?? -1,
                executeOnlyOnce: true);

            return _TaskCompletionSource.Task.ContinueWith((task) =>
            {
                _ = _RegisteredWaitHandle.Unregister(waitObject: null);
                try
                {
                    return task.Result;
                }
                catch
                {
                    return false;
                }
            });
        }

        #endregion Methods

    }

}
