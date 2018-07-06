Feature: CrisisHotlineListPage

Scenario: Navigating to the Crisis Page not yet authenticated
	Given I am not authenticated
		And I am on the page "WikiPage"
		And the menu detail is closed
	When I tap the hamburger icon
	Then I should see the menu detail is opened
	When I tap the Crisis icon from the menu detail
	Then I should see a message saying the user to sign up "When logging in your profile will still remain anonymous to the other users."
		And I am redirected to the page "LogonPage"

Scenario: Dialing the phone number from the Crisis Page not yet authenticated
	Given I am not authenticated
		And I am on the page "WikiPage"
		And the menu detail is closed
	When I tap the hamburger icon
	Then I should see the menu detail is opened
	When I tap the Crisis icon from the menu detail
	Then I should see a message saying the user to sign up "When logging in your profile will still remain anonymous to the other users."
		And I am redirected to the page "LogonPage"
		
@tag FacebookLogonViaCrisisMenu
Scenario: Navigating to the Crisis Page authenticated
	Given I am authenticated
		And I am on the page "CrisisHotlineListPage"
	Then I should see "028044673" as one of the crisis hotline

@tag FacebookLogonViaCrisisMenu
Scenario: Dialing the phone number from the Crisis Page authenticated
	Given I am authenticated
		And I am on the page "CrisisHotlineListPage"
		And I should see "028044673" as one of the crisis hotline
	When I dial the hotline no "028044673"
	Then I should see the number "028044673" was added to the dialer