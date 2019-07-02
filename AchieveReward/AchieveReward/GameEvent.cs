using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AchieveReward
{
    class GameEvent : IGameEvent
    {
        protected Dictionary<Enum, List<GameEventHandler>> mInternalEvent;
        protected Dictionary<Enum, List<GameEventHandler>> mInternalUseOnceEvent;
        public bool IsDispose
        {
            get
            {
                throw new NotImplementedException();
            }
        }
        public GameEvent()
        {
            mInternalEvent = new Dictionary<Enum, List<GameEventHandler>>();
        }

        public void AddEvent(Enum type, GameEventHandler handler, bool isUseOnce = false, bool isFirst = false)
        {
            if (mInternalEvent.ContainsKey(type))
            {
                if (isFirst) { mInternalEvent[type].Insert(0, handler); }
                else { mInternalEvent[type].Add(handler); }
                if (isUseOnce)
                {
                    if (isFirst) { mInternalUseOnceEvent[type].Insert(0, handler); }
                    else { mInternalUseOnceEvent[type].Add(handler); }
                }
            }
            else
            {
                mInternalEvent.Add(type, new List<GameEventHandler>() { handler });
                if (isUseOnce) { mInternalUseOnceEvent.Add(type, new List<GameEventHandler>() { handler }); }
            }
        }

        public void DispatchAsyncEvent(Enum type, params object[] args)
        {
            throw new NotImplementedException();
        }

        public void DispatchEvent(Enum type, params object[] args)
        {
            if (mInternalEvent.ContainsKey(type))
            {
                for (int i = 0; i < mInternalEvent[type].Count; i++)
                {
                    GameEventHandler handler = mInternalEvent[type][i];
                    if(mInternalUseOnceEvent.ContainsKey(type) && mInternalUseOnceEvent[type].Contains(handler)) { }
                    handler(args);
                }
            }
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public bool HasEvent(Enum type)
        {
            throw new NotImplementedException();
        }

        public void RemoveEvent()
        {
            mInternalEvent.Clear();
            mInternalUseOnceEvent.Clear();
        }

        public void RemoveEvent(Enum type)
        {
            if (mInternalEvent.ContainsKey(type))
            {
                mInternalEvent.Remove(type);
            }
            if (mInternalUseOnceEvent.ContainsKey(type))
            {
                mInternalUseOnceEvent.Remove(type);
            }
        }

        public void RemoveEvent(Enum type, GameEventHandler handler)
        {
            List<GameEventHandler> handlers = null;
            if (mInternalEvent.TryGetValue(type, out handlers))
            {
                if (handlers.Contains(handler))
                {
                    handlers.Remove(handler);
                }
            }
            if(mInternalUseOnceEvent.TryGetValue(type,out handlers))
            {
                if (handlers.Contains(handler))
                {
                    handlers.Remove(handler);
                }
            }
        }
    }
}
