using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Apoteke.DataObjects.Core
{
    public class ObjectCache<T>
    {
        private TimeSpan _cacheTimeOut;

        private Dictionary<ObjectBase<T>, DateTime> _cache = null;

        public ObjectCache(TimeSpan timeout)
        {
            _cacheTimeOut = timeout;
            _cache = new Dictionary<ObjectBase<T>, DateTime>();
        }

        public  void Sync(ObjectBase<T> obj)
        {
            if ( _cache == null)
                return;

            lock (_cache)
            {

                ObjectBase<T> baseObj = _cache.Keys.SingleOrDefault(baseObject => baseObject.ID == obj.ID);
                if (baseObj != null)
                    _cache.Remove(baseObj);

                _cache.Add(obj, DateTime.Now);
            }

        }

        public ObjectBase<T> Get(int id)
        {
            foreach (ObjectBase<T> o in _cache.Keys)
                if (o.ID == id && DateTime.Now.Subtract(_cache[o]) <= _cacheTimeOut)
                    return o;
                else if (o.ID == id && DateTime.Now.Subtract(_cache[o]) > _cacheTimeOut)
                    lock (_cache)
                        _cache.Remove(o);
                
            return default(ObjectBase<T>);
        }

    }
}
