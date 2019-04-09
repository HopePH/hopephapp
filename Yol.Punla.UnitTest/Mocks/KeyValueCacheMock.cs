using System;
using Yol.Punla.Utility;

namespace Yol.Punla.UnitTest.Mocks
{
    public class KeyValueCacheMock : IKeyValueCacheUtility
    {
        private bool _isWelcomeInstructionLoaded;
        private bool _isAuthenticated;
        private bool _isSignUpCompleted;
        private string _newPage;
        private int _currentContactId;
        private double _currentLatitude;
        private double _currentLongitude;
        private string _appCurrentVersion = "1.9";
        private string _userName;
        private string _password;
        private DateTime _notificationsPushedDate;
        private int _noOfNotications;

        public string GetUserDefaultsKeyValue(string key, string setInitialValueWhenNull = "")
        {
            if (key == "NewPage" && !string.IsNullOrEmpty(setInitialValueWhenNull))
                _newPage = setInitialValueWhenNull;

            if (key == "WasLogin" && !string.IsNullOrEmpty(setInitialValueWhenNull))
                _isAuthenticated = bool.Parse(setInitialValueWhenNull);

            if (key == "WasSignUpCompleted" && !string.IsNullOrEmpty(setInitialValueWhenNull))
                _isSignUpCompleted = bool.Parse(setInitialValueWhenNull);

            if (key == "NotificationsPushedDate" && !string.IsNullOrEmpty(setInitialValueWhenNull))
                _notificationsPushedDate = DateTime.Parse(setInitialValueWhenNull);

            if (key == "NoOfNotifications" && !string.IsNullOrEmpty(setInitialValueWhenNull))
                _noOfNotications = int.Parse(setInitialValueWhenNull);

            if (key == "WasLogin")
                return _isAuthenticated.ToString();

            if (key == "WasSignUpCompleted")
                return _isSignUpCompleted.ToString();

            if (key == "NewPage")
                return _newPage;

            if (key == "CurrentContactId")
                return _currentContactId.ToString();

            if (key == "CurrentLatitude")
                return _currentLatitude.ToString();

            if (key == "CurrentLongitude")
                return _currentLongitude.ToString();

            if (key == "AppCurrentVersion")
                return _appCurrentVersion;

            if (key == "WasWelcomeInstructionLoaded")
                return _isWelcomeInstructionLoaded.ToString();

            if (key == "Username")
                return _userName;

            if (key == "Password")
                return _password;

            if (key == "NotificationsPushedDate")
                return _notificationsPushedDate.ToString();

            if (key == "NoOfNotifications")
                return _noOfNotications.ToString();
            
            return "";
        }

        public void RemoveKeyObject(string key)
        {
            //do nothing
        }

        public void SetAuthenticationManually(bool val)
        {
            _isAuthenticated = val;
        }

        public void SetSignUpCompletionManually(bool val)
        {
            _isSignUpCompleted = val;
        }

        public void SetCurrentContactIdManually(int contactId)
        {
            _currentContactId = contactId;
        }

        public void SetPositionManually(double latitude, double longitude)
        {
            _currentLatitude = latitude;
            _currentLongitude = longitude;
        }

        public void SetAppCurrentVersionManually(string appVersion)
        {
            _appCurrentVersion = appVersion;
        }

        public void SetWelcomeInstructionLoadedManually(bool isLoaded)
        {
            _isWelcomeInstructionLoaded = isLoaded;
        }

        public void SetUserAndPasswordManually(string userName, string password)
        {
            _userName = userName;
            _password = password;
        }

        public void SetNotificationsPushedDateManually(DateTime pushedDate)
        {
            _notificationsPushedDate = pushedDate;
        }
    }
}
