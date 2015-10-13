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
        public static Queue<string> StoredStrings = new Queue<string>();

        [DllExport("sc2_encoding", CallingConvention.Cdecl)]
        public static bool SetEncoding(int cp) {
            try {
                MultiByte.Encoding = Encoding.GetEncoding(cp);
                Console.InputEncoding = MultiByte.Encoding;
                Console.OutputEncoding = MultiByte.Encoding;
            } catch (Exception) {
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
            while (true) {
                var key = Console.ReadKey(true);
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
        public static void StoreStr()
        {
            StoredStrings.Enqueue(Console.ReadLine());
        }

        [DllExport("sc2_getStoredCount", CallingConvention.Cdecl)]
        public static int GetStoreLength()
        {
            return StoredStrings.Count;
        }

        [DllExport("sc2_getNextLen", CallingConvention.Cdecl)]
        public static uint GetNextLengthA()
        {
            return (uint)MultiByte.Encoding.GetBytes(StoredStrings.Peek().ToCharArray()).Length + 1;
        }

        [DllExport("sc2_getNextLen_w", CallingConvention.Cdecl)]
        public static uint GetNextLengthW()
        {
            return (uint)StoredStrings.Peek().Length + 1;
        }

        [DllExport("sc2_getStore", CallingConvention.Cdecl)]
        public static bool GetStoreA(IntPtr output, uint size)
        {
            return MultiByte.StringToPtr(StoredStrings.Dequeue(), output, size);
        }

        [DllExport("sc2_getStore_w", CallingConvention.Cdecl)]
        public static bool GetStoreW(IntPtr output, uint size)
        {
            return WideChar.StringToPtr(StoredStrings.Dequeue(), output, size);
        }

        [DllExport("sc2_getPstr", CallingConvention.Cdecl)]
        public static IntPtr GetPtrStr()
        {
            string input = Console.ReadLine();
            byte[] bytes = MultiByte.Encoding.GetBytes(input);
            Array.Resize<byte>(ref bytes, bytes.Length + 1);
            bytes[bytes.Length - 1] = 0;
            IntPtr ptr = API.malloc((uint)bytes.Length);
            if (ptr != IntPtr.Zero)
                API.memmove(ptr, bytes, (uint)bytes.Length);
            return ptr;
        }

        [DllExport("sc2_getPwcs", CallingConvention.Cdecl)]
        public static IntPtr GetPtrWcs()
        {
            string input = Console.ReadLine();
            IntPtr ptr = API.malloc(((uint)input.Length + 1) * 2);
            if (ptr != IntPtr.Zero)
                WideChar.StringToPtr(input, ptr, (uint)input.Length + 1);
            return ptr;
        }

        [DllExport("sc2_getint", CallingConvention.Cdecl)]
        public static bool GetInt32(out int output) {
            return Int32.TryParse(Console.ReadLine(), out output);
        }

        [DllExport("sc2_getfloat", CallingConvention.Cdecl)]
        public static bool GetFloat(out float output) {
            return float.TryParse(Console.ReadLine(), out output);
        }

        public static int Key2Int(ConsoleKeyInfo ki)
        {
            int modifiers = (int)ki.Modifiers;
            byte[] mbytes = BitConverter.GetBytes(modifiers);
            int key = (int)ki.Key;
            byte[] kbytes = BitConverter.GetBytes(key);
            mbytes[2] = kbytes[0];
            mbytes[3] = kbytes[1];
            return BitConverter.ToInt32(mbytes, 0);
        }

        [DllExport("sc2_getkey", CallingConvention.Cdecl)]
        public static int GetKey(bool hide)
        {
            return Key2Int(Console.ReadKey(hide));
        }

        [DllExport("sc2_keycmp", CallingConvention.Cdecl)]
        public static bool KeyCmp(int k1, int k2)
        {
            return k1 == k2;
        }

        [DllExport("sc2_confirm", CallingConvention.Cdecl)]
        public static bool Confirm(bool prompt)
        {
            if (prompt) Console.Write("(Y/N)?");
            var key = Console.ReadKey(true).Key;
            while (key != ConsoleKey.Y && key != ConsoleKey.N) {
                Console.Beep();
                key = Console.ReadKey(true).Key;
            }
            if (prompt) {
                Console.Write(key == ConsoleKey.Y ? "Yes" : "No");
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
