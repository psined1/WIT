﻿using System;
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

        //AngularDatabase _connection;
        private Entities _db = new Entities();

        /// <summary>
        /// Database Context
        /// </summary>
        //protected AngularDatabase dbConnection
        //{
        //    get { return _connection; }
        //}
        protected Entities db
        {
            get { return _db; }
        }

        /// <summary>
        /// Commit Transaction
        /// </summary>
        /// <param name="closeSession"></param>
        public void CommitTransaction(Boolean closeSession)
        {
            //dbConnection.SaveChanges();
            db.SaveChanges();
        }

        /// <summary>
        /// Rollback Transaction
        /// </summary>
        /// <param name="closeSession"></param>
        public void RollbackTransaction(Boolean closeSession)
        {

        }

        public void Save(object entity) { }
        public void CreateSession() 
        {
            //Database.SetInitializer(new MigrateDatabaseToLatestVersion<CodeProjectDatabase, Configuration>()); 

            //_connection = new AngularDatabase();
            _db = new Entities();
        }
        public void BeginTransaction() { }

        public void CloseSession() { }

        /// <summary>
        /// Dispose of connection
        /// </summary>
        public virtual void Dispose()
        {
            //if (_connection != null)
            //    _connection.Dispose();
            if (_db != null)
                _db.Dispose();
        }
    }
}
