Feature: LogonPage
	
Scenario Outline: Logging in to the app using valid email but incorrect verification code
	Given I am not authenticated
		And I am on the page "LogonPage"
	When I tap on the Sign In button
	Then I am redirected to the page "RequestSigninVerificationCodePage"
	When I enter my email address "<emailAddress>" and tap on the submit button
	Then I am redirected to the page "ConfirmVerificationCodePage"
	When I enter verification code "<verificationCode>" and tap submit button
	Then I should see an error message "<errorMessage>"

	Examples: 
	| emailAddress           | verificationCode | errorMessage                           |
	| alfeo.salano@gmail.com | 2-2-2-2          | Please enter a valid verification code |

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
