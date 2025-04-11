using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Overrank
{
    public class LinkedBehavior<T, T2> where T : MonoBehaviour where T2 : LinkedBehavior<T, T2>, new()
    {
        protected T parent;
        private static Dictionary<T, T2> Link = new Dictionary<T, T2>();
        public static T2 AttachLink(T mb)
        {
            if (Link.TryGetValue(mb, out T2 np))
                return np;
            T2 linked = new T2();
            linked.parent = mb;
            linked.Init();
            Link.Add(mb, linked);

            foreach (var key in Link.Keys.ToArray())
            {
                if (key == null)
                    Link.Remove(key);
            }

            return linked;
        }

        public static T2 GetLink(T mb)
        {
            if (Link.TryGetValue(mb, out T2 np))
                return np;
            return null;
        }
        
        public virtual void Init()
        {

        }

        public virtual void OnDisable()
        {

        }

    }
}
