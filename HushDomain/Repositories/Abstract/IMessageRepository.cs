using HushDomain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HushDomain.Repositories.Abstract
{
    public interface IMessageRepository
    {
        Message GetById(long Id);
        void Add(Message message);
        IEnumerable<Message> GetAll();
        IEnumerable<Message> GetWithinOf(double distanceMeters, GeoPoint origin);
    }
}
