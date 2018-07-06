Feature: HomePage

Scenario Outline: Displaying the list of mental health care facilities
	Given I am not authenticated
		And I am on the page "WikiPage"
	When I tap the hamburger icon
	Then I should see the menu detail is opened
	When I tap the Mental Care Facilities icon from the menu detail
	Then I am redirected to the page "HomePage"
		And I should see one of the mental health facility "HRMC Medical and Rehabilitation Foundation Center Inc."

	Examples:
	| name                    | address                                            | photoUrl                                                                                        | telNo     | latitude  | longitude   |
	| New Day Recovery Center | Babista Compd., Beach Club Rd., Lanang, Davao City | https://yolpunlastorage.blob.core.windows.net/yolpunlacontainer/RBF/Contact.Photo/nanaydrug.jpg | 026226874 | 7.1047938 | 125.6442052 |

Scenario: Displaying the list of mental health care facilities when offline
	Given I am not authenticated
		And I am on the page "WikiPage"
	When I tap the hamburger icon
	Then I should see the menu detail is opened
	When I tap the Mental Care Facilities icon from the menu detail but the internet was off
	Then I should see the list of mental health care
		And I should see one of the mental health facility "HRMC Medical and Rehabilitation Foundation Center Inc."

Scenario: Navigating to the Wiki Page
	Given I am not authenticated
		And I am on the page "WikiPage"
		And the menu detail is closed
	When I tap the hamburger icon
	Then I should see the menu detail is opened
	When I tap the Useful Stuff item from the menu detail
	Then I am redirected to the page "WikiPage"
		And I should see a list of Wikis and the wiki with title "Depression in Philippines"
		And I shouid see the ForceToVersionNo in one of the items in wiki

Scenario Outline: Navigating to the mental care details
	Given I am not authenticated
		And I am on the page "WikiPage"
	When I tap the hamburger icon
	Then I should see the menu detail is opened
	When I tap the Mental Care Facilities icon from the menu detail
	Then I am redirected to the page "HomePage"
		And I should see the list of mental health care
		And I should see one of the mental health facility "HRMC Medical and Rehabilitation Foundation Center Inc."
	When I tap the mental care name "<name>", address "<address>", tel no "<telNo>" photo url "<photoUrl>", latitude "<latitude>" and longitude "<longitude>"
	Then I am redirected to the page "MentalCareDetailsPage"
		And I should see the name "<name>", address "<address>", tel no "<telNo>" and photo url "<photoUrl>" is displayed

	Examples:
	| name                    | address                                            | photoUrl                                                                                        | telNo     | latitude  | longitude   |
	| New Day Recovery Center | Babista Compd., Beach Club Rd., Lanang, Davao City | https://yolpunlastorage.blob.core.windows.net/yolpunlacontainer/RBF/Contact.Photo/nanaydrug.jpg | 026226874 | 7.1047938 | 125.6442052 |

Scenario: Dialing the phone no of the mental care facility
	Given I am not authenticated
		And I am on the page "WikiPage"
	When I tap the hamburger icon
	Then I should see the menu detail is opened
	When I tap the Mental Care Facilities icon from the menu detail
	Then I am redirected to the page "HomePage"
		And I should see the list of mental health care
		And I should see one of the mental health facility "HRMC Medical and Rehabilitation Foundation Center Inc."
	When I tap item "Nanay Drug and Alcohol Rehabilitation Center" Call button
	Then I should see the phone no dialed "0916 627 5132"

Scenario Outline: Viewing the Mental Health Facility in map
	Given I am not authenticated
		And I am on the page "WikiPage"
	When I tap the hamburger icon
	Then I should see the menu detail is opened
	When I tap the Mental Care Facilities icon from the menu detail
	Then I am redirected to the page "HomePage"
		And I should see the list of mental health care
		And I should see one of the mental health facility "HRMC Medical and Rehabilitation Foundation Center Inc."
	When I tap the mental care name "<name>", address "<address>", tel no "<telNo>" photo url "<photoUrl>", latitude "<latitude>" and longitude "<longitude>"
	Then I am redirected to the page "MentalCareDetailsPage"
		And I should see the name "<name>", address "<address>", tel no "<telNo>" and photo url "<photoUrl>" is displayed
		And I should see that the google map app has not launched yet
	When I tap the View Map Button
	Then I am redirected to the google map with pin on the facility's location

	Examples:
	| name                    | address                                            | photoUrl                                                                                        | telNo     | latitude  | longitude   |
	| New Day Recovery Center | Babista Compd., Beach Club Rd., Lanang, Davao City | https://yolpunlastorage.blob.core.windows.net/yolpunlacontainer/RBF/Contact.Photo/nanaydrug.jpg | 026226874 | 7.1047938 | 125.6442052 |

Scenario: Filtering Mental Care Facilities then close it
	Given I am not authenticated
		And I am on the page "WikiPage"
	When I tap the hamburger icon
	Then I should see the menu detail is opened
	When I tap the Mental Care Facilities icon from the menu detail
	Then I am redirected to the page "HomePage"
		And I should see the list of mental health care
		And I should see one of the mental health facility "HRMC Medical and Rehabilitation Foundation Center Inc."
	When I tap the Filter tab above the list
	Then I should see the filter modal appear
	When I tap the close icon of the filter modal
	Then the filter modal should disapear

