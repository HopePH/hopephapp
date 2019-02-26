Feature: EmailVerificationPage
	This is another alternative in signing up to the app.
	This will require the user to enter email and confirm
	the verification code sent to their email.

Background: 
	Given I am not authenticated
		And I am on the page "WikiPage"
		And the menu detail is closed
	When I tap the hamburger icon
	Then I should see the menu detail is opened
	When I tap the Write Down icon from the menu detail with authenticated "false" and signed up "false"
	Then I should see a message saying the user to sign up "When logging in your profile will still remain anonymous to the other users."
		And I am redirected to the page "LogonPage"
	When I tap the Sign up link text
	Then I am redirected to the page "SignUpPage"
	When I tap the Verify Email Button
	Then I am redirected to the page "EmailVerificationPage"

Scenario Outline: Signing up via email verification and entering valid email and requesting verification code
	Given I am on the page "EmailVerificationPage"
		And I could see the Email and verification code entries are empty
	When I type the email "<email>" and tap send verification code button
	Then I should get a verification code sent to my email
	When I type the verification code "<typedVerificationCode>" and tap the submit button using the email "<email>" provided
	Then I am redirected to the page "AccountRegistrationPage"
		And I could see the Fullname "<fullname>"
		And I could see the email "<email>"
		And I could see my default avatar picture "<defaultPicture>"

	Examples: 
	| email            | typedVerificationCode | fullname			| defaultPicture                                                                                  |
	| momoy@gmail.com | 1111                  | Undisclosed Name	| https://yolpunlastorage.blob.core.windows.net/yolpunlacontainer/RBF/Contact.Photo/manavatar.png |

Scenario Outline: Signing up via email verification but entering incorrect verification code
	Given I am on the page "EmailVerificationPage"
		And I could see the Email and verification code entries are empty
	When I type the email "<email>" and tap send verification code button
	Then I should get a verification code sent to my email
	When I type the verification code "<typedVerificationCode>" and tap the submit button using the email "<email>" provided
	Then I should see an error message "<errorMsg>"

	Examples: 
	| errorMsg                               | email            | typedVerificationCode |
	| Please enter a valid verification code | momoy@gmail.com | 1112                  |

Scenario Outline: Signing up via email verification but using an email that exists already
	Given I am on the page "EmailVerificationPage"
		And I could see the Email and verification code entries are empty
	When I type the email "<email>" and tap send verification code button
	Then I should see an error message "<errorMsg>"

	Examples: 
	| errorMsg                  | email            | typedVerificationCode |
	| The email already exists. | hynrbf@gmail.com | 1112                  |

Scenario: Submitting request for verification code when there is no email supplied
	Given I am on the page "EmailVerificationPage"
		And I could see the Email and verification code entries are empty
	When I tap send verification code button
	Then I should see an error message "Please enter a valid email address."

Scenario Outline: Signing up via email verification but without alias name
	Given I am on the page "EmailVerificationPage"
		And I could see the Email and verification code entries are empty
	When I type the email "<email>" and tap send verification code button
	Then I should get a verification code sent to my email
	When I type the verification code "1111" and tap the submit button using the email "<email>" provided
	Then I am redirected to the page "AccountRegistrationPage"
		And I could see the Fullname "<fullName>"
		And I could see the email "<email>"
		And I could see my default avatar picture "<defaultPicture>"
	When I enter email address "<email>", alias "<alias>", and mobile no "<mobilePhone>" I tap the Save and Continue button
	Then I should stay on the same page "AccountRegistrationPage"
		And I should see registration error message "Please enter your alias."

	Examples: 
	| email            | alias | mobilePhone | defaultPicture                                                                                  | fullName         |
	| momoy@gmail.com |       | 026500987   | https://yolpunlastorage.blob.core.windows.net/yolpunlacontainer/RBF/Contact.Photo/manavatar.png | Undisclosed Name |

Scenario Outline: Signing up via email verification and completing the signup process
	Given I am on the page "EmailVerificationPage"
		And I could see the Email and verification code entries are empty
	When I type the email "<email>" and tap send verification code button
	Then I should get a verification code sent to my email
	When I type the verification code "1111" and tap the submit button using the email "<email>" provided
	Then I am redirected to the page "AccountRegistrationPage"
		And I could see the Fullname "<fullName>"
		And I could see the email "<email>"
		And I could see my default avatar picture "<defaultPicture>"
	When I tap the Change Photo button
	Then I should see the list of avatar images to choose from
	When I tap on the avatar image "<picture>" that I choose
	Then I should see that my default picture was changed to "<picture>"
	When I enter email address "<email>", alias "<alias>", and mobile no "<mobilePhone>" I tap the Save and Continue button
	Then I am authenticated
		And I am redirected to the page "PostFeedPage"

	Examples: 
	| email            | alias | mobilePhone | picture                                                                                      | defaultPicture                                                                                  | fullName         |
	| momoy@gmail.com | Pitts | 026500987   | https://yolpunlastorage.blob.core.windows.net/yolpunlacontainer/RBF/Contact.Photo/rabbit.png | https://yolpunlastorage.blob.core.windows.net/yolpunlacontainer/RBF/Contact.Photo/manavatar.png | Undisclosed Name |

Scenario Outline: Signing up via email verification but without mobile phone
	Given I am on the page "EmailVerificationPage"
		And I could see the Email and verification code entries are empty
	When I type the email "<email>" and tap send verification code button
	Then I should get a verification code sent to my email
	When I type the verification code "1111" and tap the submit button using the email "<email>" provided
	Then I am redirected to the page "AccountRegistrationPage"
		And I could see the Fullname "<fullName>"
		And I could see the email "<email>"
		And I could see my default avatar picture "<defaultPicture>"
	When I tap the Change Photo button
	Then I should see the list of avatar images to choose from
	When I tap on the avatar image "<picture>" that I choose
	Then I should see that my default picture was changed to "<picture>"
	When I enter email address "<email>", alias "<alias>", and mobile no "<mobilePhone>" I tap the Save and Continue button
	Then I should stay on the same page "AccountRegistrationPage"
		And I should see registration error message "Please enter your mobile no."

	Examples: 
	| email            | alias | mobilePhone | picture                                                                                      | defaultPicture                                                                                  | fullName         |
	| momoy@gmail.com | Pitts |             | https://yolpunlastorage.blob.core.windows.net/yolpunlacontainer/RBF/Contact.Photo/rabbit.png | https://yolpunlastorage.blob.core.windows.net/yolpunlacontainer/RBF/Contact.Photo/manavatar.png | Undisclosed Name |