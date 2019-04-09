Feature: RequestVerificationCodePage
	This is the alternative workflow for signing in
	which uses your email to receive a verification 
	code which you will confirm to sign in using your
	alias

Background: 
	Given I am not authenticated
		And I am on the page "WikiPage"
		And the menu detail is closed
	When I tap the hamburger icon
	Then I should see the menu detail is opened
	When I tap the Write Down icon from the menu detail with authenticated "false" and signed up "false"
	Then I should see a message saying the user to sign up "When logging in your profile will still remain anonymous to the other users."
		And I am redirected to the page "LogonPage"
	When I tap the Alias link text in sign in with alias label
	Then I am redirected to the page "RequestSigninVerificationCodePage"
		And I should see that the email address field is empty

Scenario Outline: Signing in using Verfication code but Email field is not supplied or has invalid data
	Given I am on the page "RequestSigninVerificationCodePage"
	When I type my email "<email>" in the email field and tap the send verification code button
	Then I should see an error message "<errorMsg>" in RequestSigninVerificationCodePage

	Examples: 
	| email           | errorMsg                            |
	| momoyfailed.com | Please enter a valid email address. |
	|                 | Please enter a valid email address. |
	| momoy@gmail     | Please enter a valid email address. |

Scenario Outline: Signing in using Verfication code by entering the correct verification code sent to the email
	Given I am on the page "RequestSigninVerificationCodePage"
		And the verification code is null
	When I type my email "<email>" in the email field and tap the send verification code button
	Then I am redirected to the page "ConfirmVerificationCodePage"
		And I should receive a verification code "<code>" sent to my email
		And I should see that the verification code field is empty
	When I type the verification code "<code>" and tap the submit code button
	Then I am redirected to the page "PostFeedPage"

	Examples: 
	| email                  | code |
	| alfeo.salano@gmail.com | 1111 |

Scenario Outline: Signing in using Verfication code but verification is incorrect or not supplied
	Given I am on the page "RequestSigninVerificationCodePage"
		And the verification code is null
	When I type my email "<email>" in the email field and tap the send verification code button
	Then I am redirected to the page "ConfirmVerificationCodePage"
		And I should receive a verification code "<code>" sent to my email
		And I should see that the verification code field is empty
	When I type the verification code "<code>" and tap the submit code button
	Then I should see an error message "<errorMessage>" in the ConfirmVerificationCodePage
		And I should stay on the same page "ConfirmVerificationCodePage"

	Examples: 
	| email                  | code | errorMessage                           |
	| alfeo.salano@gmail.com | 1112 | Please enter a valid verification code |
	| alfeo.salano@gmail.com |      | Please enter a valid verification code |
