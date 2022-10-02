using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Import.Logic.Abstractions;
using Import.Logic.Models;

using Microsoft.Extensions.Logging;

using Nito.AsyncEx;

namespace Import.Logic;
internal class HistoryRecorder
{
    private readonly IRepository<History> _repository;
    private readonly ILogger<HistoryRecorder> _logger;

    public HistoryRecorder(IRepository<History> repository, ILogger<HistoryRecorder> logger)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async void RecordHistoryAsync(IReadOnlyCollection<History> histories)
    {
        ArgumentNullException.ThrowIfNull(histories);

        var _mutex = new AsyncLock();
        using (await _mutex.LockAsync())
        {
            foreach (var history in histories)
            {
                await _repository.AddAsync(history);
            }

            await _repository.SaveAsync();
        }

        


    }
}
