﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LiteDB;

namespace Lightbringer.Web.Core.Store
{
    public class LiteDbStore : IStore
    {
        private readonly LiteDbStoreConfiguration _config;

        public LiteDbStore(LiteDbStoreConfiguration config)
        {
            _config = config;
        }

        private string _fullDbPath;

        private string GetFullDbPath
        {
            get
            {
                if (_fullDbPath == null)
                {
                    if (!string.IsNullOrEmpty(_config.DbPath))
                        return _config.DbPath;

                    var pathToDb = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "lightbringer\\database\\");
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