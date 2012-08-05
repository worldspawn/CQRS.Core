using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using FluentMongo.Linq;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace CQRS.Core.ViewModel
{
    public interface IContextDtoRepositoryWrapper<TDto> : IDtoRepository<TDto>
        where TDto : IPersistedDto
    {
        Guid SourceId { get; set; }
    }

    public class ContextDtoRepositoryWrapper<TDto> : IContextDtoRepositoryWrapper<TDto>
        where TDto : IPersistedDto
    {
        private readonly IDtoRepository<TDto> _dtoRepository;
        private readonly IBus _bus;

        public ContextDtoRepositoryWrapper(IDtoRepository<TDto> dtoRepository, IBus bus)
        {
            _dtoRepository = dtoRepository;
            _bus = bus;
        }

        public Guid SourceId { get; set; }

        public bool Any(Expression<Func<TDto, bool>> criteria)
        {
            return _dtoRepository.Any(criteria);
        }

        public TDto Single(Expression<Func<TDto, bool>> criteria)
        {
            return _dtoRepository.Single(criteria);
        }

        public int Count(Expression<Func<TDto, bool>> criteria)
        {
            return _dtoRepository.Count(criteria);
        }

        public TDto Delete(TDto entity)
        {
            var result = _dtoRepository.Delete(entity);
            _bus.Send(
                   new ViewModelUpdatedEvent<TDto> { Dto = result, UpdateType = ViewModelUpdateType.Delete, SourceId = SourceId}
                   );

            return result;
        }

        public IEnumerable<TDto> Find()
        {
            return _dtoRepository.Find();
        }

        public IEnumerable<TDto> Find(Expression<Func<TDto, bool>> criteria)
        {
            return _dtoRepository.Find(criteria);
        }

        public IEnumerable<TDto> Find(Expression<Func<TDto, bool>> criteria, Expression<Func<TDto, object>> orderBy, int skip = 0, int? take = new int?())
        {
            return _dtoRepository.Find(criteria, orderBy, skip, take);
        }

        public TDto Insert(TDto entity)
        {
            var result = _dtoRepository.Insert(entity);
            _bus.Send(
                   new ViewModelUpdatedEvent<TDto> { Dto = result, UpdateType = ViewModelUpdateType.Insert, SourceId = SourceId }
                   );

            return result;
        }

        public TDto Update(TDto entityToUpdate)
        {
            var result = _dtoRepository.Update(entityToUpdate);
            _bus.Send(
                   new ViewModelUpdatedEvent<TDto> { Dto = result, UpdateType = ViewModelUpdateType.Update, SourceId = SourceId }
                   );

            return result;
        }
    }

    public class DtoRepository<TDto> : IDtoRepository<TDto> 
        where TDto : IPersistedDto
    {
        private readonly MongoCollection<TDto> _collection;

        public DtoRepository(MongoDatabase mongoDatabase)
        {
            var type = typeof (TDto);
            if (!mongoDatabase.CollectionExists(type.Name))
                mongoDatabase.CreateCollection(type.Name);
            _collection = mongoDatabase.GetCollection<TDto>(type.Name, SafeMode.True);
        }

        #region IViewModelRepository<TDto> Members

        public virtual TDto Single(Expression<Func<TDto, bool>> criteria)
        {
            return _collection.AsQueryable()
                .SingleOrDefault(criteria);
        }

        public virtual TDto Insert(TDto entity)
        {
            if (entity.Id == default(Guid))
                entity.Id = Guid.NewGuid();

            _collection.Insert(entity);

            return entity;
        }

        public virtual TDto Delete(TDto entity)
        {
            IMongoQuery query = Query<TDto>.EQ(e => e.Id, entity.Id);
            _collection.Remove(query, RemoveFlags.Single);
            return entity;
        }

        private IQueryable<TDto> ApplyWhere(MongoCollection<TDto> collection, Expression<Func<TDto, bool>> criteria)
        {
            if (criteria != null)
                return collection.AsQueryable().Where(criteria);

            return collection.AsQueryable();
        }

        public virtual IEnumerable<TDto> Find()
        {
            return Find(null);
        }

        public virtual IEnumerable<TDto> Find(Expression<Func<TDto, bool>> criteria)
        {
            return ApplyWhere(_collection, criteria);
        }

        public virtual IEnumerable<TDto> Find(Expression<Func<TDto, bool>> criteria,
                                              Expression<Func<TDto, object>> orderBy, int skip = 0, int? take = null)
        {
            if (orderBy == null) throw new ArgumentNullException("orderBy");
            var result = (IQueryable<TDto>) ApplyWhere(_collection, criteria).OrderBy(orderBy);
            if (skip > 0)
                result = result.Skip(skip);
            if (take.HasValue)
                result = result.Take(take.Value);

            return result;
        }

        public virtual bool Any(Expression<Func<TDto, bool>> criteria)
        {
            return _collection.AsQueryable().Any(criteria);
        }

        public virtual int Count(Expression<Func<TDto, bool>> criteria)
        {
            return _collection.AsQueryable().Count(criteria);
        }

        public virtual TDto Update(TDto entityToUpdate)
        {
            IMongoQuery query = Query<TDto>.EQ(e => e.Id, entityToUpdate.Id);
            IMongoUpdate update = Update<TDto>.Replace(entityToUpdate);
            _collection.Update(query, update);

            return entityToUpdate;
        }

        #endregion
    }
}