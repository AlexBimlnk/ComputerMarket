using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Import.Logic.Models;

namespace Import.Logic.Abstractions;
public interface IMapper<TEntity> where TEntity : IMappableEntity<InternalID, ExternalID>
{

    public TEntity MapEntity(TEntity entity);

    public IEnumerable<TEntity> MapEntityCollection(IEnumerable<TEntity> entities);
}
