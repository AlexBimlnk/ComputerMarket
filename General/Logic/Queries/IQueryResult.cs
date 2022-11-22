using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using General.Logic.Executables;

namespace General.Logic.Queries;
public interface IQueryResult<TEntity> : IExecutableResult
{
    public TEntity? Result { get; }
}
