using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets._Scripts.FakeHexes
{
    public interface ICopy<T>
    {
        T ShallowCopy();
    }
}
