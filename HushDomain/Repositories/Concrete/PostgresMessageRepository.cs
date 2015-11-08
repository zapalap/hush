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

        /// <summary>
        /// Adds new message to database
        /// </summary>
        /// <param name="message"></param>
        public void Add(Message message)
        {
            message.DateCreated = DateTime.UtcNow;

            using (var session = GetSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    session.Save(message);
                    transaction.Commit();
                }
            }
        }

        /// <summary>
        /// Gets all messages
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Message> GetAll()
        {
            var session = GetSession();
            using (session)
            {
                return session.CreateCriteria(typeof(Message)).List<Message>();
            }
        }

        /// <summary>
        /// Gets specific message by id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public Message GetById(long Id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Queries database for all messages left within distanceMeters from origin
        /// </summary>
        /// <param name="distanceMeters"></param>
        /// <param name="origin"></param>
        /// <returns></returns>
        public IEnumerable<Message> GetWithinOf(double distanceMeters, GeoPoint origin)
        {
            var query = String.Format(
                "SELECT * FROM messages WHERE ST_DWithin(geography(geom), geography(ST_SetSRID(ST_Point({0}, {1}), 4326)), {2})", 
                origin.Longitude, 
                origin.Latitude, 
                distanceMeters);

            var session = GetSession();
            using (session)
            {
                return session
                    .CreateSQLQuery(query)
                    .AddEntity(typeof(Message)).List<Message>();
            }
        }
    }
}
