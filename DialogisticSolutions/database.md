# Steps to get the database up and running

**NOTE**

These steps assume you are working with a database with no tables. If you have tables already, **DO NOT DROP YOUR ASPNET TABLES**. Only drop the tables specific to our project, e.g., Constituents, CallLogs, UserProfiles, Gifts, ProposedChanges, and CallAssignments. However, you should open the AspNet tables (right-click the table name, select 'View Data'), and delete any rows that contain data. But be sure to keep the tables themselves!

## Step 1-A (Optional)
* If you need to drop our tables, connect the DOWN script to your database, and drop **ONLY** our tables (Constituents, CallDetails, UserProfiles, and CallAssignments).

## Step 1-B (Required)
* In the UP script, connect to your database. Comment out all of the insert statements. Then, run **ONLY** the table creation statements for our tables (Constituents, CallDetails, UserProfiles, and CallAssignments). Verify that the tables were created in the SQL Server Explorer window in Visual Studio.

## Step 2
* Add these two lines to your Web.config inside the appSettings tag:
```
<add key="SUPER_ADMIN_USERNAME" value ="DialogisticSuperAdmin"/>
<add key="SUPER_ADMIN_PASSWORD" value="Dialogistic_SuperAdmin_Password_02"/>
```

## Step 3
* Add the following code to Startup.cs **AFTER** the "Standard" user role is created. Duplicate it for as many users as you'd like to add, changing values where necessary to make each unique.
```
// Create a new user (first one)
var userOne = new ApplicationUser
{ 
	FirstName = "User",
	LastName = "One",
	UserName = "userOne",
	Email = "user_one@gmail.com",
	EmailConfirmed = true,
	LockoutEnabled = true
};

// Set the password and attempt to create the user
string userOnePWD = "I_Am_User_1";
var newUserOne = UserManager.Create(userOne, userOnePWD);

// Add the new user account to a role of your choosing
if (newUserOne.Succeeded)
{
	var newUserOneRole = UserManager.AddToRole(userOne.Id, "Standard");
}
```

## Step 4
* Run the application **ONE TIME**, stopping it once the page fully loads. Check your AspNetUsers table and verify that the users were created. Only proceed to Step 5 if the users were created.

## Step 5
**IMPORTANT**
* DELETE the lines added to your Web.config in Step 2, as well as the lines added to your Startup.cs in Step 3.

## Step 6
* In the UP script comment out all the statements **EXCEPT** the UserProfiles INSERT statement. Change the UserID, UserName, and Email fields to match what's in your AspNetUsers table. Put whatever values you want for the remaining fields. Run the script, and verify that your UserProfiles table has been populated. Only proceed to Step 7 if it was.

## Step 7
* Run the application again and try to sign in with the SuperAdmin account. If successful, navigate to the Import page, and import the provided CSV file.

## Step 8
* Comment out the INSERT statement from Step 6, and uncomment the INSERT statement for the CallAssignments. Change the CallerID fields to match your user's UserID's. There can be duplicate CallerID's, but not ConstituentID's.
