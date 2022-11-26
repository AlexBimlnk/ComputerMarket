using General.Transport;

using Market.Logic.Commands.Import;
using Market.Logic.Queries.Import;
using Market.Logic.Transport.Models;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Market.Logic.Transport.Serializers;

using TQueryBase = QueryBase;
using TGetLinksQuery = Models.Queries.Import.GetLinksQuery;

public sealed class ImportQuerySerializer : ISerializer<ImportQuery, string>
{
    public string Serialize(ImportQuery source)
    {
        ArgumentNullException.ThrowIfNull(source, nameof(source));

        TQueryBase transportProducts = source switch
        {
            GetLinksQuery setLinkCommand => TGetLinksQuery.ToModel(setLinkCommand),
            var unknownCommandType =>
                throw new InvalidOperationException($"The source contains unknown command '{unknownCommandType?.GetType().Name}'. ")
        };

        return JsonConvert.SerializeObject(transportProducts, new StringEnumConverter());
    }
}
