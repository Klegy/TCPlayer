﻿/*
    TC Plyer
    Total Commander Audio Player plugin & standalone player written in C#, based on bass.dll components
    Copyright (C) 2016 Webmaster442 aka. Ruzsinszki Gábor

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace TCPlayer.Code
{
    internal class Native
    {
        private static int IntPtrSize(IntPtr p)
        {
            int num = 0;
            while (Marshal.ReadByte(p, num) != 0) num++;
            return num;
        }

        [DllImport("dwmapi.dll", EntryPoint = "#127")]
        internal static extern void DwmGetColorizationParameters(ref DWMCOLORIZATIONPARAMS pars);

        [DllImport("kernel32.dll", EntryPoint = "FreeLibrary")]
        public static extern bool FreeLibrary(IntPtr hLib);

        public static string[] IntPtrToArray(IntPtr pointer)
        {
            unsafe
            {
                if (pointer != IntPtr.Zero)
                {
                    List<string> list = new List<string>();
                    string item = string.Empty;
                    while (true)
                    {
                        int num = IntPtrSize(pointer);
                        if (num <= 0) break;
                        byte[] array = new byte[num];
                        Marshal.Copy(pointer, array, 0, num);
                        pointer = new IntPtr((void*)((byte*)((byte*)pointer.ToPointer() + num) + 1));
                        item = Encoding.UTF8.GetString(array, 0, num);
                        list.Add(item);
                    }
                    if (list.Count > 0) return list.ToArray();
                }
                return null;
            }
        }

        [DllImport("kernel32.dll", EntryPoint = "LoadLibrary")]
        public static extern IntPtr LoadLibrary(string DllToLoad);

        // Registers a hot key with Windows.
        [DllImport("user32.dll")]
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

        // Unregisters the hot key with Windows.
        [DllImport("user32.dll")]
        public static extern bool UnregisterHotKey(IntPtr hWnd, int id);
    }


    public struct DWMCOLORIZATIONPARAMS
    {
        public uint ColorizationColor,
            ColorizationAfterglow,
            ColorizationColorBalance,
            ColorizationAfterglowBalance,
            ColorizationBlurBalance,
            ColorizationGlassReflectionIntensity,
            ColorizationOpaqueBlend;
    }
}
