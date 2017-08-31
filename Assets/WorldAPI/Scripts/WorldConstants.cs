using System;

namespace WAPI
{
    /// <summary>
    /// Constants for World API
    /// </summary>
    public static class WorldConstants
    {
        /// <summary>
        /// Version information
        /// </summary>
        public static readonly int MajorVersion = 0;
        public static readonly int MinorVersion = 10;

        /// <summary>
        /// World API present define
        /// </summary>
        public static readonly string WAPIPresentSymbol = "WORLDAPI_PRESENT";

        /// <summary>
        /// World change event constants
        /// </summary>
        public static class WorldChangeEvents
        {
            public const UInt64 ManagerActiveChanged = 1 << 1;
            public const UInt64 GameTimeChanged = 1 << 2;
            public const UInt64 PlayerChanged = 1 << 3;

            public const UInt64 SeaChanged = 1 << 10;
            public const UInt64 LatLngChanged = 1 << 11;
            public const UInt64 SceneMetricsChanged = 1 << 12;

            public const UInt64 TempAndHumidityChanged = 1 << 20;
            public const UInt64 WindChanged = 1 << 21;
            public const UInt64 FogChanged = 1 << 22;
            public const UInt64 RainChanged = 1 << 23;
            public const UInt64 HailChanged = 1 << 24;
            public const UInt64 SnowChanged = 1 << 25;
            public const UInt64 ThunderChanged = 1 << 26;
            public const UInt64 CloudsChanged = 1 << 27;
            public const UInt64 MoonChanged = 1 << 28;
            public const UInt64 SeasonChanged = 1 << 29;
            public const UInt64 VolumeChanged = 1 << 30;

            public const UInt64 ExtensionChanged = (UInt64)1 << 31;
        }
    }
}