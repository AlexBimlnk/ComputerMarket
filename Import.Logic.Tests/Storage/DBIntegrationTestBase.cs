using System.Data.Common;

using Import.Logic.Storage;

using Microsoft.EntityFrameworkCore;

namespace Import.Logic.Tests.Storage;
public class DBIntegrationTestBase : IDisposable, IAsyncDisposable
{
    private const string CONNECTION = "Host=localhost;Port=5432;Username=postgres;Password=admin;";
    private bool _disposed;

    protected readonly ImportContext _importContext;

    protected DBIntegrationTestBase(string dbName)
    {
        var options = new DbContextOptionsBuilder<ImportContext>()
            .UseNpgsql(CONNECTION + $"Database={dbName}")
            .Options;

        _importContext = new ImportContext(options);

        _importContext.Database.EnsureCreated();
    }

    protected async Task<IList<T>> GetTableRecordsAsync<T>(
        string fromQuery,
        Func<DbDataReader, T> convertFunc)
    {
        using var cmd = _importContext.Database
            .GetDbConnection()
            .CreateCommand();

        cmd.CommandText = $"SELECT * FROM {fromQuery}";
        cmd.CommandType = System.Data.CommandType.Text;

        await _importContext.Database.OpenConnectionAsync()
            .ConfigureAwait(false);

        using var sqlResult = await cmd.ExecuteReaderAsync()
            .ConfigureAwait(false);

        var result = new List<T>();

        if (sqlResult.HasRows)
        {
            while (await sqlResult.ReadAsync().ConfigureAwait(false))
            {
                result.Add(convertFunc(sqlResult));
            }
        }

        await _importContext.Database.CloseConnectionAsync()
            .ConfigureAwait(false);

        return result;
    }

    protected async Task AddAsync(string fromQuery, string valuesQuery)
    {
        await using var cmd = _importContext.Database
            .GetDbConnection()
            .CreateCommand();

        cmd.CommandText = $"INSERT INTO {fromQuery} VALUES {valuesQuery}";
        cmd.CommandType = System.Data.CommandType.Text;

        await _importContext.Database
            .OpenConnectionAsync()
            .ConfigureAwait(false);

        await cmd.ExecuteNonQueryAsync()
            .ConfigureAwait(false);

        await _importContext.Database
            .CloseConnectionAsync()
            .ConfigureAwait(false);
    }

    public void Dispose()
    {
        if (!_disposed)
        {
            Task.Run(async () => await DisposeAsync().ConfigureAwait(false))
                .Wait();
        }
    }
    public async ValueTask DisposeAsync()
    {
        if (!_disposed)
        {
            _importContext.Database.EnsureDeleted();

            await _importContext.DisposeAsync()
                .ConfigureAwait(false);

            _disposed = true;
        }
    }
}
