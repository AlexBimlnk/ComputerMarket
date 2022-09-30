using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Import.Logic.Abstractions;
public interface IMappableEntity<TInternalId, TExternalId>
{
    /// <summary xml:lang = "ru">
    /// Указывает на наличие связи с внутренним продуктом.
    /// </summary>
    public bool IsMapped { get; }

    public TInternalId InternalID { get; }

    public TExternalId ExternalID { get; }

    public void MapTo(TInternalId mapId);
}
