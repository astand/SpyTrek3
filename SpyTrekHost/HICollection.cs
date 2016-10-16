using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace SpyTrekHost
{
    public static class HICollection
    {
        static List<HandleInstance> list_;
        static public List<HandleInstance> List => list_;

        static Action WhenListRefresh;
        static HICollection()
        {
            list_ = new List<HandleInstance>();
        }

        public static void Add(HandleInstance instance)
        {
            lock (list_)
            {
                instance.SelfDeleter += Deleter;
                list_.Add(instance);
                RefreshList();
            }
        }

        public static void Remove(HandleInstance instance)
        {
            lock (list_)
            {
                instance.SelfDeleter -= Deleter;
                list_.Remove(instance);
                RefreshList();
            }
        }


        public static void Deleter(Object sender, EventArgs e)
        {
            lock (list_)
            {
                var item = sender as HandleInstance;
                Int32 index = list_.IndexOf(item);
                if (index >= 0)
                    list_.RemoveAt(index);

                item.Dispose();
                Debug.WriteLine($"Item [{index}] was been deleted.");
            }
        }

        public static HandleInstance GetByIndex(int index)
        {
            if (index < list_.Count)
                return list_[index];
            return null;
        }


        public static void AddListUpdater(Action updater)
        {
            WhenListRefresh += updater;
        }


        internal static void RefreshList()
        {
            WhenListRefresh?.Invoke();
        }
    }
}
