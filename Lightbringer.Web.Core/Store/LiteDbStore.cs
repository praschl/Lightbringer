using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LiteDB;

namespace Lightbringer.Web.Store.Store
{
    public class LiteDbStore : IStore
    {
        private static string _fullDbPath;

        private static string GetFullDbPath
        {
            get
            {
                if (_fullDbPath == null)
                {
                    var pathToDb = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "lightbringer\\db\\");
                    if (!Directory.Exists(pathToDb))
                        Directory.CreateDirectory(pathToDb);

                    var fullDbFilePath = Path.Combine(pathToDb, "lite.db");
                    _fullDbPath = fullDbFilePath;
                }

                return _fullDbPath;
            }
        }

        public DaemonHost Get(int id)
        {
            using (var db = CreateSession())
            {
                var hosts = db.GetCollection<DaemonHost>();

                var daemonHost = hosts.FindById(id);
                
                return daemonHost;
            }
        }

        public DaemonHost Find(string url)
        {
            using (var db = CreateSession())
            {
                var hosts = db.GetCollection<DaemonHost>();

                return hosts.FindOne(sh => sh.Url == url);
            }
        }

        public void Upsert(DaemonHost daemonHost)
        {
            using (var db = CreateSession())
            {
                var hosts = db.GetCollection<DaemonHost>();

                hosts.Upsert(daemonHost);
            }
        }

        public IReadOnlyCollection<DaemonHost> FindAllHosts()
        {
            using (var db = CreateSession())
            {
                var hosts = db.GetCollection<DaemonHost>().FindAll();

                return hosts.ToArray();
            }
        }

        public void Delete(int id)
        {
            using (var db = CreateSession())
            {
                var hosts = db.GetCollection<DaemonHost>();

                hosts.Delete(id);
            }
        }

        private LiteDatabase CreateSession()
        {
            var fullDbFilePath = GetFullDbPath;

            var liteDatabase = new LiteDatabase(fullDbFilePath);
            return liteDatabase;
        }
    }
}