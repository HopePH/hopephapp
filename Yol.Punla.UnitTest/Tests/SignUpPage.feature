Feature: SignUpPage

Scenario Outline: Signing up a new user
	Given I am not authenticated
		And I am on the page "LogonPage"
	When I tap the signup link below
	Then I am redirected to the page "EmailVerificationPage"
	When I enter my email address "<emailAdd>" and tap continue button
	Then the verification boxes appear
	When I type my verification code code-a "<code1>", code-b "<code2>", code-c "<code3>", code-d "<code4>", and tap the continue button
	Then I am redirected to the page "AccountRegistrationPage"
	#When I tap the change photo and select unicorn
	#Then I should see the photo is change to unicorn
	#When I enter my alias "<alias>" and mobile no "<mobileno>" and tap save button
	#Then I am redirected to the page "MainTabbedPage"

	Examples: 
	| code1 | code2 | code3 | code4 | emailAdd         | alias | mobileno   |
	| 1     | 1     | 1     | 1     | hynrbf@gmail.com | 0000  | 1212121221 |

Scenario Outline: Signing up a new user but with incorrect verfication code
	Given I am not authenticated
		And I am on the page "LogonPage"
	When I tap the signup link below
	Then I am redirected to the page "EmailVerificationPage"
	When I enter my email address "<emailAdd>" and tap continue button
	Then the verification boxes appear
	When I type my verification code code-a "<code1>", code-b "<code2>", code-c "<code3>", code-d "<code4>", and tap the continue button
	Then I should see an error message "<errorMessage>" in verification page

	Examples: 
	| code1 | code2 | code3 | code4 | emailAdd         | errorMessage							|
	| 1     | 2     | 1     | 3     | hynrbf@gmail.com | Please enter a valid verification code |