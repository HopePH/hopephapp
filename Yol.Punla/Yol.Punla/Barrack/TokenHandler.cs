using System;
using System.Threading;
using Yol.Punla.AttributeBase;

namespace Yol.Punla.Barrack
{
    [DefaultModule]
    public class TokenHandler : IDisposable
    {
        private bool isTokenSourceHasNoErrors;

        private CancellationTokenSource _tokenSource;
        public CancellationTokenSource TokenSource { get => _tokenSource; }

        private CancellationToken _token;
        public CancellationToken Token { get => _token; }

        private bool _isTaskHandlerTimerRunning;
        public bool IsTaskHandlerTimerRunning { get => _isTaskHandlerTimerRunning; set => _isTaskHandlerTimerRunning = value; }        

        private string _taskName;
        public string TaskName { get => _taskName; }

        public void Init(string taskName, CancellationTokenSource tokenSource)
        {
            _tokenSource = tokenSource;
            _token = tokenSource.Token;
            _taskName = taskName;

            _isTaskHandlerTimerRunning = true;
            isTokenSourceHasNoErrors = true;
        }

        public bool Destroy()
        {
            _token = CancellationToken.None;
            isTokenSourceHasNoErrors = false;
            return _isTaskHandlerTimerRunning = false;
        }

        public bool IsTokenSourceCompleted()
        {
            var result = isTokenSourceHasNoErrors;
            Destroy();

            return result;
        }

        public void Dispose()
        {
            _tokenSource = null;
        }
    }
}
