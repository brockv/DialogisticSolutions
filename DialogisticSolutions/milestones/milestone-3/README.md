# Milestone 3 #

## Team Project - Dialogistic (Phone-a-thon Software) ##

### Mind Map ###

![MindMap](https://bitbucket.org/stuartjash/float-none-_stuartjash/raw/6cadef4fbc4eb9e25d6e7b3d24624853775809dd/milestones/milestone-3/images/mindmap.jpg)


### Vision ###
For fundraisers who need phone-a-thon donor data organization and scheduling, **Dialogistic** is a data management system that will assist in the managing of a phone-a-thon, displaying information on past constituents and potential constituents. It will also allow volunteer fundraisers to be assigned people to call and allow them to submit changes to data records which will be reviewed and approved by an administrator. Unlike the current manual google document process for handling this information and call assignment, our product will allow for CSV importing/exporting and will algorithmically assign calls to volunteers based on history.
 
### Proposal ###
We would like to develop a web application that would be implemented by businesses or non-profit organizations that need to contact large amounts of customers. This could be used to get donations, phone-a-thons, or marketing.

### Initial Description ###
This project would be a web application that would assist in the managing of a phone-a-thon. This would be targeted at non-profit organizations or businesses that contact potential donors (constituents) in an attempt to receive donations. This application would display a constituents contact information that would include but is not limited to, their name, prefix, address, phone number, past donation amounts (if applicable), the time they were contacted, and if the last time they were called they answered. 

The callers would enter in donation amounts that would be in the form of either credit/debit card payments or pledges. If the constituent wishes to donate via pledge, which is usually because they do not wish to give their credit card information over the phone, and caller or administrator would send out an envelope via USPS, so that constituent could return it with a check for the pledge amount enclosed.

Before the money is officially received, the amount must be confirmed by an administrator, with general users not having the ability to facilitate changes to caller information. For any constituent updates, such as change of address, new phone number, change in marital status, an administrator’s approval would be required.

### List of Needs & Features / Requirements ###
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
* Ability in export call lists for a certain period of time (daily, weekly, monthly, yearly, custom)
* Inability for standard users to alter criteria and information about constituents. They can propose a change that then is approved by an administrator.



