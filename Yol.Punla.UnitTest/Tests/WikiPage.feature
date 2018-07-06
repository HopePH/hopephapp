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
	| title                                         | wikiTitle                                              | content                                                                                                                                         |
	| Depression in Philippines                     | <h3>Depression in Philippines</h3>                     | <html><body style='background-color:#F5F5F5'><h3>Depression in Philippines</h3><p>We are top 1 in Asia in terms of depression</p></body></html> |
	| Mental health law to help fight illegal drugs | <h3>Mental health law to help fight illegal drugs</h3> | <html><body style='background-color:#F5F5F5'><h3>Mental health law to help fight illegal drugs</h3><p>Senator Risa Hontiveros believes passing a Mental Health Law would also help the country fight illegal drugs.</p><p>Hontiveros said illegal drugs, which also deals with addiction, is not only a law enforcement issue but also a public health issue. &quot;<i>We should look at it also as a public health problem, hindi lang law enforcement. At sa ilalim niyan, mental health problem nga,&quot;</i> she told ANC on Monday.</p><p>The senator said the <i>Mental Health Bill</i>, which was recently passed on third reading at the Senate, will help patients become fully-functioning members of the society again. According to Hontiveros, mental health remains a concern as data show 7 Filipinos commit suicide everyday. She added that people with mental health concerns also suffer stigma from society.</p><p>Under the <i>Philippine Mental Health Act of 2017</i>, she said, mental health will be integrated into the school curriculum to reduce discrimination of patients. Hontiveros added that the Department of Health is mandated to increase mental health professionals in the country under the proposed measure.<i>&quot;May isang pag-aaral na half of all adult Filipinos consulting health professionals in rural areas, kalahati, may nade-detect na mental health problem dahil sa kakulangan ng pagkakataon para magpa-checkup,&quot;</i> she said.</p><p>Hontiveros hopes the House of Representatives will be able to pass its version of the Mental Health Bill this year.</p></body></html> |
			
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
	| title                     | wikiTitle                          | content                                                                                                                                         | defaultAddMessage				| newAddMessage																|
	| Depression in Philippines | <h3>Depression in Philippines</h3> | <html><body style='background-color:#F5F5F5'><h3>Depression in Philippines</h3><p>We are top 1 in Asia in terms of depression</p></body></html> | Welcome to HopePH.	| Sorry for the inconvenience but please download the latest version now.	|

