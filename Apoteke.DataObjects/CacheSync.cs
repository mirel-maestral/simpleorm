using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using Apoteke.DataObjects.Core;
using Apoteke.DataObjects.BLL;


namespace Apoteke.DataObjects
{
    public class CacheSync
    {
        private static TimeSpan _cacheTimeOut = new TimeSpan(0, 2, 0);

        private static List<object> _cacheList = new List<object>();
        
        static CacheSync()
        {
            if (!String.IsNullOrEmpty(ConfigurationManager.AppSettings["CacheTimeOut"]))
                TimeSpan.TryParse(ConfigurationManager.AppSettings["CacheTimeOut"], out _cacheTimeOut);
            
            Initialize();
        }

        private static void Initialize()
        {
            //_cacheList.Add(new ObjectCache<Objekat>(_cacheTimeOut));
            
            List<Proizvodjac> proizvodjaci = ApotekeDB.Instance.GetProizvodjaci();
            foreach (Proizvodjac pro in proizvodjaci)
                if (pro != null)
                    Sync<Proizvodjac>(pro, new TimeSpan(24, 0, 0));

            List<Mjera> mjere = ApotekeDB.Instance.GetMjere();
            foreach (Mjera mje in mjere)
                if (mje != null)
                    Sync<Mjera>(mje,new TimeSpan(24,0,0));

            List<Smjena> smjene = ApotekeDB.Instance.GetSmjene();
            foreach (Smjena smje in smjene)
                if (smje != null)
                    Sync<Smjena>(smje, new TimeSpan(24, 0, 0));

            List<Korisnik> korisnici = ApotekeDB.Instance.GetKorisnici();
            foreach (Korisnik kor in korisnici)
                if (kor != null)
                    Sync<Korisnik>(kor, new TimeSpan(24, 0, 0));

            List<PDVStopa> pdvStope = ApotekeDB.Instance.GetPDVStope();
            foreach (PDVStopa stopa in pdvStope )
                if (stopa != null)
                    Sync<PDVStopa>(stopa, new TimeSpan(24, 0, 0));


        }

        public static void SyncRange<T>(List<ObjectBase<T>> objList)
        {
            SyncRange<T>(objList, _cacheTimeOut);
        }


        public static void SyncRange<T>(List<ObjectBase<T>> objList, TimeSpan cacheTimeOut)
        {
            foreach (ObjectBase<T> obj in objList)
                if (obj != null)
                    Sync<T>(obj, cacheTimeOut);
        }

        public static void Sync<T>(ObjectBase<T> obj)
        {
            Sync<T>(obj, _cacheTimeOut);
        }

        public static void Empty<T>()
        {
            try
            {
                ObjectCache<T> _cache = _cacheList.SingleOrDefault(ob => ob.GetType() == typeof(ObjectCache<T>)) as ObjectCache<T>;
                if (_cache == null)
                    return;

                lock (_cacheList)
                {
                    _cacheList.Remove(_cache);
                }
            }
            catch (System.Exception ex)
            {

            }

        }

        public static void Sync<T>(ObjectBase<T> obj, TimeSpan cacheTimeOut)
        {
            if (obj == null)
                return;

            try
            {
                ObjectCache<T> _cache = _cacheList.SingleOrDefault(ob => ob.GetType() == typeof(ObjectCache<T>)) as ObjectCache<T>;
                if (_cache == null)
                {
                    lock (_cacheList)
                    {
                        _cacheList.Add(new ObjectCache<T>(cacheTimeOut));

                    }

                    return;
                }

                lock(_cache)
                _cache.Sync(obj);

            }
            catch (System.Exception ex)
            {

            }

        }

        public static ObjectBase<T> Get<T>(int id)
        {
            try
            {

                ObjectCache<T> _cache = _cacheList.SingleOrDefault(obj => obj.GetType() == typeof(ObjectCache<T>)) as ObjectCache<T>;
                if (_cache == null)
                {
                    lock (_cacheList)
                    {
                        _cacheList.Add(new ObjectCache<T>(_cacheTimeOut));

                    }

                    return default(ObjectBase<T>);
                }

                return _cache.Get(id);

            }
            catch (System.Exception ex)
            {

            }


            return default(ObjectBase<T>);
        }

        
    }
}
