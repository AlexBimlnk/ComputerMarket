using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace General.Logic.Queries;
public abstract class QueryParametersBase
{
    /// <summary xml:lang = "ru">
    /// Создает новый экземпляр типа <see cref="CommandParametersBase"/>.
    /// </summary>
    /// <param name="id" xml:lang = "ru">
    /// Идентификатор команды.
    /// </param>
    /// <exception cref="ArgumentNullException" xml:lang = "ru">
    /// Когда <paramref name="id"/> оказался <see langword="null"/>.
    /// </exception>
    protected QueryParametersBase(QueryID id)
    {
        Id = id ?? throw new ArgumentNullException(nameof(id));
    }

    /// <summary xml:lang = "ru">
    /// Идентификатор команды.
    /// </summary>
    public QueryID Id { get; }
}
