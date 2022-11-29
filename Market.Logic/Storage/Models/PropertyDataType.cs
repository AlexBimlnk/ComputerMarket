using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Logic.Storage.Models;

public class PropertyDataType
{
    public PropertyDataTypeId PropertyDataTypeId { get; set; }

    public string Name { get; set; } = default!;
}
