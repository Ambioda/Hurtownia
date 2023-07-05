using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;


namespace Hurtownia.Infrastruktura
{
    public class SessionMenager : ISessionManager
    {
        private HttpSessionStateBase session;

        public SessionMenager()
        {
           session = HttpContext.Current.Session;
        }
        public void Abandon()
        {
            throw new NotImplementedException();
        }

        public T GeT<T>(string key)
        {
            throw new NotImplementedException();
        }

        public void Set<T>(string name, T value)
        {
            throw new NotImplementedException();
        }

        public T TryGet<T>(string key)
        {
            throw new NotImplementedException();
        }
    }
}