using HushDomain.Repositories.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HushDomain.Entities;
using NHibernate.Cfg;
using NHibernate;

namespace HushDomain.Repositories.Concrete
{
    public class PostgresMessageRepository : IMessageRepository
    {
        private ISession GetSession()
        {
            Configuration config = new Configuration();
            config.Configure();
            config.AddAssembly(typeof(Message).Assembly);
            ISessionFactory sessionFac = config.BuildSessionFactory();
            return sessionFac.OpenSession();
        }

        private ITransaction GetTransaction()
        {
            return GetSession().BeginTransaction();
        }

        public void Add(Message message)
        {
            using (var session = GetSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    session.Save(message);
                    transaction.Commit();
                }
            }
        }

        public IEnumerable<Message> GetAll()
        {
            var session = GetSession();
            using (session)
            {
                return session.CreateCriteria(typeof(Message)).List<Message>();
            }
        }

        public Message GetById(long Id)
        {
            throw new NotImplementedException();
        }
    }
}
