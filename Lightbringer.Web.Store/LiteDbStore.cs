using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using LiteDB;

namespace Lightbringer.Web.Store
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

        public ServiceHost Get(int id)
        {
            using (var db = CreateSession())
            {
                var hosts = db.GetCollection<ServiceHost>();

                var serviceHost = hosts.FindById(id);
                
                return serviceHost;
            }
        }

        public ServiceHost Find(string url)
        {
            using (var db = CreateSession())
            {
                var hosts = db.GetCollection<ServiceHost>();

                return hosts.FindOne(sh => sh.Url == url);
            }
        }

        public void Upsert(ServiceHost serviceHost)
        {
            using (var db = CreateSession())
            {
                var hosts = db.GetCollection<ServiceHost>();

                hosts.Upsert(serviceHost);
            }
        }

        public IReadOnlyCollection<ServiceHost> FindAllHosts()
        {
            using (var db = CreateSession())
            {
                var hosts = db.GetCollection<ServiceHost>().FindAll();

                return hosts.ToArray();
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