using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RefactorThis.DataLayer
{
    public interface IEntity<TKey>
    {
        TKey Id { get; set; }
    }
}
