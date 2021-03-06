-----------------------------------------
----                                   --
------  MASTER DB INSERT STATEMENTS  ----
----                                   --
-----------------------------------------

----INSERT INTO [dbo].[UserProfiles] (UserID, UserName, FullName, Email, IsCaller, DonationsRaised) VALUES
----('2c48fada-a71e-499e-aad3-89dc6db9a478', 'mvargas', 'Meghan Vargas', 'mvargas@gmail.com', 1, 345.20),
----('945c08c0-fcad-4b83-909e-739eafd804c0', 'fluffykitty91', 'Rebecca Williamson', 'fluffykitty91@yahoo.com', 1, 128.50),
----('04ff984f-3771-4003-bdfb-6b971ca8d564', 'dinkster01', 'Gary Dinkins', 'dinkster01@hotmail.com', 1, 228.75),
----('cd0552d1-b775-4628-aaa8-d38ff951fdf5', 'DialogisticSuperAdmin', 'Dialogistic Super Admin', 'admin@admin.com', 0, 0);

----INSERT INTO [dbo].[CallDetails] (CallerID, ConstituentID, DateOfCall, CallAnswered, LineAvailable, GiftType, GiftAmount, GiftRecipient)
----	VALUES ('2c48fada-a71e-499e-aad3-89dc6db9a478', 778, '2019/01/05', 1, 1, 'Pledge', 200.00, 'Foundation'),
----		   ('945c08c0-fcad-4b83-909e-739eafd804c0', 1176, '2019/01/05', 1, 1, 'Pledge', 200.00, 'Art'),
----		   ('2c48fada-a71e-499e-aad3-89dc6db9a478', 760, '2019/01/06', 1, 1, 'Pledge', 200.00, 'Science'),
----		   ('945c08c0-fcad-4b83-909e-739eafd804c0', 778, '2019/01/07', 1, 1, 'Pledge', 200.00, 'Sports'),
----		   ('2c48fada-a71e-499e-aad3-89dc6db9a478', 1999, '2019/02/08', 1, 1, 'Pledge', 200.00, 'General'),
----		   ('04ff984f-3771-4003-bdfb-6b971ca8d564', 1176, '2019/02/08', 1, 1, 'Pledge', 200.00, 'General'),
----		   ('2c48fada-a71e-499e-aad3-89dc6db9a478', 760, '2019/02/10', 1, 1, 'Pledge', 200.00, 'General'),
----		   ('945c08c0-fcad-4b83-909e-739eafd804c0', 679, '2019/03/11', 1, 1, 'Pledge', 200.00, 'General'),
----		   ('2c48fada-a71e-499e-aad3-89dc6db9a478', 1176, '2019/03/11', 1, 1, 'Pledge', 200.00, 'General'),
----		   ('04ff984f-3771-4003-bdfb-6b971ca8d564', 760, '2019/03/15', 1, 1, 'Pledge', 200.00, 'General'),
----		   ('2c48fada-a71e-499e-aad3-89dc6db9a478', 1013, '2019/03/15', 1, 1, 'Pledge', 200.00, 'General'),
----		   ('945c08c0-fcad-4b83-909e-739eafd804c0', 1176, '2019/03/20', 1, 1, 'Pledge', 200.00, 'General'),
----		   ('2c48fada-a71e-499e-aad3-89dc6db9a478', 760, '2019/03/20', 1, 1, 'Pledge', 200.00, 'General'),
----		   ('04ff984f-3771-4003-bdfb-6b971ca8d564', 778, '2019/03/21', 1, 1, 'Pledge', 200.00, 'General'),
----		   ('2c48fada-a71e-499e-aad3-89dc6db9a478', 679, '2019/03/23', 1, 1, 'Pledge', 200.00, 'General'),
----		   ('04ff984f-3771-4003-bdfb-6b971ca8d564', 1013, '2019/03/08', 1, 1, 'Pledge', 200.00, 'General'),
----		   ('2c48fada-a71e-499e-aad3-89dc6db9a478', 679, '2019/03/08', 1, 1, 'Pledge', 200.00, 'General'),
----		   ('945c08c0-fcad-4b83-909e-739eafd804c0', 760, '2019/04/01', 1, 1, 'Pledge', 200.00, 'General'),
----		   ('2c48fada-a71e-499e-aad3-89dc6db9a478', 1013, '2019/04/02', 1, 1, 'Pledge', 200.00, 'General'),
----		   ('945c08c0-fcad-4b83-909e-739eafd804c0', 1176, '2019/04/03', 1, 1, 'Pledge', 200.00, 'General'),
----		   ('2c48fada-a71e-499e-aad3-89dc6db9a478', 778, '2019/04/04', 1, 1, 'Pledge', 200.00, 'General'),
----		   ('04ff984f-3771-4003-bdfb-6b971ca8d564', 1176, '2019/04/10', 1, 1, 'Pledge', 600.00, 'General');

