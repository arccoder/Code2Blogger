using System;
using System.IO;
using System.Net;
using System.Xml;
using System.Text;
using System.Text.RegularExpressions;
using Google.GData.Client;

namespace BloggerDevSample
{
    class ConsoleSample
    {
        public struct BlogPost
        {
            public string postTitle;
            public string postAuthor;
            public string codeFile;
            public string outFile;
        }

        /* Creates a new blog entry and sends it to the specified Uri */
        static AtomEntry PostNewEntry(Service service, Uri blogPostUri, BlogPost bP)
        {
            Console.WriteLine("\nPublishing a blog post");
            AtomEntry createdEntry = null;
            if (blogPostUri != null)
            {
                // Construct the new entry
                AtomEntry newPost = new AtomEntry();
                newPost.Title.Text = bP.postTitle;
                newPost.Content = new AtomContent();
                newPost.Content.Type = "html";
                newPost.Authors.Add(new AtomPerson());
                newPost.Authors[0].Name = bP.postAuthor;
                //newPost.Authors[0].Email = "";

                string fc = "";
                if (bP.outFile.Length > 0)
                {
                    if (File.Exists(bP.codeFile))
                    {
                        // Parse the code file
                        fc = File.ReadAllText(bP.codeFile);
                        fc = fc.Replace("<", "&lt;");
                        fc = fc.Replace(">", "&gt;");

                        // Append the code to the post
                        newPost.Content.Content = "<script src=\"https://google-code-prettify.googlecode.com/svn/loader/run_prettify.js\"></script>" +
                                                  "<h2>Code</h2>" + "<pre class=\"prettyprint\">" + fc + "</pre>";
                    }
                    else
                    {
                        Console.WriteLine("\nCode file does not exist.\n");
                    }
                }

                if (bP.outFile.Length > 0)
                {
                    if (File.Exists(bP.outFile))
                    {
                        // Parse the output file 
                        fc = File.ReadAllText(bP.outFile);
                        fc = fc.Replace("<", "&lt;");
                        fc = fc.Replace(">", "&gt;");

                        // Append the code output to the post
                        newPost.Content.Content += "<h2>Output</h2>" + fc;

                    } else {
                        Console.WriteLine("\nOutput file does not exist.\n");
                    }
                }

                createdEntry = service.Insert(blogPostUri, newPost);
                if (createdEntry != null)
                {
                    Console.WriteLine("  New blog post created with title: " + createdEntry.Title.Text);
                }
            }
            return createdEntry;
        }

        static void printHelp()
        {
            Console.WriteLine("How To Post Code to Blogger");
            Console.WriteLine("Usage :");
            Console.WriteLine("Code2Blogger -[OPTIONS] Arguments");
            Console.WriteLine("-[OPTIONS]");
            Console.WriteLine("-h or -help");
            Console.WriteLine(" Shows how to use Code2Blogger Application");
            Console.WriteLine("-u");
            Console.WriteLine(" Email ID / Username of the blogger account");
            //Console.WriteLine("-p");
            //Console.WriteLine(" Password of the blogger account");
            Console.WriteLine("-b");
            Console.WriteLine(" Blog ID for the blog");
            Console.WriteLine("To find your Blog ID");
            Console.WriteLine("-  Log into your blogger account and open the dashboard of your blog");
            Console.WriteLine("-  In the URL you should find your 18 digit blog id");
            Console.WriteLine("-  https://www.blogger.com/blogger.g?blogID=XXXXXXXXXXXXXXXXXX#overview/src=dashboard");
            Console.WriteLine("-t");
            Console.WriteLine(" Name / Title of the blog to be posted");
            Console.WriteLine("-a");
            Console.WriteLine(" Name of the author");
            Console.WriteLine("-i");
            Console.WriteLine(" Path to the code file");
            Console.WriteLine(" Currently supports only one file");
            Console.WriteLine("-o");
            Console.WriteLine(" Path to the file containing the codes output");
        }

