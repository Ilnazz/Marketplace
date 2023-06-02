using System.Collections.Generic;
using System;

namespace Marketplace.DataTypes.Records;

public record Sorting<T>(string Name, Func<IEnumerable<T>, IEnumerable<T>> Sorter);