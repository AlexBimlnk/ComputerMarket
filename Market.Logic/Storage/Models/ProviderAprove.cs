using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Logic.Storage.Models;

public class ProviderAprove
{
    public long ProviderId { get; set; }

    public long OrderId { get; set; }

    public virtual Order Order { get; set; } = default!;

    public virtual Provider Provider { get; set; } = default!;
}
