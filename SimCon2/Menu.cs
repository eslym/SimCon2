using CString;
using RGiesecke.DllExport;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace SimCon2
{
    class Menu
    {
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int Callback(int param);

        public static Callback callback = null;
        public static Callback keyListener = null;
        public static List<Item> items = new List<Item>();

        [DllExport("sc2m_set_callback", CallingConvention.Cdecl)]
        public static void SetCallback(IntPtr callback)
        {
            if (callback == IntPtr.Zero) Menu.callback = null;
            else
                Menu.callback = (Callback)Marshal.GetDelegateForFunctionPointer(
               callback, typeof(Callback));
        }

        [DllExport("sc2m_set_keyListener", CallingConvention.Cdecl)]
        public static void SetKeyListener(IntPtr callback)
        {
            if (callback == IntPtr.Zero) Menu.keyListener = null;
            else
                Menu.keyListener = (Callback)Marshal.GetDelegateForFunctionPointer(
               callback, typeof(Callback));
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

        public static int Show(string title)
        {
            int index = 0;
            Draw(title, index);
            while (true)
            {
                ConsoleKeyInfo ki = Console.ReadKey(true);
                if (ki.Key == ConsoleKey.Enter || ki.Key == ConsoleKey.Spacebar) break;
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
                else if (Menu.keyListener != null &&
                         Menu.keyListener(SimCon2.Key2Int(ki)) != 0) break;
            }
            if (items[index].callback != null) return items[index].callback(index);
            else if (Menu.callback != null) return Menu.callback(index);
            return index;
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