----INSERT INTO [dbo].[CallAssignments](CallerID, ConstituentID) VALUES
----('04ff984f-3771-4003-bdfb-6b971ca8d564', 760),
----('2c48fada-a71e-499e-aad3-89dc6db9a478', 1013),
----('945c08c0-fcad-4b83-909e-739eafd804c0', 1176),
----('945c08c0-fcad-4b83-909e-739eafd804c0', 679),
----('945c08c0-fcad-4b83-909e-739eafd804c0', 778),
----('04ff984f-3771-4003-bdfb-6b971ca8d564', 1719);

----INSERT INTO [dbo].[Constituents](ConstituentID, PrimaryAddressee, PreferredAddressLine1, PreferredAddressLine2, PreferredAddressLine3, PreferredCity, PreferredState, PreferredZIP, PhoneNumber, MobilePhoneNumber, AlternatePhoneNumber, LastContacted, NextToLastGiftDate, LastGiftDate, Deceased, LastGiftAmount, LifeTimeDonations, DonationStatus, UniversityRelationship, CallPriority)
----	VALUES
----( 679, 'Mr. Joan Lawrence', '4778 N Orchard St', '', '', 'Fresno', 'Oregon', '97333-2274', '515-555-5516', '515-555-3843', NULL,'4/11/2019','2/25/2018' ,'10/25/2017', 1, NULL, NULL, NULL, 'Alumni', NULL),
----( 760, 'Ms. Sharelle Beck', 'PO Box 305', '', '', 'Monmouth', 'California', '98512-8108', '831-555-9748', NULL, NULL,'4/11/2019','2/25/2018','12/14/2017', 1, NULL, NULL, NULL, 'Alumni', NULL),
----( 778, 'Ms. Katrina Horning', '8639 Highland Rd', '', '', 'Independence', 'Ohio', '97601-9313', '541-555-9601', NULL, NULL, '4/11/2019','2/25/2018','12/8/2017', 1, NULL, NULL, NULL, 'Alumni', NULL),
----( 1013, 'Mr. Keith Hogan', '1840 W 14th Ave', '', '', 'Eugene', 'Oregon', '97086-5029', '503-555-3755', NULL, NULL,'4/11/2019','2/25/2018','12/8/2017', 1, NULL, NULL, NULL, 'Alumni', NULL),
----( 1176, 'Mr. Christian Sanchez', '135 Kanuku Ct SE', '', '', 'Salem', 'Oregon', '93314-7704', '503-555-1192', NULL, NULL, '4/11/2019','2/25/2018','12/15/2017', 1, NULL, NULL, NULL, 'Alumni', NULL),
----( 1719, 'Mr. Dell Peterson', '1471 Burns St', '', '', 'West Linn', 'Oregon', '95126-4817', '503-555-1701', NULL, NULL,'4/11/2019','2/25/2018','10/25/2017', 0, NULL, NULL, NULL, 'Alumni', NULL),
----( 1999, 'Mr. Harold McCabe', '15744 SE Harrison St', '', '', 'Portland', 'Oregon', '97031-1994', '503-555-2297', NULL, NULL,'4/11/2019','2/25/2018','12/11/2017', 0, NULL, NULL, NULL, 'Alumni', NULL);