Scenario: Sorting Mental Care Facilities then close it
	Given I am not authenticated
		And I am on the page "WikiPage"
	When I tap the hamburger icon
	Then I should see the menu detail is opened
	When I tap the Mental Care Facilities icon from the menu detail
	Then I am redirected to the page "HomePage"
		And I should see the list of mental health care
		And I should see one of the mental health facility "HRMC Medical and Rehabilitation Foundation Center Inc."
	When I tap the Sort tab above the list
	Then I should see the sort modal appear
	When I tap the close icon of the sort modal
	Then the sort modal should disapear

Scenario Outline: Closing the Alpha Advertisement from the first load of the app
	Given I am not authenticated
		And I am on the page "WikiPage"
		And the menu detail is closed
		And I should see the Alpa Adverstisement is displayed
	When I tap the close Alpha Advertisement
	Then I should see the Alpha Advertisement is closed
	#When I tap the mental care name "<name>", address "<address>", tel no "<telNo>" photo url "<photoUrl>", latitude "<latitude>" and longitude "<longitude>"
	#Then I am redirected to the page "MentalCareDetailsPage"
	#	And I should see the name "<name>", address "<address>", tel no "<telNo>" and photo url "<photoUrl>" is displayed
	#When I tap the back icon from Mental Care Details
	#Then I am redirected to the page "HomePage"
	#	And I should see the Alpa Adverstisement is not displayed

	Examples:
	| name                    | address                                            | photoUrl                                                                                        | telNo     | latitude  | longitude   |
	| New Day Recovery Center | Babista Compd., Beach Club Rd., Lanang, Davao City | https://yolpunlastorage.blob.core.windows.net/yolpunlacontainer/RBF/Contact.Photo/nanaydrug.jpg | 026226874 | 7.1047938 | 125.6442052 |

Scenario: Writing down the thoughts when not authenticated and not signedup
	Given I am not authenticated
		And I am on the page "WikiPage"
		And the menu detail is closed
	When I tap the hamburger icon
	Then I should see the menu detail is opened
	When I tap the Write Down icon from the menu detail with authenticated "false" and signed up "false"
	Then I should see a message saying the user to sign up "When logging in your profile will still remain anonymous to the other users."
		And I am redirected to the page "LogonPage"

Scenario: Writing down the thoughts when not authenticated and but was signup already
	Given I am not authenticated
		And I am on the page "WikiPage"
		And the menu detail is closed
	When I tap the hamburger icon
	Then I should see the menu detail is opened
	When I tap the Write Down icon from the menu detail with authenticated "false" and signed up "true"
	Then I am redirected to the page "LogonPage"

Scenario: Writing down the thoughts when authenticated and has signedup already
	Given I am not authenticated
		And I am on the page "WikiPage"
		And the menu detail is closed
	When I tap the hamburger icon
	Then I should see the menu detail is opened
	When I tap the Write Down icon from the menu detail with authenticated "true" and signed up "true"
	Then I am redirected to the page "PostFeedPage"

Scenario: Refeshing the mental care facilities from the server again
	Given I am not authenticated
		And I am on the page "WikiPage"
	When I tap the hamburger icon
	Then I should see the menu detail is opened
	When I tap the Mental Care Facilities icon from the menu detail
	Then I am redirected to the page "HomePage"
		And I should see one of the mental health facility "HRMC Medical and Rehabilitation Foundation Center Inc."
	When I pull to refresh the mental care list
	Then I should see the list of mental health care coming from gateway
		And I should see one of the mental health facility "HRMC Medical and Rehabilitation Foundation Center Inc."

Scenario: Filtering Mental Care Facilities by proximity
	Given I am not authenticated
		And I am on the page "WikiPage"
	When I tap the hamburger icon
	Then I should see the menu detail is opened
	When I tap the Mental Care Facilities icon from the menu detail
	Then I am redirected to the page "HomePage"
		And I should see the list of mental health care
		And I should see one of the mental health facility "HRMC Medical and Rehabilitation Foundation Center Inc."
	When I tap the Filter tab above the list
	Then I should see the filter modal appear
	When I choose the radius km "1" with the current position latitude "14.572372", longitude "121.054156" and tap the filter button
	Then I should see that there are "2" mental care is displayed within the proximity

Scenario: Sorting Mental Care Facilities by proximity
	Given I am not authenticated
		And I am on the page "WikiPage"
	When I tap the hamburger icon
	Then I should see the menu detail is opened
	When I tap the Mental Care Facilities icon from the menu detail
	Then I am redirected to the page "HomePage"
		And I should see the list of mental health care
		And I should see one of the mental health facility "HRMC Medical and Rehabilitation Foundation Center Inc."
		And I should see the first item mental care name "HRMC Medical and Rehabilitation Foundation Center Inc."
	When I tap the Sort tab above the list
	Then I should see the sort modal appear
	When I select the sort by location and tap the sort button
	Then I should see the first item mental care name "Estrellas Home Care Clinic & Hospital Inc."

Scenario: Sorting Mental Care Facilities without choosing any inputs
	Given I am not authenticated
		And I am on the page "WikiPage"
	When I tap the hamburger icon
	Then I should see the menu detail is opened
	When I tap the Mental Care Facilities icon from the menu detail
	Then I am redirected to the page "HomePage"
		And I should see the list of mental health care
		And I should see one of the mental health facility "HRMC Medical and Rehabilitation Foundation Center Inc."
		And I should see the first item mental care name "HRMC Medical and Rehabilitation Foundation Center Inc."
	When I tap the Sort tab above the list
	Then I should see the sort modal appear
	When I tap the sort button in mental facility page
	Then I should see the mental care error message "Please select a sort item."

