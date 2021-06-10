using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dn35x.Base
{
    public interface IDefined
    {
        bool IsDefined { get; }
        object Value { get; }
    }

    public struct Definition<T> : IDefined
    {
        private bool isDefined;
        private T data;

        public bool IsDefined => isDefined;

        public object Value => data;

        public Definition(T one)
        {
            isDefined = true;
            data = one;
        }

        public static implicit operator Definition<T>(T one)
        {
            return new Definition<T>(one);
        }

        public static implicit operator T(Definition<T> one)
        {
            return one.data;
        }
    }
}
