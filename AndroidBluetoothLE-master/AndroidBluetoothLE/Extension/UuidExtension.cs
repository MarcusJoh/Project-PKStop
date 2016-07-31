using System;
using Java.Util;

namespace AndroidBluetoothLE.Extension
{
    public static class UuidExtension
    {
        public static bool IsEqual(this UUID firstUuid, UUID secondUuid)
        {
            return firstUuid.ToString().Equals(secondUuid.ToString(), StringComparison.OrdinalIgnoreCase);
        }
    }
}