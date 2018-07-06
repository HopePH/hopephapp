Feature: AccountRegistrationPage

Background: 
	Given I am not authenticated
		And I am on the page "LogonPage"
	When I tap the Sign up link text
	Then I am redirected to the page "SignUpPage"
	When I tap the Sign up with Facebook Button using facebook email account "robertjlima38@gmail.com"
	Then I am authenticated
		And I am redirected to the page "AccountRegisrationPage"

