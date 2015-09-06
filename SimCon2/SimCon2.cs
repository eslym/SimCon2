using RGiesecke.DllExport;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using CString;

namespace SimCon2
{
    public class SimCon2
    {
        public static string stored = "";

        [DllExport("sc2_encoding", CallingConvention.Cdecl)]
        public static bool SetEncoding(int cp) {
            try {
                MultiByte.Encoding = Encoding.GetEncoding(cp);
                Console.InputEncoding = MultiByte.Encoding;
                Console.OutputEncoding = MultiByte.Encoding;
            } catch (Exception e) {
                return false;
            }
            return true;
        }

        [DllExport("sc2_getstr", CallingConvention.Cdecl)]
        public static bool GetStringA(IntPtr output, uint size) {
            string input = Console.ReadLine();
            return MultiByte.StringToPtr(input, output, size);
        }

        [DllExport("sc2_getpwd", CallingConvention.Cdecl)]
        public static void GetPassword(IntPtr output, uint size, short mask) {
            if (mask == 0) mask = (short)'*';
            string pwd = "";
            int start = Console.CursorLeft;
            int index = 0;
            ConsoleKeyInfo key;
            while (true) {
                key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.Enter) break;
                if (index > 0 && key.Key == ConsoleKey.LeftArrow) {
                    index -= 1;
                    Console.CursorLeft = start + index;
                    continue;
                }
                if (index < pwd.Length && key.Key == ConsoleKey.RightArrow){
                    index += 1;
                    Console.CursorLeft = start + index;
                    continue;
                }
                if (index > 0 && ((key.Key == ConsoleKey.Backspace) ||
                    (index == pwd.Length && key.Key == ConsoleKey.Delete))){
                    index -= 1;
                    pwd = pwd.Substring(0, index) + pwd.Substring(index + 1);
                    Console.CursorLeft = start + index;
                    Console.Write(new String((char)mask, pwd.Substring(index).Length) + " ");
                    Console.CursorLeft = start + index;
                    continue;
                }
                if (pwd.Length > 0 && key.Key == ConsoleKey.Delete) {
                    pwd = pwd.Substring(0, index) + pwd.Substring(index + 1);
                    Console.CursorLeft = start + index;
                    Console.Write(new String((char)mask, pwd.Substring(index).Length) + " ");
                    Console.CursorLeft = start + index;
                    continue;
                }
                if (pwd.Length < size - 1 && key.KeyChar >= '!' && key.KeyChar <= '~'){
                    pwd = pwd.Substring(0, index) + key.KeyChar + pwd.Substring(index);
                    Console.Write(new String((char)mask, pwd.Substring(index).Length));
                    index += 1;
                    Console.CursorLeft = start + index;
                    continue;
                }
                Console.Beep();
            }
            MultiByte.StringToPtr(pwd, output, size);
        }

        [DllExport("sc2_getwcs", CallingConvention.Cdecl)]
        public static bool GetStringW(IntPtr output, uint size) {
            string input = Console.ReadLine();
            return WideChar.StringToPtr(input, output, size);
        }

        [DllExport("sc2_store", CallingConvention.Cdecl)]
        public static uint StoreStrA()
        {
            stored = Console.ReadLine();
            return (uint)MultiByte.Encoding.GetBytes(stored.ToCharArray()).Length + 1;
        }

        [DllExport("sc2_store_w", CallingConvention.Cdecl)]
        public static uint StoreStrW()
        {
            stored = Console.ReadLine();
            return (uint)stored.Length + 1;
        }

        [DllExport("sc2_getstore", CallingConvention.Cdecl)]
        public static bool GetStoreA(IntPtr output, uint size)
        {
            return MultiByte.StringToPtr(stored, output, size);
        }

        [DllExport("sc2_getstore_w", CallingConvention.Cdecl)]
        public static bool GetStoreW(IntPtr output, uint size)
        {
            return WideChar.StringToPtr(stored, output, size);
        }

        [DllExport("sc2_getint", CallingConvention.Cdecl)]
        public static bool GetInt32(out int output) {
            return Int32.TryParse(Console.ReadLine(), out output);
        }

        [DllExport("sc2_getfloat", CallingConvention.Cdecl)]
        public static bool GetFloat(out float output) {
            return float.TryParse(Console.ReadLine(), out output);
        }

        [DllExport("sc2_confirm", CallingConvention.Cdecl)]
        public static bool Confirm(bool prompt) {
            if (prompt) Console.Write("(Y/N)?");
            ConsoleKey key = Console.ReadKey(true).Key;
            while (key != ConsoleKey.Y && key != ConsoleKey.N) {
                Console.Beep();
                key = Console.ReadKey(true).Key;
            }
            if (prompt) {
                if (key == ConsoleKey.Y) Console.Write("Yes");
                else Console.Write("No");
            }
            Console.WriteLine();
            return key == ConsoleKey.Y;
        }

        [DllExport("sc2_title", CallingConvention.Cdecl)]
        public static void Title(IntPtr title)
        {
            Console.Title = MultiByte.StringFromPtr(title);
        }

        [DllExport("sc2_clrscr", CallingConvention.Cdecl)]
        public static void Clear() {
            Console.Clear();
        }

        [DllExport("sc2_pause", CallingConvention.Cdecl)]
        public static void Pause() {
            Console.ReadKey(true);
        }

        [DllExport("sc2_beep", CallingConvention.Cdecl)]
        public static void Beep() {
            Console.Beep();
        }
    }
}
