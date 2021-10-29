using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parser
{
    public class Entity<T>
    {
        public T Id { get; set; }
        public Entity(T id)
        {
            Id = id;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Entity<T>))
                return false;
            return Id.Equals(((Entity<T>)obj).Id);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
