using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AchieveReward
{
    public class GameEventInfo
    {
        public Enum type;
        public List<GameEventHandler> mHandler;
        public object[] mArgs;
        public GameEventInfo(Enum type, GameEventHandler handler, params object[] args)
        {
            this.type = type;
            if (mHandler == null) { mHandler = new List<GameEventHandler>(); }
            mHandler.Add(handler);
            mArgs = args;
        }

        public GameEventInfo(Enum type, List<GameEventHandler> handlers, params object[] argss)
        {
            this.type = type;
            mHandler = handlers;
            mArgs = argss;
        }
        public void ExcuteHandler()
        {
            if (mHandler != null)
            {
                for (int i = 0; i < mHandler.Count; i++)
                {
                    mHandler[i](mArgs);
                }
            }
        }
    }
    class GameEvent : IGameEvent
    {
        protected Dictionary<Enum, List<GameEventHandler>> mInternalEvent;
        protected Dictionary<Enum, List<GameEventHandler>> mInternalUseOnceEvent;
        protected List<GameEventInfo> mAsyncHandlerInfos;

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
            mInternalUseOnceEvent = new Dictionary<Enum, List<GameEventHandler>>();
            mAsyncHandlerInfos = new List<GameEventInfo>();
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
            List<GameEventHandler> handlers = null;
            if (mInternalEvent.TryGetValue(type, out handlers))
            {
                mAsyncHandlerInfos.Add(new GameEventInfo(type, handlers, args));
            }
        }

        public void DispatchEvent(Enum type, params object[] args)
        {
            if (mInternalEvent.ContainsKey(type))
            {
                GameEventHandler handler = null;
                for (int i = 0; i < mInternalEvent[type].Count; i++)
                {
                    handler = mInternalEvent[type][i];
                    handler(args);
                }
                if (mInternalUseOnceEvent.ContainsKey(type) && mInternalUseOnceEvent[type].Contains(handler))
                {
                    RemoveEvent(type, handler);
                }
            }
        }

        public void Dispose()
        {
            mInternalUseOnceEvent.Clear();
            mInternalEvent.Clear();
            mAsyncHandlerInfos.Clear();
        }

        public bool HasEvent(Enum type)
        {
            return mInternalEvent.ContainsKey(type) || mInternalUseOnceEvent.ContainsKey(type);
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
            if (mInternalUseOnceEvent.TryGetValue(type, out handlers))
            {
                if (handlers.Contains(handler))
                {
                    handlers.Remove(handler);
                }
            }
        }

        public void UpdateEvents()
        {
            if (mAsyncHandlerInfos.Count > 0)
            {
                for (int i = 0; i < mAsyncHandlerInfos.Count; i++)
                {
                    GameEventInfo info = mAsyncHandlerInfos[i];
                    for (int j = 0; j < info.mHandler.Count; j++)
                    {
                        GameEventHandler handler = info.mHandler[j];
                        handler(info.mArgs);
                        if (mInternalUseOnceEvent.ContainsKey(mAsyncHandlerInfos[i].type)
    && mInternalUseOnceEvent[mAsyncHandlerInfos[i].type].Contains(handler))
                        {
                            RemoveEvent(info.type, handler);
                        }
                    }

                }
                mAsyncHandlerInfos.Clear();
            }
        }
    }
}
