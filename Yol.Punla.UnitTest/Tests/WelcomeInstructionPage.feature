Feature: WelcomeInstructionPage
	This is the first page the user will be 
	navigated to when he / she run the app
	for the first time.

	Running again the app after that will not
	navigate the user to the WelcomeInstructionPage
	again.

@tag WelcomeInstructionIsNotLoadedYet
Scenario: Running the app for the first time
	Given I am not authenticated
		And I am on the page "WelcomeInstructionsPage"
	Then I should see the back arrow of the page grayed
		And I should see the instruction text "You could post your feelings anonymously. When you post only your alias is shown."

@tag WelcomeInstructionIsNotLoadedYet
Scenario: Navigating to the second page of the instructions page
	Given I am on the page "WelcomeInstructionsPage"
		And I could see the instruction text "You could post your feelings anonymously. When you post only your alias is shown."
		And the back button is grayed
	When I tap the forward arrow
	Then I should see the instruction text "If you wish to see a local psychologist or psychiatrist, you could browse the mental care facilities section. You could sort them by location and, you could also call their tel nos straight away."
		And I should see the back button is enabled

@tag WelcomeInstructionIsNotLoadedYet
Scenario: Navigating back to the previous instruction page from last instruction
	Given I am on the page "WelcomeInstructionsPage"
		And I could see the instruction text "You could post your feelings anonymously. When you post only your alias is shown."
		And I should see the back arrow of the page grayed
	When I tap the forward arrow three times
	Then  I should see the instruction text "Signing up with facebook or signing up with aliasname only are provided. Your privacy and data security is our top priority."
	When I tap the back arrow
	Then I should see the instruction text "If you are in crisis or your family or friends, you can browse to the directory of crisis providers section. You could easily tap and call their tel nos."
		And I should see the back button is enabled

@tag WelcomeInstructionIsNotLoadedYet
Scenario: Navigating back to the previous instruction page from third instruction
	Given I am on the page "WelcomeInstructionsPage"
		And I could see the instruction text "You could post your feelings anonymously. When you post only your alias is shown."
		And I should see the back arrow of the page grayed
	When I tap the forward arrow two times
	Then  I should see the instruction text "If you are in crisis or your family or friends, you can browse to the directory of crisis providers section. You could easily tap and call their tel nos."
	When I tap the back arrow
	Then I should see the instruction text "If you wish to see a local psychologist or psychiatrist, you could browse the mental care facilities section. You could sort them by location and, you could also call their tel nos straight away."
		And I should see the back button is enabled

@tag WelcomeInstructionIsNotLoadedYet
Scenario: Navigating back to the previous instruction page from second instruction
	Given I am on the page "WelcomeInstructionsPage"
		And I could see the instruction text "You could post your feelings anonymously. When you post only your alias is shown."
		And I should see the back arrow of the page grayed
	When I tap the forward arrow
	Then  I should see the instruction text "If you wish to see a local psychologist or psychiatrist, you could browse the mental care facilities section. You could sort them by location and, you could also call their tel nos straight away."
	When I tap the back arrow
	Then I should see the instruction text "You could post your feelings anonymously. When you post only your alias is shown."
		And I should see the back arrow of the page grayed

@tag WelcomeInstructionIsNotLoadedYet
Scenario: Navigating to WikiPage via tapping the forward arrow in the last instruction text
	Given I am on the page "WelcomeInstructionsPage"
		And I could see the instruction text "You could post your feelings anonymously. When you post only your alias is shown."
		And I should see the back arrow of the page grayed
	When I tap the forward arrow three times
	Then  I should see the instruction text "Signing up with facebook or signing up with aliasname only are provided. Your privacy and data security is our top priority."
		And I should see the back button is enabled
	When I tap the forward arrow
	Then I am redirected to the page "WikiPage"