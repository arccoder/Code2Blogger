# Code2Blogger#

Code2Blogger enables semi automated code publishing to blogger directly from the source file.

Version 0.1

### What does the repository contains? ###

* Visual Studio 2010 C# Project

### How to use the Code2Blogger executable? ###

* To use the executable you need to install 
  [Microsoft .NET Framework 4](http://www.microsoft.com/en-us/download/details.aspx?displaylang=en&id=17851)
  
* Command line interface 
  Code2Blogger -[OPTIONS] Arguments
  -[OPTIONS]
  -h or -help
   Shows how to use Code2Blogger Application
  -u
   Email ID / Username of the blogger account
  -p
   Password of the blogger account
  -b
   Blog ID for the blog
   To find your Blog ID
   -  Log into your blogger account and open the dashboard of your blog
   -  In the URL you should find your 18 digit blog id
   -  https://www.blogger.com/blogger.g?blogID=XXXXXXXXXXXXXXXXXX#overview/src=dashboard
  -t
   Name / Title of the blog to be posted
  -a
   Name of the author
  -i
   Path to the code file
   Currently supports only one file
  -o
   Path to the file containing the codes output

### Contribution guidelines ###

* Code review
  I would be happy if I get review comments on how I can improve the structure of the code, its performance, the interface or anything else regarding the app.

