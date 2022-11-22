using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using General.Logic.Commands;
using General.Logic.Executables;

namespace General.Logic.Queries;

public interface IQuery<TEntity> : IExecutable<IQueryResult<TEntity>>
{
    
}
