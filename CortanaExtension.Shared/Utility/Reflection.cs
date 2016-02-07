using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CortanaExtension.Shared.Utility
{
    public class Reflection
    {
        protected Reflection()
        {

        }

        private static T Cast<T>(object o)
        {
            return (T)o;
        }

        private object CastTo(object obj, Type t)
        {
            MethodInfo castMethod = this.GetType().GetMethod("Cast").MakeGenericMethod(t);
            object castedObject = castMethod.Invoke(null, new object[] { obj });
            return castedObject;
        }

        public static object Cast(object obj, Type t)
        {
            Reflection reflect = new Reflection();
            return reflect.CastTo(obj, t);
        }
}
}
