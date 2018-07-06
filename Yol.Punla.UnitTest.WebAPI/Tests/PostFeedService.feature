Feature: PostFeedService

Scenario: Calling the Http GET method in getting top post feed
	Given the api client is authenticated
		And the post feed list is null
	When the client calls to the GetTopPosts endpoint for company "HopePH"
	Then I should see the response code is OK
		And I should that there are more or less 100 post the server
		And I should see that the post has a displayed a contact gender

Scenario: Calling the Http GET method in getting comments of a specific post feed
	Given the api client is authenticated
	When the client calls to the GetComments endpoint for post id "1" company "HopePH"
	Then I should see the response code is OK

Scenario Outline: Calling the Http POST method for adding or updating post feed
	Given the api client is authenticated
	When the client calls to the GetTopPosts endpoint and get the first item for company "HopePH" and get the post feed "<contentTextOld>"
	Then I should see one of the item content is "<contentTextOld>"
	When the client calls to the AddNewPostFeed endpoint for post id "<postId>", company "<company>", title "<title>", content url "<contentURL>", content text "<contentText>", contact id "<contactId>"
	Then I should see the response code is OK
		And I should see that the new post feed content "<contentText>"
	When the client calls to the AddNewPostFeed endpoint for post id "<postId>", company "<company>", title "New Post from @HaiyanRbf", content url "<contentURL>", content text "<contentTextOld>", contact id "<contactId>"
	Then I should see the response code is OK
		And I should see that the new post feed content "<contentTextOld>"

	Examples: 
	| postId | company | title                | contentURL | contentText																																				| contactId | contentTextOld                                                                                                                                      |
	| 512    | HopePH  | New Post from @worde |            | Welcome to depression and anxiety support group! You could now post what you are going into right now anonymously without sacrificing your privacy. Edited | 1934      | Welcome to depression and anxiety support group! You could now post what you are going into right now anonymously without sacrificing your privacy. |

Scenario Outline: Calling the Http GET and POST method in adding, deleting and getting likes
	Given the api client is authenticated
	When the client calls to the GetPostFeedLike endpoint for post id "<postId>" company "HopePH"
	Then I should see the response code is OK
		And I should see that there are "<beforeLikesCount>" likes
	When I hit one like to the post feed id "<postId>" of poster id "<posterId>"
	Then I should see the response code is OK
		And I should see that there are "<afterLikesCount>" likes
	When I remove one like to the post feed id "<postId>" of poster id "<posterId>"
	Then I should see the response code is OK
		And I should see that there are "<beforeLikesCount>" likes
	
	Examples: 
	| postId | beforeLikesCount | afterLikesCount | posterId |
	| 1      | 1                | 2               | 1893     | 	 

Scenario: Calling the Http GET method in getting own post feed
	Given the api client is authenticated
	When the client calls to the GetOwnPosts endpoint for company "HopePH" and poster id "1934"
	Then I should see the response code is OK
		And I should see that all posts are from poster id "1934"

Scenario Outline: Calling the Http GET method of getting post feed in quick and by batch
	Given the api client is authenticated
		And the post feed list is null
	When the client calls to the GetTopPostsWithSpeed endpoint for company "<company>", contact "<contactId>" and starting post "<basePostId>" for the first time
	Then I should see the response code is OK
		And I should that there are "<noOfPosts>" post fetch from the server
	When the client calls again to fetch the next batch to the GetTopPostsWithSpeed endpoint for company "<company>", contact "<contactId>" and starting post "<basePostId>"
	Then I should that there are "<noOfPosts>" post fetch from the server of the next batch

	Examples: 
	| contactId | company | basePostId | noOfPosts |
	| 1934      | HopePH  | 0          | 20        |	

Scenario Outline: Calling the Http GET method of getting notifications from the server
	Given the api client is authenticated
		And the post feed list is null
	When the client calls to the GetPostNotifications endpoint for company "<company>" and contact "<contactId>"
	Then I should see the response code is OK
		And I should that there post notifications fetch from the server

	Examples: 
	| contactId | company |
	| 1934      | HopePH  |