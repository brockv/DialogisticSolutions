------------------------------------------------------
----                                                  --
----                DOM INSERT SECTION                --
----                                                  --
--------------------------------------------------------

INSERT INTO [dbo].[UserProfiles] (UserID, UserName, FullName, Email, IsCaller, DonationsRaised) VALUES
('17e550aa-1e39-490f-9768-30266cf6f36c', 'starkt', 'Tony Stark', 'user_three@gmail.com', 1, 0),
('56f35c65-9a79-42da-929a-7e27f8da25a7', 'adminUser', 'Admin User', 'admin_user@gmail.com', 0, 0),
('b45b12bf-5a4e-48c7-898f-ad69585cc97d', 'groshond', 'Dominic Groshong', 'user_two@gmail.com', 1, 128.50),
('ed11ceec-e5f5-4ef9-aaf6-dd8ce57dd954', 'blackj', 'Jack Black', 'admin@admin.com', 1, 200.00);


--INSERT INTO [dbo].[Constituents](ConstituentID, PrimaryAddressee, PreferredAddressLine1, PreferredAddressLine2, PreferredAddressLine3, PreferredCity, PreferredState, PreferredZIP, PhoneNumber, MobilePhoneNumber, AlternatePhoneNumber, Deceased, DonationStatus, UniversityRelationship, CallPriority)
--	VALUES
--( 679, 'Mr. Joan Lawrence', '4778 N Orchard St', '', '', 'Fresno', 'Oregon', '97333-2274', '515-555-5516', '515-555-3843', NULL, 0, NULL, 'Alumni', 1),
--( 760, 'Ms. Sharelle Beck', 'PO Box 305', '', '', 'Monmouth', 'California', '98512-8108', '831-555-9748', NULL, NULL, 0, NULL, 'Alumni', 1),
--( 778, 'Ms. Katrina Horning', '8639 Highland Rd', '', '', 'Independence', 'Ohio', '97601-9313', '541-555-9601', NULL, NULL, 0, NULL, 'Alumni', 2),
--( 1013, 'Mr. Keith Hogan', '1840 W 14th Ave', '', '', 'Eugene', 'Oregon', '97086-5029', '503-555-3755', NULL, NULL, 0, NULL, 'Alumni', 1),
--( 1176, 'Mr. Christian Sanchez', '135 Kanuku Ct SE', '', '', 'Salem', 'Oregon', '93314-7704', '503-555-1192', NULL, NULL, 0, NULL, 'Alumni', 1),
--( 1719, 'Mr. Dell Peterson', '1471 Burns St', '', '', 'West Linn', 'Oregon', '95126-4817', '503-555-1701', NULL, NULL, 0, NULL, 'Alumni', 1),
--( 1999, 'Mr. Harold McCabe', '15744 SE Harrison St', '', '', 'Portland', 'Oregon', '97031-1994', '503-555-2297', NULL, NULL, 0, NULL, 'Alumni', 1),
--( 2000, 'Mr. Denny Malone', '345 7th Ave', '', '', 'New York', 'New York', '10001-5003', '503-555-2297', NULL, NULL, 0, NULL, 'Alumni', 1),
--( 2005, 'Mr. Phil Russo', '345 7th Ave', '', '', 'New York', 'New York', '10001-5003', '503-555-2297', NULL, NULL, 0, NULL, 'Alumni', 1),
--( 2006, 'Mr. Mitch Rapp', '155 Rapp Way', '', '', 'Arlington', 'Virginia', '22207-5003', '929-818-9900', NULL, NULL, 0, NULL, 'Alumni', 1),
--( 2007, 'Mr. Scot Harvath', '322 127th Ave', '', '', 'Seattle', 'Washington', '99112-5003', '503-666-2297', NULL, NULL, 0, NULL, 'Alumni', 1),
--( 2008, 'Mr. John Wells', '3 9th Ave', '', '', 'Fayetteville', 'North Carolina', '81893-1334', '503-777-2297', NULL, NULL, 0, NULL, 'Alumni', 1);

