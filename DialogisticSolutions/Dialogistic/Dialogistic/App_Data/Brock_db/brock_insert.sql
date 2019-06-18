-----------------------------------------
----                                   --
-------- Brock's insert statements ------
----                                   --
-----------------------------------------

--INSERT INTO [dbo].[UserProfiles] (UserID, UserName, FullName, Email, IsCaller, DonationsRaised) VALUES
--('1865444f-b329-4fd2-8d9e-b3c540a455f2', 'mvargas', 'Meghan Vargas', 'mvargas@gmail.com', 0, 0),
--('6f81a0fd-e18a-43bb-9118-2b2ae71820a3', 'fluffykitty91', 'Rebecca Williamson', 'fluffykitty91@yahoo.com', 0, 0),
--('c3b448ed-60eb-45ad-afab-0fc498938c3b', 'dinkster01', 'Gary Dinkins', 'dinkster01@hotmail.com', 1, 228.00),
--('4532f546-3233-41af-8d1b-a9278b35cb1b', 'DialogisticSuperAdmin', 'Dialogistic Super Admin', 'admin@admin.com', 1, 345.00);


INSERT INTO [dbo].[CallLogs] (CallerID, ConstituentID, DateOfCall, CallAnswered, LineAvailable) VALUES
    ('1865444f-b329-4fd2-8d9e-b3c540a455f2', 2, '2019/01/05', 1, 1),		   
    ('1865444f-b329-4fd2-8d9e-b3c540a455f2', 3, '2019/01/06', 1, 1),		   
    ('1865444f-b329-4fd2-8d9e-b3c540a455f2', 4, '2019/02/08', 1, 1),		   
    ('1865444f-b329-4fd2-8d9e-b3c540a455f2', 5, '2019/02/10', 1, 1),
    ('1865444f-b329-4fd2-8d9e-b3c540a455f2', 6, '2019/04/04', 1, 1),
    ('d5870b36-d2d5-42ca-8059-b9ccdefd540f', 8, '2019/04/02', 1, 1),
    ('d5870b36-d2d5-42ca-8059-b9ccdefd540f', 760, '2019/03/11', 1, 1),
    ('d5870b36-d2d5-42ca-8059-b9ccdefd540f', 778, '2019/03/15', 1, 1),
    ('d5870b36-d2d5-42ca-8059-b9ccdefd540f', 999, '2019/03/20', 1, 1),
    ('d5870b36-d2d5-42ca-8059-b9ccdefd540f', 1176, '2019/03/23', 1, 1),
    ('9a77e51b-8a44-417b-bcfb-e8d9f8d22266', 1719, '2019/03/08', 1, 1),
    ('9a77e51b-8a44-417b-bcfb-e8d9f8d22266', 1999, '2019/03/08', 1, 1),
    ('9a77e51b-8a44-417b-bcfb-e8d9f8d22266', 2719, '2019/03/08', 1, 1),
    ('9a77e51b-8a44-417b-bcfb-e8d9f8d22266', 3177, '2019/03/08', 1, 1),
    ('9a77e51b-8a44-417b-bcfb-e8d9f8d22266', 3178, '2019/03/08', 1, 1),
    ('26dad7a4-ba08-4f6f-8260-070b1574613e', 3179, '2019/01/05', 1, 1),
    ('26dad7a4-ba08-4f6f-8260-070b1574613e', 7, '2019/01/07', 1, 1),
    ('26dad7a4-ba08-4f6f-8260-070b1574613e', 12, '2019/03/11', 1, 1),		   		   		   
    ('26dad7a4-ba08-4f6f-8260-070b1574613e', 144, '2019/03/20', 1, 1),
    ('26dad7a4-ba08-4f6f-8260-070b1574613e', 679, '2019/04/01', 1, 1),		   		   
    ('c3b448ed-60eb-45ad-afab-0fc498938c3b', 1778, '2019/03/21', 1, 1),
    ('c3b448ed-60eb-45ad-afab-0fc498938c3b', 2013, '2019/03/08', 1, 1),		   		   
    ('c3b448ed-60eb-45ad-afab-0fc498938c3b', 3176, '2019/03/15', 1, 1),
    ('c3b448ed-60eb-45ad-afab-0fc498938c3b', 3200, '2019/02/08', 1, 1),
    ('c3b448ed-60eb-45ad-afab-0fc498938c3b', 3201, '2019/04/10', 1, 1);

