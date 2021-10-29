using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parser
{
    public class ValueObject<T>
    {
        public override bool Equals(object obj)
        {
            if (!(obj is T))
                return false;
            obj = (T)obj;
            return Equals(obj);
        }

        public bool Equals(T v)
        {
            if (v == null)
                return false;
            var result = true;
            var t = typeof(T);
            foreach (var p in t.GetProperties())
            {
                var objVal = t.GetProperty(p.Name).GetValue(v);
                var thisVal = t.GetProperty(p.Name).GetValue(this);
                if (objVal == null || thisVal == null)
                {
                    result &= objVal == null && thisVal == null;
                }
                else
                    result &= objVal.Equals(thisVal);
            }
            return result;
        }

        public override int GetHashCode()
        {
            return typeof(T).GetProperties().Select(t => t.GetHashCode()).Aggregate((res, next) => (res * 3) ^ next);
        }
    }
}
