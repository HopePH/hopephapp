using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using Org.Json;
using System;
using System.Collections.Generic;
using Xamarin.Facebook;
using Xamarin.Facebook.Login;
using Xamarin.Facebook.Login.Widget;
using Xamarin.Forms.Platform.Android;
using Yol.Punla.Contract;
using Yol.Punla.Droid.CustomRenderers;
using Yol.Punla.ViewModels;
using Yol.Punla.Views;

[assembly: Xamarin.Forms.ExportRenderer(typeof(NativeFacebookPage), typeof(NativeFacebookPageRenderer))]
namespace Yol.Punla.Droid.CustomRenderers
{
    public class NativeFacebookPageRenderer : PageRenderer, IFacebookCallback, GraphRequest.IGraphJSONObjectCallback
    {
        protected Android.Views.View NativeView;
        private Activity activity;
        private View view;
        public static ICallbackManager callbackManager;
        List<string> readPermissions = new List<string> { "public_profile", "email", "user_birthday" };
        private NativeFacebookPageViewModel viewModel;

        string firstname;
        string lastname;
        ProfilePictureView profilePictureView;
        Profile profile;
        string fbPictureUrl;

        public NativeFacebookPageRenderer()
        {

        }

        private void InitializedFB()
        {
            activity = this.Context as Activity;
            view = activity.LayoutInflater.Inflate(Resource.Layout.FacebookLayout, this, false);

            FacebookSdk.SdkInitialize(this.Context);
            NativeFacebookPageRenderer.callbackManager = CallbackManagerFactory.Create();

            #region CHECKING IF ALREADY LOGGED IN TO FACEBOOK
            profile = Profile.CurrentProfile;

            if (profile != null)
            {
                firstname = profile.FirstName;
                lastname = profile.LastName;
                GetUserInfoViaGraphRequest();
            }

            #endregion

            LoginManager.Instance.RegisterCallback(NativeFacebookPageRenderer.callbackManager, this);
            var buttonFB = view.FindViewById<Button>(Resource.Id.buttonFB);
            buttonFB.Click += ButtonFB_Click;
            profilePictureView = view.FindViewById<ProfilePictureView>(Resource.Id.profilePicture);

            this.AddView(view);
            NativeView = view;
        }

        private void ButtonFB_Click(object sender, EventArgs e)
        {
            LoginManager.Instance.LogInWithReadPermissions(activity, readPermissions);
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Page> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null || Element == null)
                return;

            var pageElement = this.Element as NativeFacebookPage;

            if (pageElement != null)
                viewModel = pageElement.ViewModel;

            try
            {
                InitializedFB();
            }
            catch (System.Exception ex)
            {

            }
        }

        protected override void OnLayout(bool changed, int l, int t, int r, int b)
        {
            base.OnLayout(changed, l, t, r, b);

            if (NativeView != null)
            {
                var msw = MeasureSpec.MakeMeasureSpec(r - l, MeasureSpecMode.Exactly);
                var msh = MeasureSpec.MakeMeasureSpec(b - t, MeasureSpecMode.Exactly);

                NativeView.Measure(msw, msh);
                NativeView.Layout(0, 0, r - l, b - t);
            }
        }

        public void OnCancel()
        {
            string cancelMsg = "Cancelled";
            Toast.MakeText(Xamarin.Forms.Forms.Context, cancelMsg, ToastLength.Long).Show();
        }

        public void OnError(FacebookException p0)
        {
            string errorMsg = "Error occured: " + p0.Message;
            Toast.MakeText(Xamarin.Forms.Forms.Context, errorMsg, ToastLength.Long).Show();
        }

        public void OnSuccess(Java.Lang.Object p0)
        {
            LoginResult loginResult = p0 as LoginResult;

            var enableButtons = AccessToken.CurrentAccessToken != null;

            if (profile != null)
            {
                firstname = profile.FirstName;
                lastname = profile.LastName;
                profilePictureView.ProfileId = profile.Id;
                Android.Net.Uri profilePic = profile.GetProfilePictureUri(220, 220);
                fbPictureUrl = profilePic.ToString();
            }

            GetUserInfoViaGraphRequest();
        }

        private void GetUserInfoViaGraphRequest()
        {
            GraphRequest request = GraphRequest.NewMeRequest(AccessToken.CurrentAccessToken, this);
            Bundle parameters = new Bundle();
            parameters.PutString("fields", "id,name,first_name,last_name,email,birthday,gender,picture,link");
            request.Parameters = parameters;
            request.ExecuteAsync();
        }

        public void OnCompleted(JSONObject p0, GraphResponse p1)
        {

            FacebookResponseK fbResponse = JsonConvert.DeserializeObject<FacebookResponseK>(p1.RawResponse);
            profile = Profile.CurrentProfile;

            if (profile != null)
            {
                var buttonFB = view.FindViewById<Button>(Resource.Id.buttonFB);
                buttonFB.Enabled = false;
                buttonFB.SetBackgroundColor(Xamarin.Forms.Color.Gray.ToAndroid());

                profilePictureView.ProfileId = profile.Id;
                Android.Net.Uri profilePic = profile.GetProfilePictureUri(220, 220);
                fbPictureUrl = profilePic.ToString();

                //chito.do not force user to must have an email
                viewModel.FacebookEmail = fbResponse.email ?? "";
                viewModel.FacebookFirstName = firstname ?? fbResponse.first_name;
                viewModel.FacebookLastName = lastname ?? fbResponse.last_name;
                viewModel.FacebookPhoto = fbPictureUrl;
                viewModel.FacebookBirthday = fbResponse.birthday;
                viewModel.FacebookGender = fbResponse.gender;
                viewModel.FacebookId = fbResponse.id ?? "0";
                viewModel.FacebookLink = fbResponse.link;
                viewModel.SaveFacebookProfileAsync();
            }
        }
    }
}