--INSERT INTO [dbo].[CallAssignments](CallerID, ConstituentID) VALUES
--('c3b448ed-60eb-45ad-afab-0fc498938c3b', 760),
--('6f81a0fd-e18a-43bb-9118-2b2ae71820a3', 1013),
--('1865444f-b329-4fd2-8d9e-b3c540a455f2', 1176),
--('1865444f-b329-4fd2-8d9e-b3c540a455f2', 679),
--('1865444f-b329-4fd2-8d9e-b3c540a455f2', 778),
--('c3b448ed-60eb-45ad-afab-0fc498938c3b', 1719);

--INSERT INTO [dbo].[Constituents](ConstituentID, PrimaryAddressee, PreferredAddressLine1, PreferredCity, PreferredState, PreferredZIP, PhoneNumber, Deceased, UniversityRelationship)
--	VALUES
--( 679, 'Mr. Joan Lawrence', '4778 N Orchard St', 'Fresno', 'Oregon', '97333-2274', '515-555-5516', 1, 'Alumni'),
--( 760, 'Ms. Sharelle Beck', 'PO Box 305', 'Monmouth', 'California', '98512-8108', '831-555-9748', 1, 'Alumni'),
--( 778, 'Ms. Katrina Horning', '8639 Highland Rd', 'Independence', 'Ohio', '97601-9313', '541-555-9601', 1, 'Alumni'),
--( 1013, 'Mr. Keith Hogan', '1840 W 14th Ave', 'Eugene', 'Oregon', '97086-5029', '503-555-3755', 1, 'Alumni'),
--( 1176, 'Mr. Christian Sanchez', '135 Kanuku Ct SE', 'Salem', 'Oregon', '93314-7704', '503-555-1192', 1, 'Alumni'),
--( 1719, 'Mr. Dell Peterson', '1471 Burns St', 'West Linn', 'Oregon', '95126-4817', '503-555-1701', 0, 'Alumni'),
--( 1999, 'Mr. Harold McCabe', '15744 SE Harrison St', 'Portland', 'Oregon', '97031-1994', '503-555-2297', 0, 'Alumni'),
--( 12, 'Mr. David Smith', '4071 Commercial St', 'Fresno', 'Oregon', '97333-2274', '515-555-5516', 1, 'Alumni'),
--( 144, 'Ms. Tracey Billera', '1203 Carilor Ct', 'Monmouth', 'California', '98512-8108', '831-555-9748', 1, 'Alumni'),
--( 1778, 'Ms. Katherine Howards', '1635 River Rd', 'Independence', 'Ohio', '97601-9313', '541-555-9601', 1, 'Alumni'),
--( 2013, 'Mr. Levi Strauss', '840 NW 10th Ave', 'Eugene', 'Oregon', '97086-5029', '503-555-3755', 1, 'Alumni'),
--( 3176, 'Mr. Rebecca Sanchez', '1356 Happy Ct SE', 'Salem', 'Oregon', '93314-7704', '503-555-1192', 1, 'Alumni'),
--( 2719, 'Mr. William Ricketts', '3431 Kalmia Dr', 'Keizer', 'Oregon', '95126-4817', '503-555-1701', 0, 'Alumni'),
--( 999, 'Mr. Jeremy Lincoln', '1744 NE 1st St', 'Portland', 'Oregon', '97031-1994', '503-555-2297', 0, 'Alumni');

INSERT INTO [dbo].[Gifts] (CallID, ConstituentID, Printed, GiftAmount, GiftType, GiftRecipient) VALUES
	(43, 778, 0, 200.00, 'Pledge', 'Computer Science'),
	(44, 1176, 0, 100.00, 'Pledge', 'Foundation'),
	(45, 760, 0, 50.00, 'Pledge', 'Basketball'),
	(46, 778, 0, 500.00, 'Pledge', 'Football'),
	(47, 1999, 0, 1000.00, 'Pledge', 'English'),
	(48, 1176, 0, 250.00, 'Pledge', 'General'),
	(49, 679, 0, 20.00, 'Pledge', 'Foundation'),
	(50, 1176, 0, 2000.00, 'Pledge', 'Computer Science'),
	(51, 1999, 0, 1000.00, 'Pledge', 'English');