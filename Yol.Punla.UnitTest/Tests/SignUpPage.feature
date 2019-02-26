Feature: SignUpPage

Scenario: Add two numbers
	Given I have entered 50 into the calculator
	And I have entered 70 into the calculator
	When I press add
	Then the result should be 120 on the screen

#Background: 
#	Given I am not authenticated
#		And I am on the page "WikiPage"
#		And the menu detail is closed
#	When I tap the hamburger icon
#	Then I should see the menu detail is opened
#	When I tap the Write Down icon from the menu detail with authenticated "false" and signed up "false"
#	Then I should see a message saying the user to sign up "When logging in your profile will still remain anonymous to the other users."
#		And I am redirected to the page "LogonPage"
#
#Scenario Outline: Signing up with complete inputs and using the fb test account
#	Given I am not authenticated
#		And I am on the page "LogonPage"
#	When I tap the Sign up link text
#	Then I am redirected to the page "SignUpPage"
#	When I tap the Sign up with Facebook Button using facebook email account "<fbEmail>"
#	Then I am redirected to the page "AccountRegistrationPage"
#		And I can see the fullname "<fullname>", email "<fbEmail>", alias "<alias>", mobile phone "<mobilePhone>" and Photo "<photoURL>" values of the fields
#		And I could see my default avatar picture "<defaultPicture>"
#	When I tap the Change Photo button
#	Then I should see the list of avatar images to choose from
#	When I tap on the avatar image "<picture>" that I choose
#	Then I should see that my default picture was changed to "<picture>"
#	When I enter email address "<fbEmail>", alias "<alias>", and mobile no "<mobilePhone>" I tap the Save and Continue button
#	Then I am authenticated
#		And I am redirected to the page "PostFeedPage"
#
#	Examples: 
#	| fbEmail          | fullname     | alias  | mobilePhone | photoURL                                                                                    | picture                                                                                      | defaultPicture                                                                                  |
#	| hynrbf@gmail.com | Chito Salano | Chito1 | 026500987   | https://yolpunlastorage.blob.core.windows.net/yolpunlacontainer/RBF/Contact.Photo/alfeo.jpg | https://yolpunlastorage.blob.core.windows.net/yolpunlacontainer/RBF/Contact.Photo/rabbit.png | https://yolpunlastorage.blob.core.windows.net/yolpunlacontainer/RBF/Contact.Photo/manavatar.png |
#
#Scenario Outline: Signing up using the fb test account but without alias
#	Given I am not authenticated
#		And I am on the page "LogonPage"
#	When I tap the Sign up link text
#	Then I am redirected to the page "SignUpPage"
#	When I tap the Sign up with Facebook Button using facebook email account "<fbEmail>"
#	Then I am redirected to the page "AccountRegistrationPage"
#	When I enter email address "<fbEmail>", alias "<alias>", and mobile no "<mobilePhone>" I tap the Save and Continue button
#	Then I should stay on the same page "AccountRegistrationPage"
#		And I should see registration error message "Please enter your alias."
#
#	Examples: 
#	| fbEmail                | fullname		| alias		| mobilePhone	| photoURL                                                                                    |
#	| hynrbf@gmail.com		 | Chito Salano	|			| 026500987		| https://yolpunlastorage.blob.core.windows.net/yolpunlacontainer/RBF/Contact.Photo/alfeo.jpg |
#
#Scenario Outline: Signing up using the fb test account but without mobile no
#	Given I am not authenticated
#		And I am on the page "LogonPage"
#	When I tap the Sign up link text
#	Then I am redirected to the page "SignUpPage"
#	When I tap the Sign up with Facebook Button using facebook email account "<fbEmail>"
#	Then I am redirected to the page "AccountRegistrationPage"
#	When I enter email address "<fbEmail>", alias "<alias>", and mobile no "<mobilePhone>" I tap the Save and Continue button
#	Then I should stay on the same page "AccountRegistrationPage"
#		And I should see registration error message "Please enter your mobile no."
#
#	Examples: 
#	| fbEmail                | fullname		| alias		| mobilePhone	| photoURL                                                                                    |
#	| hynrbf@gmail.com		 | Chito Salano	| Chito1	|				| https://yolpunlastorage.blob.core.windows.net/yolpunlacontainer/RBF/Contact.Photo/alfeo.jpg |
#
#Scenario Outline: Signing up using the fb test account but already signed up previously
#	Given I am not authenticated
#		And I am on the page "LogonPage"
#	When I tap the Sign up link text
#	Then I am redirected to the page "SignUpPage"
#	When I tap the Sign up with Facebook Button using facebook mobile no "<fbMobileNo>" and I tap the Save and Continue button
#	Then I should see a duplicate pop message is displayed
#		And I am redirected to the page "LogonPage" 
#
#	Examples: 
#	| fbMobileNo  |
#	| 09477691857 |
#
#Scenario Outline: Signing up with complete inputs and changing the avatar photo
#	Given I am not authenticated
#		And I am on the page "LogonPage"
#	When I tap the Sign up link text
#	Then I am redirected to the page "SignUpPage"
#	When I tap the Sign up with Facebook Button using facebook email account "<fbEmail>"
#	Then I am redirected to the page "AccountRegistrationPage"
#		And I can see the fullname "<fullname>", email "<fbEmail>", alias "<alias>", mobile phone "<mobilePhone>" and Photo "<photoURL>" values of the fields
#	When I tap on the ChangePhoto button
#	Then I should see the popup that displays the list of avatar
#	When I choose the chicken avatar "Chicken"
#	Then I should see that the account photo was changed to "<newPhotoURL>"	
#	When I enter email address "<fbEmail>", alias "<alias>", and mobile no "<mobilePhone>" I tap the Save and Continue button
#	Then I am authenticated
#		And I am redirected to the page "PostFeedPage"
#
#	Examples: 
#	| fbEmail          | fullname     | alias  | mobilePhone | photoURL                                                                                    | newPhotoURL																					|
#	| hynrbf@gmail.com | Chito Salano | Chito1 | 026500987   | https://yolpunlastorage.blob.core.windows.net/yolpunlacontainer/RBF/Contact.Photo/alfeo.jpg | https://yolpunlastorage.blob.core.windows.net/yolpunlacontainer/RBF/Contact.Photo/chicken.png	|
#
#Scenario Outline: Signing up using the fb test account but without changing the default picture
#	Given I am not authenticated
#		And I am on the page "LogonPage"
#	When I tap the Sign up link text
#	Then I am redirected to the page "SignUpPage"
#	When I tap the Sign up with Facebook Button using facebook email account "<fbEmail>"
#	Then I am redirected to the page "AccountRegistrationPage"
#	When I enter email address "<fbEmail>", alias "<alias>", and mobile no "<mobilePhone>" I tap the Save and Continue button
#	Then I should stay on the same page "AccountRegistrationPage"
#		And I should see registration error message "Please change your 'Photo'."
#
#	Examples: 
#	| fbEmail                | fullname		| alias		| mobilePhone	| photoURL                                                                                    |
#	| hynrbf@gmail.com		 | Chito Salano	| Chito1	| 026500987		| https://yolpunlastorage.blob.core.windows.net/yolpunlacontainer/RBF/Contact.Photo/alfeo.jpg |


