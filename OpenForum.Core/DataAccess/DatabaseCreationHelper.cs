using System;
using System.IO;
using OpenForum.Core.Properties;

namespace OpenForum.Core.DataAccess
{
    public static class DatabaseCreationHelper
    {
        public static void EnsureDatabaseExists(string defaultConnectionString)
        {
            using (OpenForumDataContext context = new OpenForumDataContext(defaultConnectionString))
            {
                string finalDirectory = (string)AppDomain.CurrentDomain.GetData("DataDirectory");
                string finalPath = Path.Combine(finalDirectory, "OpenForum.mdf");
                                
                if (!File.Exists(finalPath))
                {
                    Directory.CreateDirectory(finalDirectory);
                    
                    string tempPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());

                    using (OpenForumDataContext tempContext = new OpenForumDataContext(tempPath + ".mdf"))
                    {
                        tempContext.CreateDatabase();
                        tempContext.ExecuteCommand(Resources.DefaultData);
                        tempContext.ExecuteCommand("Declare @name as varchar(100); set @name = DB_NAME(); Use master; exec sp_detach_db @name, 'true';");
                    }

                    File.Move(tempPath + ".mdf", finalPath);
                    File.Delete(tempPath + ".ldf");
                }
            }
        }
    }
}
