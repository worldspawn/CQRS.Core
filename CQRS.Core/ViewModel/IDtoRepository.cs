using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace CQRS.Core.ViewModel
{
    public interface IDtoRepository<TDto>
        where TDto : IPersistedDto
    {
        bool Any(Expression<Func<TDto, bool>> criteria);
        TDto Single(Expression<Func<TDto, bool>> criteria);
        int Count(Expression<Func<TDto, bool>> criteria);
        TDto Delete(TDto entity);
        IEnumerable<TDto> Find();
        IEnumerable<TDto> Find(Expression<Func<TDto, bool>> criteria);

        IEnumerable<TDto> Find(Expression<Func<TDto, bool>> criteria, Expression<Func<TDto, object>> orderBy,
                               int skip = 0, int? take = null);

        TDto Insert(TDto entity);
        TDto Update(TDto entityToUpdate);
    }
}