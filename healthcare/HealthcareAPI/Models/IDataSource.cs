using System;
using System.Collections.Generic;
using System.Linq;

namespace HealthcareAPI.Models
{
    public interface IDataSource<T>
    {
        IEnumerable<T> ReadData(int split);
    }
}