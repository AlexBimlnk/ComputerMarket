using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Logic.Models.Abstractions;
internal interface IFileterProperty
{
    public ItemProperty Property { get; }
    
    public IEnumerable<IFilterValue> Values { get; }
}
