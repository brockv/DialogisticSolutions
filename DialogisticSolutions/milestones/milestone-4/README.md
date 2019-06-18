# Milestone 4 #

## Team Project - Dialogistic (Phone-a-thon Software) ##

### Mind Map ###

![MindMap](https://bitbucket.org/floatnone/float-none/raw/550ee3020e6b2d8144d1a097ec1722101273afdd/milestones/milestone-4/images/mindmap.jpg)


### Final Vision Statement ###
For fundraisers who need phone-a-thon donor data organization and scheduling, **Dialogistic** is a data management system that will assist in the managing of a phone-a-thon, displaying information on past constituents and potential constituents. It will also allow volunteer fundraisers to be assigned people to call and allow them to submit changes to data records which will be reviewed and approved by an administrator. Unlike the current manual google document process for handling this information and call assignment, our product will allow for CSV importing/exporting and will algorithmically assign calls to volunteers based on history.
 
### Proposal ###
We would like to develop a web application that would be implemented by businesses or non-profit organizations that need to contact large amounts of customers. This could be used to get donations, phone-a-thons, or marketing.

### Initial Description ###
This project would be a web application that would assist in the managing of a phone-a-thon. This would be targeted at non-profit organizations or businesses that contact potential donors (constituents) in an attempt to receive donations. This application would display a constituents contact information that would include but is not limited to, their name, prefix, address, phone number, past donation amounts (if applicable), the time they were contacted, and if the last time they were called they answered. 

The callers would enter in donation amounts that would be in the form of either credit/debit card payments or pledges. If the constituent wishes to donate via pledge, which is usually because they do not wish to give their credit card information over the phone, and caller or administrator would send out an envelope via USPS, so that constituent could return it with a check for the pledge amount enclosed.

Before the money is officially received, the amount must be confirmed by an administrator, with general users not having the ability to facilitate changes to caller information. For any constituent updates, such as change of address, new phone number, change in marital status, an administrator’s approval would be required.

### List of Needs & Features ###
* An easy-to-use interface that allows callers to quickly and efficiently make calls to constituents
* User accounts for each caller, with the ability to make certain users administrators
* Callers to be able to click the phone number on their screen and have it dial the constituent.
* An interface that shows the constituents past donation amounts, when they were made, and the last time the constituent was contacted.
* Notate time zone of constituent, so callers can call people in Eastern Standard Time earlier than those in Pacific Standard Time
* If the constituent is an alumni, list their degree type in order to assist the caller in connecting with the constituent
* If a constituent makes a donation, the transaction needs to be approved by an administrator (not necessarily at the time of the call)
* Ability to track how much money each caller is bringing in via donations from constituents.
* Algorithm that will assign the callers bringing in the most money to the highest priority constituents. The priority would be based on how much that constituent has given in the past (a large donor), or if the call center is worried about losing a constituent (hasn’t donated in over a certain amount of time).
* List can be organized to show the constituents recommended to call first
* Ability in import/export call lists and other data for a certain period of time (daily, weekly, monthly, yearly, custom)
* Inability for standard users to alter criteria and information about constituents. They can propose a change that then is approved by an administrator.
* Daily reports sent to administrators
* In-depth donation history of the donors, which allow them to be sorted by past donors that have given the most. This will allow them to be assigned to the “top callers.”


### List of Requirements & Non-Functional Requirements ###


#### Requirements (User Stories) ####
```
E: Epic
F: Feature
U: User Story
T: Task

[E] Create a fully functional database which stores user records and constituent records
    [F] Allow the import and export of CSV files
        [U] As an administrator, I want to import a CSV file into the database, so we can have our Constituent data readily available
            [T] Create a button that allows the Upload of a CSV or Excel file that will seed the database
        [U] As an administrator, I need to be able to export a CSV file, so that I can use it in an outside database
        [U] As a volunteer I want an easy way to view the information on the individual I am calling so that I can address them properly.
                [T] Make UI that contains the most relevant information on Constituents.
                [T] Show Constituent name, address verification, donation history, and collegiate background.
        [U] As a volunteer I want to be able to update constituent information so that our records are up to date.
        [U] As an administrator I want to approve/modify Constituent information so that our database contains the most up-to-date information on them.
                [T] Set up continuous deployment
                [T] Allow only administrators to modify information on Constituents
                [T] Common user can propose edits to accounts, but proposals need to be verified by the administrator
        [U] As an administrator I want to create new Constituent entries so that we can keep track of donations.
                [T] Only administrators can create new entries.
                [T] Volunteers can only propose new entries.
                [T] Fields will be verified to guarantee that each field contains content.
        [U] As a volunteer I need to be able to create an account so that I can view the data I need for making phone calls.
        [U] As an manager, I need to be able to create a administrator account so I can control who has access to our software.
        [U] As an administrator I want to be able to approve any new caller accounts so that I can confirm they have approval to view private information.
        [U] As a administrator, I need to be able to change the rolls of our callers so that if they become administrators in the future, they can have administrator’s access
        [U] As a volunteer, I need to change my email address, so I will always be able to log in
        [U] As a Constituent, I want my personal data kept private, so it does not get sold to third-party companies.
            [T] Have administrators and callers required to log in to access any Constituent data.
            [T] Only administrators can alter customer data
            [T] Standard users can only propose alterations to customer data

[E] Ability to connect to an external phone calling and management API
    [F] As a caller, I want to easily make phone calls, so I don’t waste time doing meaningless tasks.
        [U] As a volunteer, I want to click a phone number on the screen and have my computer call the Constituent, so I don’t have to manually dial it.
                [T] Have the view of the phone number be clickable, allowing the caller to click the number and have a phone call.
                [T] Use an API, such as Twilio, that allows you to make phone calls directly from your computer.
        [U] As a volunteer, I want to know how many calls I will be making during a shift so I can pace myself accordingly.
                [T] Each volunteer will be assigned a certain number of calls to make per shift.
        
[F] Use an algorithm to sort through our database
    [U] As an administrator I want to use an algorithm to assign callers to our volunteers to increase efficiency.
        [T] Callers who make the most money from Constituents will be assigned the most high-profile Constituents.
        [T] Callers who make the most money from Constituents will be assigned the most low-profile Constituents.
    [U] As an administrator I want to review call history to ensure schedules and goals are being met.
            [T] All calls will be logged within the database call history.

[E] Set up a content management system within our asp.net project

[E] Show user reports, tracking information about sales, the number of calls made, and other important data for use in an algorithm.
    [F] Generate user reports
        [U] As an administrator, I want to see daily, weekly, monthly, and yearly reports, so I can visualize data from callers and Constituents.
        [U] As an administrator, I want the number of calls made tracked, so I know if the callers are actually working or not.

[F] A dashboard for making the UI user-friendly
    [U] As a administrator, I want a dashboard where I can view the most relevant information so that I can quickly view information important to me. 
    [U] As a caller/standard user, I want a dashboard where I can view the most relevant information so that I can quickly view information important to me. 
```
#### Non-Functional Requirements ####
* Must work in all languages and countries, with English as the default language.
* Database must be hosted through Azure.
* Constituent accounts and data must be stored indefinitely
* Site should never return debug error pages. Web server must never return 404's. All server errors must be logged. Constituents should receive a custom error page in that case telling them what to do.
* Site and data must be backed up regularly and have failover redundancy that will allow the site to remain functional in the event of loss of primary web server or primary database.
* Donations should be posted to a Constituents account within one (1) business day.
* Website must support color-blind mode.

### Architecture Design ###
* C#
* ASP.NET
* MVC
* Javascript
* JSON/AJAX
* HTML/CSS
* Visual Studio IDE
* SQL Server Management Studio (Local Testing)
* SQL Server in Azure (Deployed Site)
* Database on Azure Server (Deployed Database)
* Twilio API
* Client using web browser


### Initial Modeling  ###


#### Site Architecture ####
![Site_Architecture](https://bitbucket.org/stuartjash/float-none-_stuartjash/raw/ad82db5b4d934967f42f9661293cf96177730155/milestones/milestone-4/images/Dialogistic_Site_Architecture_Diagram.PNG)

#### Use Case Diagram ####
![Use_Case](https://bitbucket.org/stuartjash/float-none-_stuartjash/raw/ad82db5b4d934967f42f9661293cf96177730155/milestones/milestone-4/images/Use_Case_Diagram_For_Dialogistic_App.PNG)

#### Agile Entity Relationship Diagram ####
![ER_Diagram](https://bitbucket.org/floatnone/float-none/raw/550ee3020e6b2d8144d1a097ec1722101273afdd/milestones/milestone-4/images/erd.jpg)

#### Site Layout ####
[Site walkthrough](https://xd.adobe.com/view/93d372da-fe32-4918-504a-ec7c6524a0e7-e5e2/)

### Project Risks ###
#####LOW RISK - Very unlikely that this will occur during the life of the project
* Risks to the project caused by requirements that are inadequately defined.
* Risks associated with personnel assigned to the project who may be pulled off anytime for another assignment.
* A risk to the project of a facility move during the project.
* Team member quits school.
* Global warming is worse than we thought and we end up like the movie Waterworld

#####MEDIUM RISK - There is a 50-50 chance that this will occur during the life of the project 
* The chance that the workstation environment of the developers will change after requirements are gathered.
* A chance that system owner or user support staff required to be available to the development team during the software development cycle will not be available. (Take into account situations such as use/lose leave, vacation, training, travel, and meetings).
* Risks with the hardware and software (the development platform) chosen to perform project development. e.g., can this hardware and software handle the workload required to complete the project?. 
Unforeseen injury or illness could cause a team member to be absent for long spans of time.
* The President will cause a nuclear war driving us into nuclear winter if we survive.

#####HIGH RISK - Very likely that this will occur during the life of the project 
* The chance that changes, in critical personnel, will occur during the life of the project.
* A risk to the project resulting from a mandated/mandatory completion date for the project.
* Acquisition of items critical to project success (e.g., hardware and/or software resources) could be delayed in the procurement process.
* Risks associated with any conversions of existing data required before implementation of a new system.
* The risk associated with the conversion of modified data back into CSV format
* Class load of a team member prevents accomplishing everything assigned to the aforementioned team member.
* The lizardmen team up with the mole people to strike at the surface dwellers.


### Timeline & Release Plan ###
* February 11, 2019 - Induction Phase End | Plan Sprint 1
* February 25, 2019 - End Sprint 1 | Plan Sprint 2 (Release Date 1)
* March 11, 2019 - End Sprint 2 | Plan Sprint 3 (Release Date 2)
* March 25, 2019 - End Sprint 3 | Plan Sprint 4 (Release Date 3)
* April 8, 2019 - End Sprint 4 | Plan Sprint 5 (Release Date 4)
* April 22,, 2019 - End Sprint 5 | Plan Sprint 6 (Release Date 5)
* TBA - Alpha & Beta Testing
* May 6, 2019 - End Sprint 6 | Start Final Bug Fixes (Release Date 6)
* May 20, 2019 (Final Release / Showcase)
