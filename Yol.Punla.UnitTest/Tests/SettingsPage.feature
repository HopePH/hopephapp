Feature: SettingsPage

Scenario: Trying to go to the settings page without logging in
	Given I am not authenticated
		And I am on the page "WikiPage"
		And the menu detail is closed
	When I tap the hamburger icon
	Then I should see the menu detail is opened
		And I could see that the settings item in the side menu is not displayed

Scenario: Going to the settings via Post Feed Page and trying to edit a profile
	Given I am not authenticated
		And I am on the page "WikiPage"
		And the menu detail is closed
	When I tap the hamburger icon
	Then I should see the menu detail is opened
		And I could see that the settings item in the side menu is not displayed
	When I tap the Write Down icon from the menu detail
	Then I should see a message saying the user to sign up "When logging in your profile will still remain anonymous to the other users."
		And I am redirected to the page "LogonPage"
	When I tap the Login with Facebook Button with mobile account "09477691857"
	Then I am authenticated
		And I am redirected to the page "PostFeedPage"
	When I tap the hamburger icon
	Then I should see the menu detail is opened
		And I could see that the settings item in the side menu is displayed
	When I tap the Settings item in the side menu
	Then I am redirected to the page "SettingsPage"
		And I should see that the dialog is not yet shown
	When I tap on the action item Edit Profile
	Then I should see a not yet available message "Settings is not yet available by this version of the app. Please wait for our next version."
