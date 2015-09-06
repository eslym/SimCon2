using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace CString
{
    class MultiByte
    {
        public static Encoding Encoding = Encoding.Default;

        public static string StringFromPtr(IntPtr source) {
            return StringFromPtr(source, Encoding);
        }

        public static string StringFromPtr(IntPtr source, Encoding enc) {
            int len = API.strlen(source);
            byte[] bytes = new byte[len];
            API.memmove(bytes, source, (uint)len);
            return enc.GetString(bytes, 0, len);
        }

        public static bool StringToPtr(string source, IntPtr output, uint size) {
            return StringToPtr(source, output, size, Encoding);
        }

        public static bool StringToPtr(string source, IntPtr output, uint size, Encoding enc) {
            byte[] bytes = enc.GetBytes(source);
            uint max = bytes.Length >= size ? size - 1 : (uint)bytes.Length;
            API.memmove(output, bytes, max);
            IntPtr nc = new IntPtr(output.ToInt32() + max);
            API.memset(nc, 0, 1);
            return bytes.Length < size;
        }
    }
}
