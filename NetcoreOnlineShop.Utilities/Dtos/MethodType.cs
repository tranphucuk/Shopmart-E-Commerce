using System;
using System.Collections.Generic;
using System.Text;

namespace NetcoreOnlineShop.Utilities.Dtos
{
    public class MethodType<Type, Obj> where Obj : class
    {
        public MethodType()
        {

        }

        public MethodType(Type type, Obj obj)
        {
            HttpType = type;
            ObjReturn = obj;
        }

        public MethodType(Obj obj)
        {
            ObjReturn = obj;
        }

        public MethodType(Type type)
        {
            HttpType = type;
        }

        public Type HttpType { get; set; }
        public Obj ObjReturn { get; set; }
    }
}
