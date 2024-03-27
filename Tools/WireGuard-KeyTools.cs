using System;
using System.Runtime.InteropServices;

namespace ConfigGen
{
    internal static class WGTool
    {
        [DllImport("wg_Curve.dll", EntryPoint = "WGGenKeypair", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool WGGenKeypair(Byte[] publicKey, Byte[] privateKey);

        [DllImport("wg_Curve.dll", EntryPoint = "WGCalcPublicKey", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool WGCalcPublicKey(Byte[] publicKey, Byte[] privateKey);
    }
}