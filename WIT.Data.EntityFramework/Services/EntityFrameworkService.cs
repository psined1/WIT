using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIT.Interfaces;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using WIT.Data.Models;

namespace WIT.Data.Services
{
   
    public class EntityFrameworkService : IDataRepository, IDisposable
    {
        private Entities _db = new Entities();

        /// <summary>
        /// Database Context
        /// </summary>
        protected Entities db
        {
            get { return _db; }
        }

        public void CreateSession()
        {
            //Database.SetInitializer(new MigrateDatabaseToLatestVersion<CodeProjectDatabase, Configuration>()); 
        }

        public void BeginTransaction()
        {
        }

        /// <summary>
        /// Commit Transaction
        /// </summary>
        /// <param name="closeSession"></param>
        public void CommitTransaction(Boolean closeSession)
        {
            db.SaveChanges();
        }

        /// <summary>
        /// Rollback Transaction
        /// </summary>
        /// <param name="closeSession"></param>
        public void RollbackTransaction(Boolean closeSession)
        {
        }

        public void CloseSession()
        {
        }

        /// <summary>
        /// Dispose of connection
        /// </summary>
        public virtual void Dispose()
        {
            if (_db != null)
                _db.Dispose();
        }
    }
}
