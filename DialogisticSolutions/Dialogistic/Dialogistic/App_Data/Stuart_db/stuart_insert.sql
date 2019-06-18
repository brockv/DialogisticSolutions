
---------------------------------------
--                                   --
------ Stuart's insert statements ------
--                                   --
-----------------------------------------

--INSERT INTO [dbo].[UserProfiles] (UserID, UserName, FullName, Email, IsCaller, DonationsRaised) VALUES
--('14e2c40f-8330-422b-83cb-e88091a370a5', 'userOne', 'User One', 'user_one@gmail.com', 1, 345.20),
--('ca370a70-7975-4c16-ba05-a27c27836dc9', 'userTwo', 'User Two', 'user_two@gmail.com', 1, 128.50),
--('283e4eee-0ce7-4e51-8610-ae43f608ef1f', 'userThree', 'User Three', 'user_three@gmail.com', 1, 445.50),
--('950e70e6-7fe8-41a4-856f-33bf57b7757b', 'DialogisticSuperAdmin', 'Admin', 'dinkster01@hotmail.com', 0, 0),
--('9a44a21a-d719-421e-b534-31ad2596cd4f', 'adminUserOne', 'Admin User1', 'admin_user1@gmail.com', 0, 0),
--('29d404a5-3064-4d84-845e-ffc6437dcaf3', 'adminUserTwo', 'Admin User2', 'admin_user2@gmail.com', 0, 0);

--INSERT INTO [dbo].[Constituents](ConstituentID, PrimaryAddressee, PreferredAddressLine1, PreferredAddressLine2, PreferredAddressLine3, PreferredCity, PreferredState, PreferredZIP, PhoneNumber, MobilePhoneNumber, AlternatePhoneNumber, Deceased, DonationStatus, UniversityRelationship, CallPriority)
--	VALUES
--( 679, 'Alex Hawke', '4778 N Orchard St', '', '', 'Fresno', 'Oregon', '97333-2274', '515-555-5516', '515-555-3843', NULL, 0, NULL, 'Alumni', 1),
--( 760, 'John Corey', 'PO Box 305', '', '', 'Monmouth', 'California', '98512-8108', '831-555-9748', NULL, NULL, 0, NULL, 'Alumni', 1),
--( 778, 'Beecher White', '8639 Highland Rd', '', '', 'Independence', 'Ohio', '97601-9313', '541-555-9601', NULL, NULL, 0, NULL, 'Alumni', 2),
--( 1013, 'Court Gentry', '1840 W 14th Ave', '', '', 'Eugene', 'Oregon', '97086-5029', '503-555-3755', NULL, NULL, 0, NULL, 'Alumni', 1),
--( 1176, 'Dewey Andreas', '135 Kanuku Ct SE', '', '', 'Salem', 'Oregon', '93314-7704', '503-555-1192', NULL, NULL, 0, NULL, 'Alumni', 1),
--( 1719, 'Evan Smoak', '1471 Burns St', '', '', 'West Linn', 'Oregon', '95126-4817', '503-555-1701', NULL, NULL, 0, NULL, 'Alumni', 1),
--( 1999, 'James Reece', '15744 SE Harrison St', '', '', 'Portland', 'Oregon', '97031-1994', '503-555-2297', NULL, NULL, 0, NULL, 'Alumni', 1),
--( 2000, 'Denny Malone', '345 7th Ave', '', '', 'New York', 'New York', '10001-5003', '503-555-2297', NULL, NULL, 0, NULL, 'Alumni', 1),
--( 2005, 'Phil Russo', '345 7th Ave', '', '', 'New York', 'New York', '10001-5003', '503-555-2297', NULL, NULL, 0, NULL, 'Alumni', 1),
--( 2006, 'Mitch Rapp', '155 Rapp Way', '', '', 'Arlington', 'Virginia', '22207-5003', '929-818-9900', NULL, NULL, 0, NULL, 'Alumni', 1),
--( 2007, 'Scot Harvath', '322 127th Ave', '', '', 'Seattle', 'Washington', '99112-5003', '503-666-2297', NULL, NULL, 0, NULL, 'Alumni', 1),
--( 2008, 'John Wells', '3 9th Ave', '', '', 'Fayetteville', 'North Carolina', '81893-1334', '503-777-2297', NULL, NULL, 0, NULL, 'Alumni', 1);

