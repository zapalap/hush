using HushDomain.Entities;
using HushDomain.Repositories.Abstract;
using HushDomain.Repositories.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace HushApi.Controllers
{
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

        // GET api/values/5
        public Message Get(long id)
        {
            return Repository.GetById(id);
        }

        [HttpPost]
        public Message Post(Message message)
        {
            Repository.Add(message);
            return message;
        }

    }
}
