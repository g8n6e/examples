using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceAutoTest.Services
{
    public class GenericEventArgs<T> : EventArgs
    {
        public T EventData { get; private set; }

        public GenericEventArgs(T EventData)
        {
            this.EventData = EventData;
        }
    }
}
