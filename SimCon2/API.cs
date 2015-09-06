using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

class API
{
    [DllImport("msvcrt.dll")]
    public static extern int strlen(IntPtr str);

    [DllImport("msvcrt.dll")]
    public static extern int wcslen(IntPtr str);

    [DllImport("msvcrt.dll")]
    public static extern IntPtr wcsncpy(short[] dest, IntPtr src, uint size);

    [DllImport("msvcrt.dll")]
    public static extern IntPtr wcsncpy(IntPtr ddest, short[] src, uint size);

    [DllImport("msvcrt.dll")]
    public static extern IntPtr memmove(byte[] dest, IntPtr src, uint size);

    [DllImport("msvcrt.dll")]
    public static extern IntPtr memmove(IntPtr dest, byte[] src, uint size);

    [DllImport("msvcrt.dll")]
    public static extern IntPtr memset(IntPtr ptr, int value, uint size);
}