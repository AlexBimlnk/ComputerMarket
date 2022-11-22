using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace General.Logic.Queries;
public interface IQueryResult
{
    /// <summary xml:lang = "ru">
    /// Идентификатор команды.
    /// </summary>
    public QueryID Id { get; }

    /// <summary xml:lang = "ru">
    /// Флаг, указывающий является ли результат успешным.
    /// </summary>
    public bool IsSuccess { get; }

    /// <summary xml:lang = "ru">
    /// Сообщение содержащие описание ошибки.
    /// </summary>
    public string? ErrorMessage { get; }
}
