using AutoMapper;
using Hahn.Data.Context;
using Hahn.Data.Interfaces.Repositories;
using Hahn.Data.Repositories.BaseRepository;
using Hahn.Domain.Entities;

namespace Hahn.Data.Repositories
{
    public class EventsRepository : GenericRepository<Events>, IEventsRepository
    {
        public EventsRepository(HahnDbContext context, IMapper mapper) : base(context, mapper)
        {
        }
    }
}
