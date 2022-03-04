# Introduction 
This project is the WEB part of the conception called " KABI.NET ".
It's a PWA(Progressive Web App) Angular project that has a .NET Core Api backend from witch gets the data.

# Getting Started
In order to run the project you have run two simple commands:
1. npm install (this downloads all dependencies and packages)
2. ng s -o (this starts the project on your localhost and opens the browser for you)

# Build and Test
In order to build and test the project you need these commands:
1. ng build --prod (this builds the project and puts the static files in the output folder " /dist/kabinet-web "
2. To test it you have to eighter open http-server within the output folder or copy all the files from the folder and put them somewhere

# 3-rd Parties
We use some 3-rd parties in order the achieve the project targets.
This is the list of the main ones:
1. [Firebase] (https://firebase.google.com/) - This is our main platform for this web app. We use the hosting and the file storage.
2. [NGX-Translate] (http://www.ngx-translate.com) - We use this for the multilingual functionality.
3. [Stimulsoft Reports] (https://www.stimulsoft.com/en) - This is used for the admin reports.