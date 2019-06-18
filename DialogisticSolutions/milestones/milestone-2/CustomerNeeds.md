## Summary
The project detailed herein is a social-driven news networking site. We will be using Scrum, an Agile framework, in order to deliver the product to our customer, using the Disciplined Agile Delivery methodology. This product will be a basic, lower-level alternative to website like Reddit, where our focus will be on the news and news topics.

## Vision
For commenters who need to be able discuss various topics in a productive manner independent of the source of the topic in question, the Discussion Hub is a discussion site that will provide a centralized arena for carrying out such conversations. The website will maintain an individual’s account, maintain a history, indicate trustworthiness of the commenter, allow for the creation, search, and independent following of discussion pages concerning any news article, post, or web site. This system is meant to allow a user to participate in such activities via their own created identity while maintaining privacy and separation from any other social media identity. Unlike current available social media, our software product will enable individuals to openly examine all topics of interests from any source in a manner intended to promote civil, useful, productive, and transparent communication.

## Questions
* How do we link a discussion on the site to one or more articles/pages?
* How will users find out that there is a discussion on the site for the article/page they're currently viewing?
* Do we allow people to comment anonymously? Read anonymously?
* Do we allow people to sign up with a pseudonym or will we demand/enforce real names?
* What is it important to know about our users? What data should we collect?
* If there are news articles on multiple sites that are about the same topic should we have separate discussion pages or just one?
* What kind of discussion do we want to create? Linear traditional, chronological, ranked, or ?
* Should we allow image/video uploads and host them ourselves?
* Who's your target audience?
* Who is your main competitor?
* What image, look, or feel do you want your website to portray?
* When analyzing your competitors’ sites, what do you like and not like about their websites?
* What is the most important information your site must relay to the user, especially on the home page?

## Interviews
[Q] How do we link a discussion on the site to one or more articles/pages?
[A] We were thinking via URL.

[Q] How will users find out that there is a discussion on the site for the article/page they're currently viewing?
[A] How about a browser plug-in? It could send the URL of the current page to our API to see if a discussion page exists and provide an easy way for them to navigate to our page.
Or the user can copy the URL and paste it into a search bar on our site. Clearly we need accounts and logins. Our own, or do we allow logging in via 3rd party, i.e. "Log in with Google" or ...?

[Q] Do we allow people to comment anonymously? Read anonymously?
[A] I feel we should allow for anonymity in reading but not if they want to be a part of the discussion.

[Q] Do we allow people to sign up with a pseudonym or will we demand/enforce real names?
[A] Enforcing real names limits the discussion and as seen on youtube comments does not actually do anything to prevent negative comments.

[Q] What is it important to know about our users? What data should we collect?
[A] Email, password, real name

[Q] If there are news articles on multiple sites that are about the same topic should we have separate discussion pages or just one?
[A] Each article might have a different viewpoint so it would be good to have combine into a single discussion page which you could view one or more articles.

[Q] What kind of discussion do we want to create? Linear traditional, chronological, ranked, or ?
[A] Lets make it chronological, to encourage users to discuss as they read the daily news and view the record of previous days discussions.

[Q] Should we allow image/video uploads and host them ourselves?
[A] Lets not allow image uploading, instead using the link to the article pull the primary image and display that.

[Q] Who's your target audience?
[A] Adults, aged 18-60, from many walks of life who are willing to have civil discussions about the news on the internet.

[Q] Who is your main competitor?
[A] Reddit

[Q] What image, look, or feel do you want your website to portray?
[A] We want a clean modern look with bright pops of color, good use of white space and easy to read through long blocks of discussions.

[Q] When analyzing your competitors’ sites, what do you like and not like about their websites?
[A] So we dont like the expansiveness of Reddit, it is hard to navigate and unless you know you're looking for a specific subreddit its unlikely you will ever find new things.

[Q] What is the most important information your site must relay to the user, especially on the home page?
[A] We want to promote trending article discussions on the homepage, along with topics that are on the rise. Our main focus is on the topics that the users find most fascinating/popular. Breaking news/stories will take precedence at the top of the page, but the popularity will then be judged by the users.

## Needs and Features
* A great looking landing page with info to tell the user what our site is all about and how to use it. Include a link to and a page with more info. Needs a page describing our company and our philosophy.
* The ability to create a new discussion page about a given article/URL. This discussion page needs to allow users to write comments.
* The ability to find a discussion page.
* User accounts
* Accounts password protected
* The ability to login with your email address or username
* A user needs to be able to keep track of things they've commented on and easily go back to those discussion pages. If someone rates or responds to their comment we need to alert them.
* Allow users to identify fundamental questions and potential answers about the topic under discussion. Users can then vote on answers.
* Post trending functionality biased on votes.
* Post rising based on interaction within the last few hours.
* Posts default to chronological order but can be sorted into the popular content (most interaction recently)
* Allow users to post anonymously if they desire.
* Deletion of account, which will delete the user from the database for user security
* Allow users to vote on stories, which will increase the popularity of the story.
* Comments will cause stories to gain in popularity.