--INSERT INTO [dbo].[CallLogs] (CallerID, ConstituentID, DateOfCall, CallAnswered, LineAvailable, CallOutcome)
--	VALUES ('b45b12bf-5a4e-48c7-898f-ad69585cc97d', 778, '2019/01/05', 1, 1, 'Pledge'),
--		   ('b45b12bf-5a4e-48c7-898f-ad69585cc97d', 1176, '2019/01/05', 1, 1, 'Pledge'),
--		   ('b45b12bf-5a4e-48c7-898f-ad69585cc97d', 760, '2019/01/06', 1, 1, 'Pledge'),
--		   ('b45b12bf-5a4e-48c7-898f-ad69585cc97d', 778, '2019/01/07', 1, 1, 'Pledge'),
--		   ('b45b12bf-5a4e-48c7-898f-ad69585cc97d', 1999, '2019/02/08', 1, 1, 'Pledge'),
--		   ('b45b12bf-5a4e-48c7-898f-ad69585cc97d', 1176, '2019/02/08', 1, 1, 'Pledge'),
--		   ('b45b12bf-5a4e-48c7-898f-ad69585cc97d', 760, '2019/02/10', 1, 1, 'Pledge'),
--		   ('b45b12bf-5a4e-48c7-898f-ad69585cc97d', 679, '2019/03/11', 1, 1, 'Pledge'),
--		   ('b45b12bf-5a4e-48c7-898f-ad69585cc97d', 1176, '2019/03/11', 1, 1, 'Pledge'),
--		   ('b45b12bf-5a4e-48c7-898f-ad69585cc97d', 760, '2019/03/15', 1, 1, 'Pledge'),
--		   ('ed11ceec-e5f5-4ef9-aaf6-dd8ce57dd954', 1013, '2019/03/15', 1, 1, 'Pledge'),
--		   ('ed11ceec-e5f5-4ef9-aaf6-dd8ce57dd954', 1176, '2019/03/20', 1, 1, 'Pledge'),
--		   ('ed11ceec-e5f5-4ef9-aaf6-dd8ce57dd954', 760, '2019/03/20', 1, 1, 'Pledge'),
--		   ('ed11ceec-e5f5-4ef9-aaf6-dd8ce57dd954', 778, '2019/03/21', 1, 1, 'Pledge'),
--		   ('ed11ceec-e5f5-4ef9-aaf6-dd8ce57dd954', 679, '2019/03/23', 1, 1, 'Pledge'),
--		   ('ed11ceec-e5f5-4ef9-aaf6-dd8ce57dd954', 1013, '2019/03/08', 1, 1, 'Pledge'),
--		   ('ed11ceec-e5f5-4ef9-aaf6-dd8ce57dd954', 679, '2019/03/08', 1, 1, 'Pledge'),
--		   ('ed11ceec-e5f5-4ef9-aaf6-dd8ce57dd954', 760, '2019/04/01', 1, 1, 'Pledge'),
--		   ('ed11ceec-e5f5-4ef9-aaf6-dd8ce57dd954', 1013, '2019/04/02', 1, 1, 'Pledge'),
--		   ('ed11ceec-e5f5-4ef9-aaf6-dd8ce57dd954', 1176, '2019/04/03', 1, 1, 'Pledge'),
--		   ('ed11ceec-e5f5-4ef9-aaf6-dd8ce57dd954', 778, '2019/04/04', 1, 1, 'Pledge'),
--		   ('ed11ceec-e5f5-4ef9-aaf6-dd8ce57dd954', 1176, '2019/04/10', 1, 1, 'Pledge'),
--		   ('ed11ceec-e5f5-4ef9-aaf6-dd8ce57dd954', 2005, '2019/04/11', 1, 1, 'Credit Card'),
--		   ('ed11ceec-e5f5-4ef9-aaf6-dd8ce57dd954', 2006, '2019/04/11', 1, 1, 'Pledge'),
--		   ('ed11ceec-e5f5-4ef9-aaf6-dd8ce57dd954', 2007, '2019/04/12', 1, 1, 'Pledge'),
--		   ('ed11ceec-e5f5-4ef9-aaf6-dd8ce57dd954', 2008, '2019/04/12', 1, 1, 'Pledge');

--INSERT INTO [dbo].[Gifts] (ConstituentID, CallID, Printed, GiftAmount, GiftType, GiftRecipient) VALUES
--	(778, 26, 0, 200.00, 'Pledge', 'Computer Science'),
--	(1176, 1, 0, 100.00, 'Pledge', 'Foundation'),
--	(760, 2, 0, 50.00, 'Pledge', 'Basketball'),
--	(778, 3, 0, 500.00, 'Pledge', 'Football'),
--	(1999, 4, 0, 1000.00, 'Pledge', 'English'),
--	(1176, 5, 0, 250.00, 'Pledge', 'General'),
--	(679, 6, 0, 200.00, 'Pledge', 'Poetry'),
--	(2005, 7, 0, 375.00, 'Pledge', 'History'),
--	(2008, 8, 0, 220.00, 'Pledge', 'Computer Science'),
--	(2007, 9, 0, 100.00, 'Pledge', 'Computer Science'),
--	(679, 10, 0, 20.00, 'Pledge', 'Foundation'),
--	(1176, 11, 0, 2000.00, 'Pledge', 'Computer Science'),
--	(1999, 12, 0, 1000.00, 'Pledge', 'English');