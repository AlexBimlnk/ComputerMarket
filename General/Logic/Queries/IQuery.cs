using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using General.Logic.Commands;

namespace General.Logic.Queries;

public interface IQuery
{
    /// <summary xml:lang = "ru">
    /// Асинхронно выполняет команду.
    /// </summary>
    /// <returns xml:lang = "ru">
    /// <see cref="Task{TResult}>"/>.
    /// </returns>
    public Task<IQueryResult> ExecuteAsync();
}
