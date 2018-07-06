Feature: ReceiverService

Scenario Outline: Calling the Http GET method in saving a new receiver or contact/user
	Given the api client is authenticated
		And the contact is null
	When the client calls to the SavingReceiverDetails endpoint for saving a new contact id "<id>", first name "<firstName>", last name "<lastName>", mobile phone "<mobilePhone>" is active "<isActive>", user name "<userName>", password "<password>", type code "<typeCode>", gender code "<genderCode>", alias name "<aliasName>", company name "<companyName>", fbId "<fbId>" 
	Then I should see the response code is OK
	When the client calls the GetReceiverContactByUserName using the facebook id "<fbId>", user name "<userName>" and company name "<companyName>"
	Then I should see the response code is OK
		And I should see the receiver or contact is present in the records
	When the client calls the DeletingReceiver of the newly added contact
	Then I should see the response code is OK
	When the client calls the GetReceiverContactByUserName using the facebook id "<fbId>", user name "<userName>" and company name "<companyName>"
	Then I should see the response code is OK
		And I should see the receiver or contact is not present in the records already

	Examples: 
	| id | firstName | lastName | mobilePhone | isActive | userName               | password  | typeCode | genderCode | aliasName | companyName | fbId            |
	| 0  | Worde     | Salinas  | 09477691857 | true     | hello@fitnesstapp2.com | 123456Aa@ | Receiver | Male       | Worde5    | HopePH      | 357823074843149 |

Scenario: Calling the Http POST method for getting verification code of the new user
	Given the api client is authenticated
		And the verfification code is null
	When the client calls to the VerifyEmail endpoint for email "remote.alfon@gmail.com" and alias "Worde PanggaHealth"
	Then I could see that the verification could is not null

Scenario: Calling the Http Get method for checking the email if it exists in the server already
	Given the user with email is null
	When the client calls to the Http get method for getting the client contact via email address "alfonchitosalao2@gmail.com"
	Then I should see the response code is OK
		And I should see that the user with email is not null	
	When the client calls to the Http get method for getting the client contact via email address "motomotobotbot@gmail.com"
	Then I should see the response code is OK 
		And I should see that the user with email is null