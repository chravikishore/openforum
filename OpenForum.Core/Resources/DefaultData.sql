
INSERT OpenForum_Post VALUES('Mr_Question',	'6/26/2009 12:00:00 AM',	'Mr_Answer',	'6/26/2009 7:00:00 PM',	'What is OpenForum?', 'What is OpenForum? Who was it created by? Where can I learn more about it?',	1);
INSERT OpenForum_Reply VALUES(@@Identity,	'Mr_Answer',	'6/26/2009 7:00:00 PM',	'<p>OpenForum is a free forum that plugs into any .Net MVC application. It was developed by Eric Herbrandson (<a href="http://software.herbrandson.com" rel="nofollow">http://software.herbrandson.com</a>). </p>
<p>You can view a quick tutorial on setting up OpenForum at the following url <a href="http://www.youtube.com/watch?v=xgiuRFrP5sM" rel="nofollow">http://www.youtube.com/watch?v=xgiuRFrP5sM</a></p>');




INSERT OpenForum_Post VALUES('Mr_Question',	'6/26/2009 12:00:00 AM',	'Mr_Answer',	'6/26/2009 6:00:00 PM',	'Is it possible to change the look and feel of OpenForum?',	'I''ve got OpenForum installed and running within my MVC application, but the default look isn''t what I want. How do I change the look and feel?',	1);
INSERT OpenForum_Reply VALUES(@@Identity,	'Mr_Answer',	'6/26/2009 6:00:00 PM',	'<p>There are multiple ways to change the look and feel of OpenForum. If you''re happy with the html structure and just want to change the CSS then all you need to do is turn off the default styles and then provide your own styles via CSS. In order to turn off default styles, simply open your global.asax file and add OpenForumManager.IncludeDefaultStyles = false to Applicaiont_Start().</p>
<p>On the other hand, if you want to actually change the html, you can add a "Forum" directory under the "Views" directory in your MVC web application. Then, for each view that you want to override, simply add an .aspx file to your new "Forum" directory (i.e. if you want to provide a custom "Post" view, you would create a Post.aspx file).</p>
<p>For more details on changing the look and feel of OpenForum, view the following tutorial <a href="http://www.youtube.com/watch?v=2ZAyVlvSyeU" rel="nofollow">http://www.youtube.com/watch?v=2ZAyVlvSyeU</a><br></p>');




INSERT OpenForum_Post VALUES('Mr_Question',	'6/26/2009 12:00:00 AM',	'Mr_Answer',	'6/26/2009 5:00:00 PM',	'How do I integrate an existing security system with OpenForum?',	'I''m not using the default MVC authentication system. Instead, my application already has it''s own "users" database table. How do I integrate my custom authentication system with OpenForum?',	1);
INSERT OpenForum_Reply VALUES(@@Identity,	'Mr_Answer',	'6/26/2009 5:00:00 PM',	'<p>In order to use a custom "user" system with open forum, simply implement IUserRepository and register your custom repository with OpenForum by setting OpenForumManager.UserRepository to an instance of your class.</p>
<p>For more details, view the following tutorial <a href="http://www.youtube.com/watch?v=ko_98aig44M" rel="nofollow">http://www.youtube.com/watch?v=ko_98aig44M</a><br></p>');




INSERT OpenForum_Post VALUES('Mr_Question',	'6/26/2009 12:00:00 AM',	'Mr_Answer',	'6/26/2009 4:00:00 PM',	'How do I include additional data in the ViewData returned by the OpenForum controller?',	'My master page requires some specific data in the view data, but the OpenForum controller doesn''t provide that data by default. How do I extend the OpenForum controller to include additional custom data in the ViewData it passes to the views?',	1);
INSERT OpenForum_Reply VALUES(@@Identity,	'Mr_Answer',	'6/26/2009 4:00:00 PM',	'OpenForum provides an IViewModelFactory interface that you can implement in order to provide extended data in the ViewData sent to the forum views. To learn how, watch this tutorial <a href="http://www.youtube.com/watch?v=9b9Td5CFu2g" rel="nofollow">http://www.youtube.com/watch?v=9b9Td5CFu2g</a>');




INSERT OpenForum_Post VALUES('Mr_Question',	'6/26/2009 12:00:00 AM',	'Mr_Answer',	'6/26/2009 3:00:00 PM',	'Where is the data being stored?',	'I''ve got OpenForum installed on my dev box and every thing seems to be working. However, I can''t figure out where the data is being stored? What sort of black magic is going on here?',	1);
INSERT OpenForum_Reply VALUES(@@Identity,	'Mr_Answer',	'6/26/2009 3:00:00 PM',	'<p>By default, OpenForum uses SqlExpress to create a "User Instance" database (you can read more about user instance databases here: <a href="http://technet.microsoft.com/en-us/library/bb264564(SQL.90).aspx" rel="nofollow">http://technet.microsoft.com/en-us/library/bb264564(SQL.90).aspx</a>). The database files are created in the App_Data directory of your web project. OpenForum uses this functionality in order to make it as simple as possible to get a forum up and running quickly with a bare minimum of configuration. However, this most likely isn''t a desirable solution for a production environment. It is highly recommended that you provide a connection string in your web applications web.config file to your desired database. Here''s an example of what it should look like</p>
<p>&lt;connectionstrings&gt;<br>&nbsp;&nbsp;&nbsp;&nbsp; &lt;add name="OpenForum.Core.Properties.Settings.ConnectionString" connectionstring="&lt;your connection string here&gt;" providername="System.Data.SqlClient" /&gt;<br>&lt;/connectionstrings&gt;</p>
<p>You will also need to run the OpenForum setup SQL script against your database in order to create the needed schema. A setup script named Setup.sql should be available in the OpenForum download.<br></p>');

