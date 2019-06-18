1. Fork the repository. Follow the Bitbucket documentation [here](https://confluence.atlassian.com/bitbucket/forking-a-repository-221449527.html)
    * Use the fork button on the right. You may have to click the ... in order to see the options
    * The format that others have used is to name it dialogistic_yourname so it is easier to identify for pulls.
2. Clone the repository to your local hard drive.
    * Click the clone button near the top of the left menu. You may need to click the ... if you don't see it.
    * Copy the link.
    * Paste the link in a directory on your harddrive that does not have another repository in it.
3. Once the repository is copied. Set your upstream to the main repository.
    * On the bitbucket page, copy the string found in the top left corner of the repo's Overview page.
    * Go to a command line prompt and in the directory with the repository type
    * `git remote add upstream <string you just copied>`
    * This will be where you pull the latest develop branch.
4. Once you get the repository, read the project foundation documentation found in the Milestones folders.
5. Acquire keys for the following APIs, and place them in your `appSettings.config` file in the directory above your project's root (`.git`) folder:
	* Twilio
	* CometChat
	* Stripe
	* Amazon S3
	* MapQuest
	* SendGrid
6. For initializing your database, please go [here](database.md)
7. After initialization of the database, you can import your list of Constituents (donors) via a .csv or .xls/.xlsx file. Feel free to use our `sample_upload.csv` file located in `Dialogisitic/sample_upload.csv`.

**Note:** Please let me know if you have any questions via [email](mailto:stuart.ashenbrenner@gmail.com).