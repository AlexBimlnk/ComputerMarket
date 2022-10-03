using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Import.Logic.Abstractions;
public interface IKeyableCache<TEntity, TKey>: ICache<TEntity> where TEntity : IKeyable<TKey>
{
    public TEntity? GetByKey(TKey key);

    public bool Contains(TKey key);
}
