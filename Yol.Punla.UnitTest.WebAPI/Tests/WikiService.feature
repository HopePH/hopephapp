Feature: WikiService

Scenario: Calling the Http GET method of getting the wikis
	Given the wiki list is null
	When the client calls the GetWikis endpoint of company "HopePH"
	Then I should see the response code is OK
		And I should see the list of wiki
		And I shouid see the ForceToVersionNo in one of the items in wiki
