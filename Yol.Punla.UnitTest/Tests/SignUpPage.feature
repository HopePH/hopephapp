Feature: SignUpPage

Scenario: Add two numbers
	Given I have entered 50 into the calculator
	And I have entered 70 into the calculator
	When I press add
	Then the result should be 120 on the screen

Scenario Outline: Signing up a new user
	Given I am not authenticated
		And I am on the page "LoginPage"
	When I tap the signup link below
	Then I am redirected to the page "EmailVerificationPage"
	When I enter my email address "<emailAdd>" and tap continue button
	Then the verification boxes appear
	When I type my verification code code-a "<code1>", code-b "<code2>", code-c "<code3>", code-d "<code4>", and tap the continue button
	Then I am redirected to the page "AccountRegistrationPage"
	When I enter my alias and mobile no and tap save button
	Then I am redirected to the page "MainTabbedPage"

	Examples: 
	| code1 | code2 | code3 | code4 | emailAdd         |
	| 1     | 1     | 1     | 1     | hynrbf@gmail.com |

Scenario Outline: Signing up a new user but with incorrect verfication code
	Given I am not authenticated
		And I am on the page "LoginPage"
	When I tap the signup link below
	Then I am redirected to the page "EmailVerificationPage"
	When I enter my email address "<emailAdd>" and tap continue button
	Then the verification boxes appear
	When I type my verification code code-a "<code1>", code-b "<code2>", code-c "<code3>", code-d "<code4>", and tap the continue button
	Then I should see an error message "<errorMessage>" in verification page

	Examples: 
	| code1 | code2 | code3 | code4 | emailAdd         | errorMessage  |
	| 1     | 2     | 1     | 3     | hynrbf@gmail.com | error message |