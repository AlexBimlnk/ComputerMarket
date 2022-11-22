using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using General.Logic.Commands;

namespace General.Logic.Queries;
public interface IAPIQueryHandler<TResult> : IAPIExecutableHandler<IQueryResult<TResult>>
{
}
