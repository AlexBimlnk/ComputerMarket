using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Import.Logic.Abstractions;
public interface IKeyable<TKey>
{
    public TKey Key { get; }
}
