Feature: LogonPage

Background:
	Given I am not authenticated
		And I am on the page "LogonPage"
	When I tap on the Sign In button
	Then I am redirected to the page "RequestSigninVerificationCodePage"
	#	And the menu detail is closed
	#When I tap the hamburger icon
	#Then I should see the menu detail is opened
	#When I tap the Write Down icon from the menu detail with authenticated "false" and signed up "true"
	#Then I should see a message saying the user to sign up "When logging in your profile will still remain anonymous to the other users."
	#	And I am redirected to the page "LogonPage"
	
Scenario Outline: Logging in to the app using valid email and password
	Given I am not authenticated
	#	And I am on the page "LogonPage"
	#When I tap the Login with Facebook Button with account "<fbAccount>"
	#Then I am authenticated
	#	And I am redirected to the page "PostFeedPage"

	Examples: 
	| fbAccount        |
	| hynrbf@gmail.com |

#Scenario Outline: Logging in to the app using facebook mobile number account
#	Given I am not authenticated
#		And I am on the page "LogonPage"
#	When I tap the Login with Facebook Button with mobile account "<fbMobileNumber>"
#	Then I am authenticated
#		And I am redirected to the page "PostFeedPage"
#
#	Examples: 
#	| fbMobileNumber |
#	| 09477691857    |
#
#Scenario: Navigating to SignUp Page
#	Given I am not authenticated
#		And I am on the page "LogonPage"
#	When I tap the Sign up link text
#	Then I am redirected to the page "SignUpPage"
#
#Scenario Outline: Logging in to the app using facebook email account via PostFeed Page but the account is not registered to the hopeph system
#	Given I am not authenticated
#		And I am on the page "LogonPage"
#	When I tap the Login with Facebook Button with account "<fbAccount>"
#	Then I should see not registered account message is displayed
#		And I am redirected to the page "SignUpPage"
#
#	Examples: 
#	| fbAccount			|
#	| hello@pangga.ph	|