        static void Main(string[] args)
        {
            Service service = new Service("blogger", "blogger-example");

            string username = "";
            string password = "";
            string blogID = "";
            Uri blogPostUri = null;
            BlogPost bP = new BlogPost();

            int numArgs = args.GetLength(0);
            int iter = 0;
            while(iter < numArgs)
            {
                args[iter] = args[iter].ToLower();

                if (String.Compare("-h", args[iter], true) == 0 || 
                    String.Compare("-help", args[iter], true) == 0)
                {
                    printHelp();
                    Console.WriteLine();
                    Console.WriteLine("Press enter to quit");
                    Console.ReadLine();
                    System.Environment.Exit(0);
                }

                if (String.Compare("-u", args[iter], true) == 0)
                {
                    ++iter;
                    username = args[iter];
                    ++iter;
                    continue;
                }
                /*
                if (String.Compare("-p", args[iter], true) == 0)
                {
                    ++iter;
                    password = args[iter];
                    ++iter;
                    continue;
                }
                */
                if (String.Compare("-b", args[iter], true) == 0)
                {
                    ++iter;
                    blogID = args[iter];
                    ++iter;
                    continue;
                }

                if (String.Compare("-t", args[iter], true) == 0)
                {
                    ++iter;
                    bP.postTitle = args[iter];
                    ++iter;
                    continue;
                }

                if (String.Compare("-a", args[iter], true) == 0)
                {
                    ++iter;
                    bP.postAuthor = args[iter];
                    ++iter;
                    continue;
                }

                if (String.Compare("-i", args[iter], true) == 0)
                {
                    ++iter;
                    bP.codeFile = args[iter];
                    ++iter;
                    continue;
                }

                if (String.Compare("-o", args[iter], true) == 0)
                {
                    ++iter;
                    bP.outFile = args[iter];
                    ++iter;
                    continue;
                }

                ++iter;
            }

            if (username.Length == 0)
            {
                Console.WriteLine("Please provide a username");
                Console.WriteLine("Press enter to quit");
                Console.ReadLine();
                System.Environment.Exit(0);
            }

            // ASK FOR PASSWORD
            int numPassTrials = 3;

            while (numPassTrials > 0)
            {
                Console.WriteLine("Enter your password for");
                Console.WriteLine("username : " + username);
                Console.Write("password : ");
                ConsoleKeyInfo keyInput;
                do
                {
                    keyInput = Console.ReadKey(true);

                    // Backspace Should Not Work
                    if (keyInput.Key != ConsoleKey.Backspace && keyInput.Key != ConsoleKey.Enter)
                    {
                        password += keyInput.KeyChar;
                        Console.Write("*");
                    }
                    else
                    {
                        if (keyInput.Key == ConsoleKey.Backspace && password.Length > 0)
                        {
                            password = password.Substring(0, (password.Length - 1));
                            Console.Write("\b \b");
                        }
                    }
                }
                // Stops Receving Keys Once Enter is Pressed
                while (keyInput.Key != ConsoleKey.Enter);

                if (password.Length == 0)
                {
                    //Console.WriteLine("Please provide a password");
                    //Console.WriteLine("Press enter to quit");
                    //Console.ReadLine();
                    //System.Environment.Exit(0);
                }
                else
                {
                    try
                    {
                        service.Credentials = new GDataCredentials(username, password);
                        break;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("{0} Exception caught.", e);
                    }
                    
                }
            }

            if (blogID.Length > 0)
                blogPostUri = new Uri("https://www.blogger.com/feeds/" + blogID + "/posts/default");
            else
            {
                Console.WriteLine("Please provide a blogID");
                Console.WriteLine("Press enter to quit");
                Console.ReadLine();
                System.Environment.Exit(0);
            }

            AtomEntry Entry = Entry = PostNewEntry(service, blogPostUri, bP);

            Console.WriteLine("Press enter to quit");
            Console.ReadLine();
        }
    }
}
