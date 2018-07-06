Feature: PostFeedPage

@tag FacebookLogonViaWriteYourThoughtsMenu
Scenario:Navigating to the PostFeedPage
	Given I am authenticated
		And I am on the page "PostFeedPage"
	Then I should see a Post posted by "Peter Smith", with title "Depression is something everyone should be aware of.", and content "According to @Glassdoor, these signs mean you're overthinking things at work:"

@tag FacebookLogonViaWriteYourThoughtsMenu
Scenario Outline: Navigating to PostFeedDetailsPage
	Given I am authenticated
		And I am on the page "PostFeedPage"
	When I tap the comment icon of the post of "<posterName>" with content "<content>" and postFeedId "<postFeedId>"
	Then I am redirected to the page "PostFeedDetailPage"
		And I can see the details of the post with author "<posterName>" and content "<content>"

	Examples: 
	| posterName  | content                                                                                         | postFeedId |
	| Peter Smith | According to @Glassdoor, these signs mean you're overthinking things at work:                   | 1111       |
	| Peter Smith | Mental health is now on the final reading in the Congress. Sen. Riza Hotiveros pushes the bill. | 1112       |

@tag FacebookLogonViaWriteYourThoughtsMenu
Scenario Outline: Opening and closing the PostOptionsModal
	Given I am authenticated
		And I am on the page "PostFeedPage"
	Then I should see a Post posted by "Peter Smith", with title "Depression is something everyone should be aware of.", and content "According to @Glassdoor, these signs mean you're overthinking things at work:"
	When I tap the ellipsis icon to show the post option modal at the left of the post of "<author>" with postFeedId "<postFeedId>"
	Then the PostOptions modal will appear
	When I tap the close icon of the PostOptions Modal
	Then I should see the PostOptions modal disappear

	Examples: 
	| author      | postFeedId |
	| Peter Smith | 1111       |

@tag FacebookLogonViaWriteYourThoughtsMenu
Scenario Outline: Editing a self post
	Given I am authenticated
		And I am on the page "PostFeedPage"
	Then I should see a Post posted by "<author>", with title "<title>", and content "<content>"
	When I tap the ellipsis icon to show the post option modal at the left of the post of "<author>" with postFeedId "<postFeedId>"
	Then the PostOptions modal will appear
	When I tap the Edit menu
	Then I am redirected to the page "PostFeedAddEditPage"
		And I can see that the button text is "Update"
	When I type edit the message to "<message>" and tap the update button
	Then I am redirected to the page "PostFeedPage"
		And I should see my updated post with content "<message>", and author "<author>" and post id "<postFeedId>"

	Examples: 
	| author     | title    | content          | postFeedId | message						|
	| Haiyan Rbf | New Post | Newly added Post | 1120       | This is a edited post myself	|

@tag FacebookLogonViaWriteYourThoughtsMenu
Scenario Outline: Deleting a self post
	Given I am authenticated
		And I am on the page "PostFeedPage"
	Then I should see a Post posted by "<author>", with title "<title>", and content "<content>"
	When I tap the ellipsis icon to show the post option modal at the left of the post of "<author>" with postFeedId "<postFeedId>"
	Then the PostOptions modal will appear 
	When I tap the Delete menu
	Then The delete post feed request should be sent to the hub
		And I should stay on the same page "PostFeedPage"

	Examples: 
	| author     | title    | content          | postFeedId |
	| Haiyan Rbf | New Post | Newly added Post | 1120       |

@tag FacebookLogonViaWriteYourThoughtsMenu
Scenario Outline: Navigating to PostFeedAddPage
	Given I am authenticated
		And I am on the page "PostFeedPage"
	When I tap the text Share an article, photo, video or idea 
	Then I am redirected to the page "PostFeedAddEditPage"
		And I can see the photo Url of the current user is "<photoUrl>"

	Examples: 
	| photoUrl                                                                                                              |
	| https://graph.facebook.com/168866520312631/picture?height=220&width=220&migration_overrides=%7Boctober_2012%3Atrue%7D |

@tag FacebookLogonViaWriteYourThoughtsMenu
Scenario: Tapping the close button at the left side of the navigation bar
	Given I am authenticated
		And I am on the page "PostFeedPage"
	When I tap the text Share an article, photo, video or idea 
	Then I am redirected to the page "PostFeedAddEditPage"
	When I tap the close icon at the top left corner of the screen
	Then I am redirected to the page "PostFeedPage"

@tag FacebookLogonViaWriteYourThoughtsMenu
Scenario Outline: Posting a new post
	Given I am authenticated
		And I am on the page "PostFeedPage"
	When I tap the text Share an article, photo, video or idea 
	Then I am redirected to the page "PostFeedAddEditPage"
		And I can see that the button text is "Post"
	When I type "<postMessage>" and tap the Post button
	Then I am redirected to the page "PostFeedPage"
		And I can see my newly posted postfeed with title "<title>", post message "<postMessage>" and my fullname "<fullName>"

	Examples: 
	| postMessage		| title					| fullName		|
	| I added this post	| New Post from @Worde5	| Worde Salinas	|

@tag FacebookLogonViaWriteYourThoughtsMenu
Scenario Outline: Posting a new post but with empty content
	Given I am authenticated
		And I am on the page "PostFeedPage"
	When I tap the text Share an article, photo, video or idea 
	Then I am redirected to the page "PostFeedAddEditPage"
		And I can see that the button text is "Post"
	When I type "<postMessage>" and tap the Post button
	Then I should see an empty post errror message "<errorMessage>"

	Examples: 
	| postMessage | title    | fullName | errorMessage                               |
	|             | New Post | hyn rbf  | Posting with empty content is not allowed. |

@tag FacebookLogonViaWriteYourThoughtsMenu 
Scenario: Showing the Pull Down to Refresh Instruction
	Given I am authenticated
		And I am on the page "PostFeedPage"
		And I could see the pull down to refresh instruction is displayed
	When I tap on the pull down to refresh instruction
	Then I should see that the instruction is not displayed
	When I tap the text Share an article, photo, video or idea 
	Then I am redirected to the page "PostFeedAddEditPage"
	When I tap the close icon at the top left corner of the screen
	Then I am redirected to the page "PostFeedPage"
		And I could see the pull down to refresh instruction is not displayed anymore
		And the menu detail is closed
	When I tap the hamburger icon
	Then I should see the menu detail is opened
	When I tap the Useful Stuff item from the menu detail
	Then I am redirected to the page "WikiPage"
	When I tap the hamburger icon
	Then I should see the menu detail is opened
	When I tap the Write Down icon from the menu detail with authenticated "true" and signed up "true"
	Then I am redirected to the page "PostFeedPage"
		And I could see the pull down to refresh instruction is displayed again

@tag FacebookLogonViaWriteYourThoughtsMenu
Scenario: Loading more posts
	Given I am authenticated
		And I am on the page "PostFeedPage"
	Then I should see a Post posted by "Peter Smith", with title "Depression is something everyone should be aware of.", and content "According to @Glassdoor, these signs mean you're overthinking things at work:"
	When I tap the load more post at the end
	Then I should that posts are added more 

@tag FacebookLogonViaWriteYourThoughtsMenu
Scenario: Refreshing the Posts
	Given I am authenticated
		And I am on the page "PostFeedPage"
	Then I should see a Post posted by "Peter Smith", with title "Depression is something everyone should be aware of.", and content "According to @Glassdoor, these signs mean you're overthinking things at work:"
	When I pull to refresh the page
	Then I should see that post list are refreshed from the server again