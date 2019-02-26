Feature: PostFeedMyselfPage
	
Background:
	Given I am not authenticated
		And I am on the page "WikiPage"
		And the menu detail is closed
	When I tap the hamburger icon
	Then I should see the menu detail is opened
	When I tap the write down what you want to say icon
	Then I should see a message saying the user to sign up "When logging in your profile will still remain anonymous to the other users."
		And I am redirected to the page "LogonPage"
	When I tap the logon for facebook button with mobile no "09477691857"
	Then I am redirected to the page "PostFeedPage"

Scenario: Loading my own post feed only
	Given I am on the page "PostFeedPage"
	When I tap the avatar icon at the top right corner
	Then I am redirected to the page "PostFeedMyselfPage"
		And I should see the current contact first name "Worde" and last name "Salinas"
		And I should see the user own post message "Please download the new version of the app"

Scenario Outline: Seeing my own post comments
	Given I am on the page "PostFeedPage"
	When I tap the avatar icon at the top right corner
	Then I am redirected to the page "PostFeedMyselfPage"
		And I should see the current contact first name "Worde" and last name "Salinas"
		And I should see the user own post message "Please download the new version of the app"
	When I tap the comment icon of the post of "<posterName>" with content "<content>" and postFeedId "<postFeedId>" from my own post page
	Then I am redirected to the page "PostFeedDetailPage"
		And I can see the details of the post with author "<posterName>" and content "<content>"
	When I tap the back icon from the post feed detail page
	Then I am redirected to the page "PostFeedMyselfPage"

	Examples: 
	| posterName	| content										| postFeedId |
	| Worde Salinas | Please download the new version of the app    | 1133       |
