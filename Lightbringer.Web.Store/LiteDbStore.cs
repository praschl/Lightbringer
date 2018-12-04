using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LiteDB;

namespace Lightbringer.Web.Store
{
    public class LiteDbStore : IStore
    {
        public void AddServiceHost(string name, string url)
        {
            using (var db = CreateSession())
            {
                var hosts = db.GetCollection<ServiceHost>();
                var host = new ServiceHost {Name = name, Url = url};
                hosts.Insert(host);
            }
        }

        private LiteDatabase CreateSession()
        {
            var fullDbFilePath = GetFullDbPath;

            var liteDatabase = new LiteDatabase(fullDbFilePath);
            return liteDatabase;
        }

        private static string _fullDbPath;
        private static string GetFullDbPath
        {
            get
            {
                if (_fullDbPath == null)
                {
                    string pathToDb = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "lightbringer\\db\\");
                    if (!Directory.Exists(pathToDb))
                        Directory.CreateDirectory(pathToDb);

                    string fullDbFilePath = Path.Combine(pathToDb, "lite.db");
                    _fullDbPath = fullDbFilePath;
                }

                return _fullDbPath;
            }
        }

        public IReadOnlyCollection<string> GetServiceHosts()
        {
            using (var db = CreateSession())
            {
                return db.GetCollection<ServiceHost>()
                    .FindAll()
                    .Select(sh => sh.Name)
                    .ToArray();
            }
        }
    }
}