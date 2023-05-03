using CString;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace SimCon2
{
    class Menu
    {
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int Callback(int param);

        public static Callback callback = null;
        public static List<Callback> keyHandlers = new List<Callback>();
        public static List<Item> items = new List<Item>();

        [DllExport("sc2m_set_callback", CallingConvention.Cdecl)]
        public static void SetCallback(IntPtr callback)
        {
            if (callback == IntPtr.Zero) Menu.callback = null;
            else
                Menu.callback = (Callback)Marshal.GetDelegateForFunctionPointer(
               callback, typeof(Callback));
        }

        [DllExport("sc2m_add_keyHandler", CallingConvention.Cdecl)]
        public static int AddKeyHandler(int key, IntPtr handlerPtr)
        {
            if (handlerPtr == IntPtr.Zero)
                return -1;
            Callback handler = (Callback)Marshal.GetDelegateForFunctionPointer(
                handlerPtr, typeof(Callback));
            if (!keyHandlers.Exists(x => x.Equals(handler)))
                keyHandlers.Add(handler);
            return keyHandlers.IndexOf(handler);
        }

        [DllExport("sc2m_remove_keyHandler", CallingConvention.Cdecl)]
        public static int RemoveKeyHandler(int index)
        {
            try
            {
                keyHandlers.RemoveAt(index);
            }
            catch (ArgumentOutOfRangeException)
            {
                return 0;
            }
            return 1;
        }

        [DllExport("sc2m_show", CallingConvention.Cdecl)]
        public static int ShowA(IntPtr ptitle)
        {
            return Show(MultiByte.StringFromPtr(ptitle));
        }

        [DllExport("sc2m_show_w", CallingConvention.Cdecl)]
        public static int ShowW(IntPtr ptitle)
        {
            return Show(WideChar.StringFromPtr(ptitle));
        }

        [DllExport("sc2m_show_idx", CallingConvention.Cdecl)]
        public static int ShowA_Idx(IntPtr ptitle, int idx)
        {
            return Show(MultiByte.StringFromPtr(ptitle), idx);
        }

        [DllExport("sc2m_show_w_idx", CallingConvention.Cdecl)]
        public static int ShowW_Idx(IntPtr ptitle, int idx)
        {
            return Show(WideChar.StringFromPtr(ptitle), idx);
        }

        public static int Show(string title)
        {
            return Show(title, 0);
        }

        public static int Show(string title, int indexParam)
        {
            int index = indexParam;
            Draw(title, index);
            while (true)
            {
                ConsoleKeyInfo ki = Console.ReadKey(true);
                int keyInt = SimCon2.Key2Int(ki);
                if (ki.Key == ConsoleKey.Enter || ki.Key == ConsoleKey.Spacebar)
                {
                    if (items[index].callback != null)
                        return items[index].callback(index);
                    else if (Menu.callback != null)
                        return Menu.callback(index);
                    else return index;
                }
                if (ki.Key == ConsoleKey.DownArrow)
                {
                    index += 1;
                    index %= items.Count;
                    Draw(title, index);
                }
                else if (ki.Key == ConsoleKey.UpArrow)
                {
                    index -= 1;
                    if (index < 0) index = items.Count - 1;
                    Draw(title, index);
                }
                foreach (Callback i in keyHandlers)
                {
                    int tmp = i(keyInt);
                    if (tmp != 0)
                        return tmp;
                }
            }
        }

        public static void Draw(string title, int index)
        {
            Console.Clear();
            Console.WriteLine(title);
            Console.WriteLine();
            ConsoleColor fc = Console.ForegroundColor;
            ConsoleColor bc = Console.BackgroundColor;
            for (int i = 0; i < items.Count; i++) {
                Console.Write("\t");
                if (index == i) {
                    Console.ForegroundColor = bc;
                    Console.BackgroundColor = fc;
                }
                Console.Write(" {0,-30} ", items[i].title);
                Console.ForegroundColor = fc;
                Console.BackgroundColor = bc;
                Console.WriteLine(" ");
            }
            Console.WriteLine();
            if (items[index].help != null)
                Console.WriteLine("Tips: {0}", items[index].help);
        }

        [DllExport("sc2m_add", CallingConvention.Cdecl)]
        public static void AddItemA(IntPtr title, IntPtr help, IntPtr callback)
        {
            Item i = new Item();
            i.title = MultiByte.StringFromPtr(title);
            i.help = help == IntPtr.Zero ? null : MultiByte.StringFromPtr(help);
            if (callback == IntPtr.Zero) i.callback = null;
            else i.callback = (Callback)Marshal.GetDelegateForFunctionPointer(
                callback, typeof(Callback));
            items.Add(i);
        }

        [DllExport("sc2m_add_w", CallingConvention.Cdecl)]
        public static void AddItemW(IntPtr title, IntPtr help, IntPtr callback)
        {
            Item i = new Item();
            i.title = WideChar.StringFromPtr(title);
            i.help = help == IntPtr.Zero ? null : WideChar.StringFromPtr(help);
            if (callback == IntPtr.Zero) i.callback = null;
            else i.callback = (Callback)Marshal.GetDelegateForFunctionPointer(
                callback, typeof(Callback));
            items.Add(i);
        }

        [DllExport("sc2m_insert", CallingConvention.Cdecl)]
        public static void InsertItemA(int index, IntPtr title, IntPtr help, IntPtr callback)
        {
            Item i = new Item();
            i.title = MultiByte.StringFromPtr(title);
            if (help == IntPtr.Zero) i.help = null;
            else i.help = MultiByte.StringFromPtr(help);
            if (callback == IntPtr.Zero) i.callback = null;
            else i.callback = (Callback)Marshal.GetDelegateForFunctionPointer(
                callback, typeof(Callback));
            items.Insert(index, i);
        }

        [DllExport("sc2m_insert_w", CallingConvention.Cdecl)]
        public static void InsertItemW(int index, IntPtr title, IntPtr help, IntPtr callback)
        {
            Item i = new Item();
            i.title = WideChar.StringFromPtr(title);
            if (help == IntPtr.Zero) i.help = null;
            else i.help = WideChar.StringFromPtr(help);
            if (callback == IntPtr.Zero) i.callback = null;
            else i.callback = (Callback)Marshal.GetDelegateForFunctionPointer(
                callback, typeof(Callback));
            items.Insert(index, i);
        }

        [DllExport("sc2m_remove", CallingConvention.Cdecl)]
        public static void RemoveItem(int index)
        {
            items.RemoveAt(index);
        }

        [DllExport("sc2m_reset", CallingConvention.Cdecl)]
        public static void Reset()
        {
            Menu.callback = null;
            items.Clear();
        }

        public struct Item
        {
            public string title;
            public string help;
            public Callback callback;
        }
    }
}