--INSERT INTO [dbo].[CallLogs] (CallerID, ConstituentID, DateOfCall, CallAnswered, LineAvailable, CallOutcome)
--	VALUES ('14e2c40f-8330-422b-83cb-e88091a370a5', 778, '2019/01/05', 1, 1, 'Pledge'),
--		   ('14e2c40f-8330-422b-83cb-e88091a370a5', 1176, '2019/01/05', 1, 1, 'Pledge'),
--		   ('14e2c40f-8330-422b-83cb-e88091a370a5', 760, '2019/01/06', 1, 1, 'Pledge'),
--		   ('14e2c40f-8330-422b-83cb-e88091a370a5', 778, '2019/01/07', 1, 1, 'Pledge'),
--		   ('14e2c40f-8330-422b-83cb-e88091a370a5', 1999, '2019/02/08', 1, 1, 'Pledge'),
--		   ('ca370a70-7975-4c16-ba05-a27c27836dc9', 1176, '2019/02/08', 1, 1, 'Pledge'),
--		   ('ca370a70-7975-4c16-ba05-a27c27836dc9', 760, '2019/02/10', 1, 1, 'Pledge'),
--		   ('ca370a70-7975-4c16-ba05-a27c27836dc9', 679, '2019/03/11', 1, 1, 'Pledge'),
--		   ('ca370a70-7975-4c16-ba05-a27c27836dc9', 1176, '2019/03/11', 1, 1, 'Pledge'),
--		   ('ca370a70-7975-4c16-ba05-a27c27836dc9', 760, '2019/03/15', 1, 1, 'Pledge'),
--		   ('ca370a70-7975-4c16-ba05-a27c27836dc9', 1013, '2019/03/15', 1, 1, 'Pledge'),
--		   ('ca370a70-7975-4c16-ba05-a27c27836dc9', 1176, '2019/03/20', 1, 1, 'Pledge'),
--		   ('ca370a70-7975-4c16-ba05-a27c27836dc9', 760, '2019/03/20', 1, 1, 'Pledge'),
--		   ('14e2c40f-8330-422b-83cb-e88091a370a5', 778, '2019/03/21', 1, 1, 'Pledge'),
--		   ('283e4eee-0ce7-4e51-8610-ae43f608ef1f', 679, '2019/03/23', 1, 1, 'Pledge'),
--		   ('283e4eee-0ce7-4e51-8610-ae43f608ef1f', 1013, '2019/03/08', 1, 1, 'Pledge'),
--		   ('283e4eee-0ce7-4e51-8610-ae43f608ef1f', 679, '2019/03/08', 1, 1, 'Pledge'),
--		   ('283e4eee-0ce7-4e51-8610-ae43f608ef1f', 760, '2019/04/01', 1, 1, 'Pledge'),
--		   ('283e4eee-0ce7-4e51-8610-ae43f608ef1f', 1013, '2019/04/02', 1, 1, 'Pledge'),
--		   ('283e4eee-0ce7-4e51-8610-ae43f608ef1f', 1176, '2019/04/03', 1, 1, 'Pledge'),
--		   ('283e4eee-0ce7-4e51-8610-ae43f608ef1f', 778, '2019/04/04', 1, 1, 'Pledge'),
--		   ('283e4eee-0ce7-4e51-8610-ae43f608ef1f', 1176, '2019/04/10', 1, 1, 'Pledge'),
--		   ('283e4eee-0ce7-4e51-8610-ae43f608ef1f', 2005, '2019/04/11', 1, 1, 'Credit Card'),
--		   ('283e4eee-0ce7-4e51-8610-ae43f608ef1f', 2006, '2019/04/11', 1, 1, 'Pledge'),
--		   ('14e2c40f-8330-422b-83cb-e88091a370a5', 2007, '2019/04/12', 1, 1, 'Pledge'),
--		   ('14e2c40f-8330-422b-83cb-e88091a370a5', 2008, '2019/04/12', 1, 1, 'Pledge');



--INSERT INTO [dbo].[CallAssignments](CallerID, ConstituentID, CallLogID) VALUES
--('ca370a70-7975-4c16-ba05-a27c27836dc9', 760, NULL),
--('ca370a70-7975-4c16-ba05-a27c27836dc9', 1013, NULL),
--('ca370a70-7975-4c16-ba05-a27c27836dc9', 1176, NULL),
--('283e4eee-0ce7-4e51-8610-ae43f608ef1f', 679, NULL),
--('283e4eee-0ce7-4e51-8610-ae43f608ef1f', 778, NULL),
--('283e4eee-0ce7-4e51-8610-ae43f608ef1f', 1719, NULL),
--('283e4eee-0ce7-4e51-8610-ae43f608ef1f', 2000, NULL),
--('14e2c40f-8330-422b-83cb-e88091a370a5', 2005, NULL),
--('14e2c40f-8330-422b-83cb-e88091a370a5', 2006, NULL),
--('14e2c40f-8330-422b-83cb-e88091a370a5', 2007, NULL),
--('14e2c40f-8330-422b-83cb-e88091a370a5', 2008, NULL);

--INSERT INTO [dbo].[Gifts] (ConstituentID, CallID, Printed, GiftAmount, GiftType, GiftRecipient) VALUES
--	(778, 3, 0, 200.00, 'Pledge', 'Computer Science'),
--	(1176, 4, 0, 100.00, 'Pledge', 'Foundation'),
--	(760, 5, 0, 50.00, 'Pledge', 'Basketball'),
--	(778, 6, 0, 500.00, 'Pledge', 'Football'),
--	(1999, 7, 0, 1000.00, 'Pledge', 'English'),
--	(1176, 8, 0, 250.00, 'Pledge', 'General'),
--	(679, 10, 0, 200.00, 'Pledge', 'Poetry'),
--	(2005, 25, 0, 375.00, 'Pledge', 'History'),
--	(2008, 28, 0, 220.00, 'Pledge', 'Computer Science'),
--	(2007, 27, 0, 100.00, 'Pledge', 'Computer Science'),
--	(679, 17, 0, 20.00, 'Pledge', 'Foundation'),
--	(1176, 22, 0, 2000.00, 'Pledge', 'Computer Science'),
--	(1999, 12, 0, 1000.00, 'Pledge', 'English');
