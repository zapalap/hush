using HushDomain.Entities;
using HushDomain.Repositories.Abstract;
using HushDomain.Repositories.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace HushApi.Controllers
{
    [EnableCors(origins: "http://localhost:4400", headers: "*", methods: "*")]
    public class MessagesController : ApiController
    {

        private IMessageRepository Repository;

        public MessagesController()
        {
            Repository = new PostgresMessageRepository();
        }

        public IEnumerable<Message> Get()
        {
            return Repository.GetAll();
        }

        public Message Get(long id)
        {
            return Repository.GetById(id);
        }

        public Message Post(Message message)
        {
            Repository.Add(message);
            return message;
        }
        
        public IEnumerable<Message> Post(double distance, [FromBody]GeoPoint origin)
        {
            return Repository.GetWithinOf(distance, origin);
        }
    }
}