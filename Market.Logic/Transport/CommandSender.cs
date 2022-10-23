using General.Transport;

using Market.Logic.Commands.Import;
using Market.Logic.Transport.Configurations;

namespace Market.Logic.Transport;
internal class CommandSender : ISender<ImportConfigurationSender, ImportCommand>
{
    public Task SendAsync(ImportCommand entity, CancellationToken token = default) 
        => throw new NotImplementedException();
}