## Initial Modeling
### Use Case Diagrams
![Use Case Diagram](https://bytebucket.org/floatnone/float-none/raw/e34cded6eacde68b65a5e7a5954aca1ebf4b2abb/milestones/milestone-2/images/use_case_diagram.png)

### Other Modeling
#### Site Archetecture
![Arch](https://bitbucket.org/floatnone/float-none/raw/e34cded6eacde68b65a5e7a5954aca1ebf4b2abb/milestones/milestone-2/images/arch.jpg)

#### Site Layout
![Sitemap](https://bitbucket.org/floatnone/float-none/raw/e34cded6eacde68b65a5e7a5954aca1ebf4b2abb/milestones/milestone-2/images/site_layout.jpg)

## Non-Functional Requirements
* User accounts and data must be stored indefinitely

* Site and data must be backed up regularly and have failover redundancy that will allow the site to remain functional in the event of loss of primary web server or primary database. We can live with 1 minute of complete downtime per event and up to 1 hour of read-only functionality before full capacity is restored.

* Site should never return debug error pages. Web server must never return 404's. All server errors must be logged. Users should receive a custom error page in that case telling them what to do.

* Must work in all languages and countries. English will be the default language but users can comment in their own language and we may translate it.

* Database must be hosted through Azure.

* Discussion pages and their respective comments must be fetched from the database in under 300ms.

* Users must be notifed of replies to their comments within 3 minutes of the reply.

* Discussion pages should be refreshed through alerts via push notifications in order to show up-to-date comments.

* Search results must be returned to users in under 100ms.

* Comments should be uploaded to the database and posted to discussion threads in under 100ms.

## Identify Functional Requirements (User Stories)
```
E: Epic
F: Feature
U: User Story
T: Task
[U] As a visitor to the site I would like to see a fantastic and modern homepage that tells me how to use the site so I can decide if I want to use this service in the future.
    [T] Create starter ASP dot NET MVC 5 Web Application with Individual User Accounts and no unit test project
    [T] Switch it over to Bootstrap 4
    [T] Create nice homepage: write content, customize navbar
    [T] Create SQL Server database on Azure and configure web app to use it. Hide credentials.
[U] Fully enable Individual User Accounts
    [T] Copy SQL schema from an existing ASP.NET Identity database and integrate it into our UP script
    [T] Configure web app to use our db with Identity tables in it
    [T] Create a user table and customize user pages to display additional data
[F] Allow logged in user to create new discussion page
[F] Allow any user to search for and find an existing discussion page
[E] Allow a logged in user to write a comment on an article in an existing discussion page
    [F] Allow a user to post to the site anonymously
        [U] On account creation, I want to choose whether or not to make my posts anonymous
            [T] Create a anonymous checkbox within the UI   
[U] As a robot I would like to be prevented from creating an account on your website so I don't ask millions of my friends to join your website and add comments about male enhancement drugs.
    [T] Use CAPTCHA/RECAPTCHA
[E] As a logged in user, I want full control over my account with respect to dicussions on articles, from creation to interaction, and self-removal so that I can engage in truly free discourse.
    [U] As a logged in user, I want the option to create a discussion for an article, so that I can discuss it with others.
        [T] Allow logged-in users to create a discussion page
    [U] As a logged in user, I want to comment on existing discussion pages so that I can voice my opinion on a given article.
        [T] Provide a commenting system for logged-in users
    [U] As a logged in user, I want to vote / rate comments either up or down in order to give comments the recognition they deserve.
    [U] As a logged in user, I want to reply directly to existing comments so that it is clear who my message is for.
    [U] As a logged in user, I might want to deactivate / delete my account, and all of my existing comments so that I am no longer associated with the website.
        [T] Allow full-account deletion, not retaining any customer data for customer privacy and security.
[U] As a user / visitor, I want to see the most relevant (i.e., popular) comments first.
    [T] Use an algorithm to sort the most popular posts to the top of the UI
[U] As a visitor, I want the option to sign up for an account so that I can particpate in discussions.
[U] As a visitor, I want to see existing discussion pages, and their respective comments so that I can see what people are saying about a given article.
[U] As a moderator / admin, I want the ability to enforce site rules and remove comments that violate them in order to keep the discussions civil.
    [T] Create moderator / admin user permissions
[U] As a moderator / admin, I want the ability to revoke accounts / ban repeat offenders of site rules to help prevent toxicity among users.
    [T] Allow moderators to ban specific revoke access by blacklisting email addresses
```

## Initial Architecture Envisioning
* C#
* ASP.NET
* MVC
* Javascript
* JSON/AJAX
* HTML/CSS
* Visual Studio IDE
* SQL Server Management Studio
* Local server for testing
* SQL Server in Azure
* Database on Azure Server
* Webhose News API
* Client using web browser

![](images/arch.jpg?raw=true)

## Agile Data Modeling
![database model](https://bitbucket.org/floatnone/float-none/raw/e34cded6eacde68b65a5e7a5954aca1ebf4b2abb/milestones/milestone-2/images/Class_database.png)

## Timeline and Release Plan for team project

* January 23, 2019 - Induction Phase End | Plan Sprint 1
* February 5, 2019 - End Sprint 1 | Plan Sprint 2
* February 19, 2019 - End Sprint 2 | Plan Sprint 3
* March 5, 2019 - End Sprint 3 | Plan Sprint 4
* March 19, 2019 - End Sprint 4 | Plan Sprint 5
* April 2, 2019 - End Sprint 5 | Plan Sprint 6
* April 16, 2019 - End Sprint 6 | Plan Sprint 7
* April 30, 2019 - End Sprint 7 | Plan Sprint 8
* May 14, 2019 - End Sprint 8 | Plan Sprint 9
* May 28, 2019 - End Sprint 9 | Prepare for Release
* June 4, 2019 - Release Planning meeting
* June 11, 2019 - RELEASE DATE
