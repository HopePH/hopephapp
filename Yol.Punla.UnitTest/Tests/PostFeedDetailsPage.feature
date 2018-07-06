Feature: PostFeedDetailsPage

@tag FacebookLogonViaWriteYourThoughtsMenu
Scenario: Navigating to PostFeedDetailsPage Then Back
	Given I am authenticated
		And I am on the page "PostFeedPage"
		And I could see that the IsNavigatingDetailsPage mark is "false"
	When I tap the comment icon of the post of "Peter Smith" with content "Mental health is now on the final reading in the Congress. Sen. Riza Hotiveros pushes the bill." and postFeedId "1112"
	Then I am redirected to the page "PostFeedDetailPage"
		And I can see the details of the post with author "Peter Smith" and content "Mental health is now on the final reading in the Congress. Sen. Riza Hotiveros pushes the bill."
		And I could see that the IsNavigatingDetailsPage mark is "true"
	When I tap the back icon from PostFeedDetailsPage
	Then I am redirected to the page "PostFeedPage"
		And I could see that the IsNavigatingDetailsPage mark is "false"

@tag FacebookLogonViaWriteYourThoughtsMenu
Scenario Outline: Adding Comments to a post
	Given I am authenticated
		And I am on the page "PostFeedPage"
	When I tap the comment icon of the post of "Peter Smith" with content "Mental health is now on the final reading in the Congress. Sen. Riza Hotiveros pushes the bill." and postFeedId "1112"
	Then I am redirected to the page "PostFeedDetailPage"
		And I can see the details of the post with author "Peter Smith" and content "Mental health is now on the final reading in the Congress. Sen. Riza Hotiveros pushes the bill." 
	When I type comment "<comment>" and tap the Post button
	Then I should stay on the same page "PostFeedDetailPage"

	Examples: 
	| comment                  |
	| Hello, This is a comment |

@tag FacebookLogonViaWriteYourThoughtsMenu
Scenario Outline: Adding Empty comment to a post
	Given I am authenticated
		And I am on the page "PostFeedPage"
	When I tap the comment icon of the post of "Peter Smith" with content "Mental health is now on the final reading in the Congress. Sen. Riza Hotiveros pushes the bill." and postFeedId "1112"
	Then I am redirected to the page "PostFeedDetailPage"
		And I can see the details of the post with author "Peter Smith" and content "Mental health is now on the final reading in the Congress. Sen. Riza Hotiveros pushes the bill." 
	When I type comment "<comment>" and tap the Post button
	Then I should see an empty comment message "<errorMessage>"

	Examples: 
	| comment | errorMessage                                          |
	|         | Empty comment is not allowed. Please write a comment. |