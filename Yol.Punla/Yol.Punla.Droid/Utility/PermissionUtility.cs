using Android;
using Android.App;
using Android.Content;
using Android.Support.V4.App;
using Android.Support.V4.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Yol.Punla.Droid.Utility
{
    public class PermissionUtility
    {
        private readonly Context _context;

        public PermissionUtility(Context context)
        {
            _context = context;
        }

        IList<string> requestedPermissions;

        public Task<bool> CheckIfHasCameraPermission()
        {
            var names = new List<string>();

            if (HasPermissionInManifest(Manifest.Permission.Camera))
                names.Add(Manifest.Permission.Camera);

            if (names == null)
            {
                return Task.FromResult(false);
            }

            if (names.Count == 0)
            {
                return Task.FromResult(false);
            }

            Context context = _context;

            if (context == null)
            {
                return Task.FromResult(false);
            }

            foreach (var name in names)
            {
                if (ContextCompat.CheckSelfPermission(context, name) == Android.Content.PM.Permission.Denied)
                {
                    return Task.FromResult(false);
                }
            }

            return Task.FromResult(true);
        }

        public Task<bool> CheckIfHasLocationPermission()
        {
            var names = new List<string>();

            if (HasPermissionInManifest(Manifest.Permission.AccessFineLocation))
                names.Add(Manifest.Permission.AccessFineLocation);

            if (HasPermissionInManifest(Manifest.Permission.AccessCoarseLocation))
                names.Add(Manifest.Permission.AccessCoarseLocation);

            if (names == null)
            {
                return Task.FromResult(false);
            }

            if (names.Count == 0)
            {
                return Task.FromResult(false);
            }

            Context context = _context;

            if (context == null)
            {
                return Task.FromResult(false);
            }

            foreach (var name in names)
            {
                if (ContextCompat.CheckSelfPermission(context, name) == Android.Content.PM.Permission.Denied)
                {
                    return Task.FromResult(false);
                }
            }

            return Task.FromResult(true);
        }

        private bool HasPermissionInManifest(string permission)
        {
            try
            {
                if (requestedPermissions != null)
                    return requestedPermissions.Any(r => r.Equals(permission, StringComparison.InvariantCultureIgnoreCase));

                Context context = _context;

                if (context == null)
                {
                    return false;
                }

                var info = context.PackageManager.GetPackageInfo(context.PackageName, Android.Content.PM.PackageInfoFlags.Permissions);

                if (info == null)
                {
                    return false;
                }

                requestedPermissions = info.RequestedPermissions;

                if (requestedPermissions == null)
                {
                    return false;
                }

                return requestedPermissions.Any(r => r.Equals(permission, StringComparison.InvariantCultureIgnoreCase));
            }
            catch (Exception ex)
            {
                Console.Write("Unable to check manifest for permission: " + ex);
            }

            return false;
        }


        object locker = new object();
        const int PermissionCode = 25;
        TaskCompletionSource<Boolean> tcs;
        Boolean result;

        public async Task<Boolean> RequestPermissionsAsync()
        {
            if (tcs != null && !tcs.Task.IsCompleted)
            {
                tcs.SetCanceled();
                tcs = null;
            }

            lock (locker)
            {
                result = new Boolean();
            }

            var activity = _context as Activity;

            if (activity == null)
            {
                result = false;
                return result;
            }

            var permissionsToRequest = new List<string>();

            if (HasPermissionInManifest(Manifest.Permission.Camera))
                permissionsToRequest.Add(Manifest.Permission.Camera);

            if (permissionsToRequest.Count == 0)
            {
                result = false;
                return result;
            }

            tcs = new TaskCompletionSource<Boolean>();
            ActivityCompat.RequestPermissions(activity, permissionsToRequest.ToArray(), PermissionCode);
            return await tcs.Task.ConfigureAwait(false);
        }

        public async Task<Boolean> RequestLocationPermissionAsync()
        {
            if (tcs != null && !tcs.Task.IsCompleted)
            {
                tcs.SetCanceled();
                tcs = null;
            }

            lock (locker)
            {
                result = new Boolean();
            }

            var activity = _context as Activity;

            if (activity == null)
            {
                result = false;
                return result;
            }

            var permissionsToRequest = new List<string>();

            if (HasPermissionInManifest(Manifest.Permission.AccessFineLocation))
                permissionsToRequest.Add(Manifest.Permission.AccessFineLocation);

            if (HasPermissionInManifest(Manifest.Permission.AccessCoarseLocation))
                permissionsToRequest.Add(Manifest.Permission.AccessCoarseLocation);

            if (permissionsToRequest.Count == 0)
            {
                result = false;
                return result;
            }

            tcs = new TaskCompletionSource<Boolean>();
            ActivityCompat.RequestPermissions(activity, permissionsToRequest.ToArray(), PermissionCode);
            return await tcs.Task.ConfigureAwait(false);
        }
    }
}