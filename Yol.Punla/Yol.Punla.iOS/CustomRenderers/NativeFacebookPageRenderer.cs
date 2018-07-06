using CoreGraphics;
using Facebook.CoreKit;
using Facebook.LoginKit;
using Foundation;
using System;
using System.Collections.Generic;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using Yol.Punla.iOS.CustomRenderers;
using Yol.Punla.ViewModels;
using Yol.Punla.Views;

[assembly: ExportRenderer(typeof(NativeFacebookPage), typeof(NativeFacebookPageRenderer))]
namespace Yol.Punla.iOS.CustomRenderers
{
    public class NativeFacebookPageRenderer : PageRenderer
    {
        List<string> readPermissions = new List<string> { "public_profile", "email", "user_birthday" };
        LoginButton loginView;
        ProfilePictureView pictureView;
        UILabel nameLabel;

        private NativeFacebookPageViewModel viewModel;

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            if (this.Element is NativeFacebookPage pageElement)
                viewModel = pageElement.ViewModel;

            Profile.Notifications.ObserveDidChange(Profile_Changed);
            
            // Login Button Setup
            loginView = new LoginButton();
            var frame = new CGRect();
            frame.Width = 218;
            frame.X = (UIScreen.MainScreen.Bounds.Width - frame.Width) / 2;
            frame.Height = 46;
            frame.Y = 310;
            loginView.Frame = frame;
            loginView.LoginBehavior = LoginBehavior.Native;
            loginView.ReadPermissions = readPermissions.ToArray();

            loginView.Completed += LoginView_Completed;
            loginView.LoggedOut += LoginView_LoggedOut;

            // The user image profile is set automatically once is logged in
            // Create the label that will hold user's facebook name
            // PictureView Setup
            var picFrame = new CGRect();
            picFrame.Width = 220;
            picFrame.Height = 220;
            picFrame.X = (UIScreen.MainScreen.Bounds.Width - frame.Width) / 2;
            picFrame.Y = 70;
            pictureView = new ProfilePictureView(picFrame);

            // Label Setup
            var labelFrame = new CGRect();
            labelFrame.Width = 220;
            labelFrame.Height = 21;
            labelFrame.Y = 50;
            labelFrame.X = (UIScreen.MainScreen.Bounds.Width - labelFrame.Width) / 2;
            nameLabel = new UILabel(labelFrame)
            {
                TextAlignment = UITextAlignment.Center,
                BackgroundColor = UIColor.Clear
            };

            // If you have been logged into the app before, ask for the your profile name
            if (AccessToken.CurrentAccessToken != null)
                ProcessFBInfo();

            // Add views to main view
            View.AddSubview(loginView);
            View.AddSubview(pictureView);
            View.AddSubview(nameLabel);
        }

        private void LoginView_Completed(object sender, LoginButtonCompletedEventArgs e)
        {
            if (e != null)
            {
                if (e.Error != null)
                {
                    //UNDONE: Handle if there was an error
                }

                if (e.Result.IsCancelled)
                {
                    //UNDONE: Handle if the user cancelled the login request
                }
            }

            //UNDONE: Handle your successful login
            //do nothing 
        }

        private void LoginView_LoggedOut(object sender, EventArgs e)
        {
            LoginManager lm = new LoginManager();
            lm.LogOut();
        }
        
        private void Profile_Changed(object sender, ProfileDidChangeEventArgs e)
        {
            if (e.NewProfile == null)
                return;

            nameLabel.Text = e.NewProfile.Name;

            //saving fb profile to db
            viewModel.FacebookName = e.NewProfile.Name;
            viewModel.FacebookFirstName = e.NewProfile.FirstName;
            viewModel.FacebookLastName = e.NewProfile.LastName;
            viewModel.FacebookId = e.NewProfile.UserID;

            //get the email
            if (AccessToken.CurrentAccessToken != null)
            {
                ProcessFBInfo();
            }
        }

        private void ProcessFBInfo()
        {
            var request = new GraphRequest("/me?fields=email,picture,birthday,gender,link", null, AccessToken.CurrentAccessToken.TokenString, null, "GET");
            request.Start((connection, result, error) => {

                if (error != null)
                {
                    new UIAlertView("Error...", error.Description, null, "Ok", null).Show();
                    return;
                }

                loginView.Enabled = false;
                loginView.Alpha = 0.5f;
                var userInfo = result as NSDictionary;
                viewModel.FacebookEmail = (userInfo["email"] != null) ? userInfo["email"].ToString() : "";

                if (userInfo["birthday"] != null)
                    viewModel.FacebookBirthday = userInfo["birthday"].ToString();

                if (userInfo["gender"] != null)
                    viewModel.FacebookGender = userInfo["gender"].ToString();

                if (userInfo["link"] != null)
                    viewModel.FacebookLink = userInfo["link"].ToString();

                var nsImageURL = Profile.CurrentProfile.ImageUrl(ProfilePictureMode.Normal, new CGSize(220, 220));
                viewModel.FacebookPhoto = nsImageURL.AbsoluteString;
                viewModel.SaveFacebookProfileAsync();
            });
        }
    }
}