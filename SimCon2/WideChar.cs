using System;
using System.Collections.Generic;
using System.Text;

namespace CString
{
    class WideChar
    {
        public static string StringFromPtr(IntPtr source){
            int len = API.wcslen(source);
            short[] wcs = new short[len];
            API.wcsncpy(wcs, source, (uint)len);
            char[] cary = new char[len];
            for (int i = 0; i < len; i++) cary[i] = (char)wcs[i];
            return new String(cary);
        }

        public static bool StringToPtr(string source, IntPtr output, uint size)
        {
            int max = source.Length >= size ? (int)size - 1 : source.Length;
            char[] chars = source.ToCharArray();
            short[] wchar = new short[chars.Length];
            for (int i = 0; i < chars.Length; i++) wchar[i] = (short)chars[i];
            API.wcsncpy(output, wchar, size - 1);
            IntPtr nc = new IntPtr(output.ToInt32() + (max * 2));
            API.memset(nc, 0, 2);
            return source.Length < size;
        }
    }
}
