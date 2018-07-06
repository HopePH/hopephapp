using Acr.UserDialogs;
using Splat;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Yol.Punla.UnitTest.Mocks
{
    public class UserDialogMock : IUserDialogs
    {
        public IDisposable ActionSheet(ActionSheetConfig config)
        {
            throw new NotImplementedException();
        }

        public Task<string> ActionSheetAsync(string title, string cancel, string destructive, CancellationToken? cancelToken = null, params string[] buttons)
        {
            throw new NotImplementedException();
        }

        public IDisposable Alert(string message, string title = null, string okText = null)
        {
            return new AlertResult(true, message);
        }

        public IDisposable Alert(AlertConfig config)
        {
            return null;
        }

        public Task AlertAsync(string message, string title = null, string okText = null, CancellationToken? cancelToken = null)
        {
            return Task.CompletedTask;
        }

        public Task AlertAsync(AlertConfig config, CancellationToken? cancelToken = null)
        {
            return null;
        }

        public IDisposable Confirm(ConfirmConfig config)
        {
            return null;
        }

        public Task<bool> ConfirmAsync(string message, string title = null, string okText = null, string cancelText = null, CancellationToken? cancelToken = null)
        {
            return Task.FromResult<bool>(true);
        }

        public Task<bool> ConfirmAsync(ConfirmConfig config, CancellationToken? cancelToken = null)
        {
            return null;
        }

        public IDisposable DatePrompt(DatePromptConfig config)
        {
            return null;
        }

        public Task<DatePromptResult> DatePromptAsync(DatePromptConfig config, CancellationToken? cancelToken = null)
        {
            return null;
        }

        public Task<DatePromptResult> DatePromptAsync(string title = null, DateTime? selectedDate = null, CancellationToken? cancelToken = null)
        {
            return null;
        }

        public void HideLoading()
        {
           
        }

        public IProgressDialog Loading(string title = null, Action onCancel = null, string cancelText = null, bool show = true, MaskType? maskType = null)
        {
            return null;
        }

        public IDisposable Login(LoginConfig config)
        {
            return null;
        }

        public Task<LoginResult> LoginAsync(string title = null, string message = null, CancellationToken? cancelToken = null)
        {
            return null;
        }

        public Task<LoginResult> LoginAsync(LoginConfig config, CancellationToken? cancelToken = null)
        {
            return null;
        }

        public IProgressDialog Progress(ProgressDialogConfig config)
        {
            return null;
        }

        public IProgressDialog Progress(string title = null, Action onCancel = null, string cancelText = null, bool show = true, MaskType? maskType = null)
        {
            return null;
        }

        public IDisposable Prompt(PromptConfig config)
        {
            return null;
        }

        public Task<PromptResult> PromptAsync(string message, string title = null, string okText = null, string cancelText = null, string placeholder = "", InputType inputType = InputType.Default, CancellationToken? cancelToken = null)
        {
            return null;
        }

        public Task<PromptResult> PromptAsync(PromptConfig config, CancellationToken? cancelToken = null)
        {
            return null;
        }

        public void ShowError(string message, int timeoutMillis = 2000)
        {
            
        }

        public void ShowImage(IBitmap image, string message, int timeoutMillis = 2000)
        {
            
        }

        public void ShowLoading(string title = null, MaskType? maskType = null)
        {
           
        }

        public void ShowSuccess(string message, int timeoutMillis = 2000)
        {
           
        }

        public IDisposable TimePrompt(TimePromptConfig config)
        {
            return null;
        }

        public Task<TimePromptResult> TimePromptAsync(TimePromptConfig config, CancellationToken? cancelToken = null)
        {
            return null;
        }

        public Task<TimePromptResult> TimePromptAsync(string title = null, TimeSpan? selectedTime = null, CancellationToken? cancelToken = null)
        {
            return null;
        }

        public IDisposable Toast(string title, TimeSpan? dismissTimer = null)
        {
            return null;
        }

            public IDisposable Toast(ToastConfig cfg)
        {
            return null;
        }

        private void DoNothingMethod()
        {

        }

    }

    public class AlertResult : IDisposable
    {
        private bool _isSuccess = false;
        public bool IsSuccess { get => _isSuccess; set => _isSuccess = value; }

        private string _message = "";
        public string Message => _message;

        public AlertResult(bool isSuccess, string message)
        {
            _isSuccess = isSuccess;
            _message = message;
        }

        public void Dispose()
        {

        }
    }
}
