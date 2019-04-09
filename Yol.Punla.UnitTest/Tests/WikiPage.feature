Feature: WikiPage
	
Background: 
	Given I am not authenticated
		And I am on the page "WikiPage"
		And the menu detail is closed
	When I tap the hamburger icon
	Then I should see the menu detail is opened
	When I tap the Useful Stuff item from the menu detail
	Then I am redirected to the page "WikiPage"
		And I should see a list of Wikis and the wiki with title "Depression in Philippines"

Scenario Outline: Navigating to WikiDetailsPage and Back
	Given I am on the page "WikiPage"
		And I can see a list of Wikis and the wiki with title "Depression in Philippines"
	When I tap the item with title "<title>"
	Then I am redirected to the page "WikiDetailsPage"
		And I can see the title of the wiki "<wikiTitle>" with content "<content>"
	When I tap the back icon from WikiDetailsPage
	Then I am redirected to the page "WikiPage"
		And I should see a list of Wikis and the wiki with title "Depression in Philippines"

	Examples: 
	| title                     | wikiTitle                 | content                                       |
	| Depression in Philippines | Depression in Philippines | Mental health law to help fight illegal drugs |
			
Scenario:Tapping the Sort Modal
	Given I am on the page "WikiPage"
		And I can see a list of Wikis and the wiki with title "Depression in Philippines"
	When I tap the Sort tab above the wiki list
	Then I should see the wiki sort modal appear
	When I tap the close icon of the wiki sort modal
	Then the wiki sort modal should disappear

Scenario:Tapping the Filter Modal
	Given I am on the page "WikiPage"
		And I can see a list of Wikis and the wiki with title "Depression in Philippines"
	When I tap the Filter tab above the wiki list
	Then I should see the wiki filter modal appear
	When I tap the close icon of the wiki filter modal
	Then the wiki filter modal should disappear

Scenario: Sorting the wiki page alphabetically
	Given I am on the page "WikiPage"
		And I could see the first item title "Depression in Philippines" 
		And I could see that the wiki sort dialog is not displayed
	When I tap the sort button at the top in the wiki page
	Then I should see the wiki sort dialog is displayed
	When I choose the option alphabetically and tap the sort button
	Then I should see the first item title "1 out of 5 Filipinos suffer from depression"

Scenario: Sorting the wiki page alphabetically but didnt select the alphabetical input
	Given I am on the page "WikiPage"
		And I could see the first item title "Depression in Philippines" 
		And I could see that the wiki sort dialog is not displayed
	When I tap the sort button at the top in the wiki page
	Then I should see the wiki sort dialog is displayed
	When I choose the tap the sort button without choosing alphabetical input
	Then I should see the wiki page alert error "Please select a sort item."

#chito.temp the error is this When I tap the back icon from WikiDetailsPage and the app force to download to version no was updated to "1.10" from server	
#when you hit navigateback command the previous page is not hit but in real run it is.
#the reason for this error is,
#when you update the Prism to higher version back issue will be fixed but the 2 issues below will come out
#a.new prism does not support Inavigationservice in net framework project
#b.net standard not support specflow
#so need to wait for specflow to support net standard soon :(
Scenario Outline: Forcing a user to download a new version of the app
	Given I am on the page "WikiPage"
		And I can see a list of Wikis and the wiki with title "Depression in Philippines"
		And I could see the ad message "<defaultAddMessage>"
		And I could see the current app version no "1.9"
	When I tap the item with title "<title>"
	Then I am redirected to the page "WikiDetailsPage"
		And I can see the title of the wiki "<wikiTitle>" with content "<content>"
	When I tap the back icon from WikiDetailsPage and the app force to download to version no was updated to "1.10" from server	
	Then I am redirected to the page "WikiPage"
		And I should see the pop message "<newAddMessage>"

	Examples: 
	| title                     | wikiTitle                 | content                                       | defaultAddMessage  | newAddMessage                                                           |
	| Depression in Philippines | Depression in Philippines | Mental health law to help fight illegal drugs | Welcome to HopePH. | Sorry for the inconvenience but please download the latest version now. |

