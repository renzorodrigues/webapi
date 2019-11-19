using System;
using FluentNHibernate.Automapping;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using MySql.Data.MySqlClient;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using WebApi.Data.Helpers.Interfaces;
using WebApi.Data.MappingOverrides;
using WebApi.Domain.Entities;

namespace WebApi.Data.Helpers
{
    public class UnitOfWork : IUnitOfWork
    {
        private static readonly ISessionFactory _sessionFactory;
        private ITransaction _transaction;
        public ISession Session { get; set; }

        static UnitOfWork() 
        {
            _sessionFactory = Fluently.Configure()
                .Database(MySQLConfiguration.Standard.ConnectionString(
                    x => x.Server("localhost").
                    // Username("pma"). //landix
                    // Password(""). //landix
                    Username("root"). //home
                    Password("123"). //home
                    Database("ccaudb")
                ))
                .Mappings(x => x.AutoMappings.Add(AutoMap.AssemblyOf<User>(new AutoMappingConfiguration()).UseOverridesFromAssemblyOf<UserMap>()))
                .Mappings(x => x.AutoMappings.Add(AutoMap.AssemblyOf<Attended>(new AutoMappingConfiguration()).UseOverridesFromAssemblyOf<AttendedMap>()))
                .ExposeConfiguration(config => new SchemaUpdate(config).Execute(false, true))
                .BuildSessionFactory();
        }

        public UnitOfWork()
        {
            Session = _sessionFactory.OpenSession();
        }
        
        public void BeginTransaction()
        {
            _transaction = Session.BeginTransaction();
        }

        public void Commit()
        {
            try
            {
                if (_transaction != null && _transaction.IsActive)
                    _transaction.Commit();
            }
            catch
            {
                if (_transaction != null && _transaction.IsActive)
                    _transaction.Rollback();

                throw;
            }
            finally
            {
                Session.Dispose();
            }
        }

        public void Rollback()
        {
            try
            {
                if (_transaction != null && _transaction.IsActive)
                    _transaction.Rollback();
            }
            finally
            {
                Session.Dispose();
            }
        }
    }
}