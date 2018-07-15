Feature: NotificationsPage

#the OnResume bdd not working good
@tag OnResume
Scenario: Resuming the app but the user didnt logon to the app
	Given I am not authenticated
		And there are no username and password yet stored in the app
	Then I should see that there are no post feeds to be pushed into the local notifications

@tag OnResumeLogon
Scenario: Resuming the app and the user logon to app previously
	Given I am authenticated
		And the username "hynrbf@gmail.com" and password are stored in the app
	Then I should see that there are post feeds that is pushed into the local notifications
		And I am redirected to the page "WikiPage"
		And I should see that the notifications quantity is displayed at the top banner of the WikiPage

@tag OnResumeLogonAndPushedDateNotExpired
Scenario: Resuming the app and the user logon to app previously but the date when the notifications was pushed was not yet expired
	Given I am authenticated
		And the username "hynrbf@gmail.com" and password are stored in the app
	Then I should see that there are no post feeds to be pushed into the local notifications
		And I should see that the notifications quantity is displayed at the top banner of the WikiPage in only "1"

@tag FacebookLogonViaNotificationsMenu
Scenario: Going to the notifications page for the first time
	Given I am authenticated
		And I am on the page "NotificationsPage"
	Then I should see that there are notifications displayed on the page

@tag FacebookLogonViaNotificationsMenu
Scenario: Going to the notifications page but the user is not connected to the internet
	Given I am authenticated
		And I am on the page "NotificationsPage"
	Then I should see that there are notifications displayed on the page
	When I tap the hamburger icon
	Then I should see the menu detail is opened
	When I tap the Write Down icon from the menu detail pane
	Then I am redirected to the page "PostFeedPage"
	When I tap the hamburger icon
		And I tap the notifications icon but suddenly the internet went off
	Then I am redirected to the page "NotificationsPage"
		And I should see that there are no notifications displayed on the page
		And I should see that the offline message is displayed

@tag FacebookLogonViaNotificationsMenu
Scenario: Going to the notifications page and refreshing the notifications
	Given I am authenticated
		And I am on the page "NotificationsPage"
	Then I should see that there are notifications displayed on the page
	When I pull to refresh the notifications
	Then I should see that the notifications are refreshed

@tag FacebookLogonViaNotificationsMenu
Scenario: Loading comments from the notifications page 
	Given I am authenticated
		And I am on the page "NotificationsPage"
	Then I should see that there are notifications displayed on the page
	When I select a post from the notifications list
	Then I am redirected to the page "PostFeedDetailPage"
