using System;

namespace WAPI
{
    /// <summary>
    /// Arguments passed to the change handler when the world changes
    /// </summary>
    public struct WorldChangeArgs
    {
        public UInt64 changeMask;
        public WorldManager manager;

        /// <summary>
        /// Constructore
        /// </summary>
        /// <param name="mask">Mask</param>
        /// <param name="worldManager">World manager that raised this</param>
        public WorldChangeArgs(UInt64 mask, WorldManager worldManager)
        {
            changeMask = mask;
            manager = worldManager;
        }

        /// <summary>
        /// Handy helper to see if the thing you are interested in has changed
        /// </summary>
        /// <param name="changes">Changes to check for</param>
        /// <returns>True if it has changed</returns>
        public bool HasChanged(UInt64 changes)
        {
            return ((changes & changeMask) != 0);
        }
    }

    /// <summary>
    /// If you want to listen to World API chnage events then implement this on your class and make sure
    /// you register to receive events.
    /// </summary>
    public interface IWorldApiChangeHandler
    {
        /// <summary>
        /// Handler to be called when something changes in the world
        /// </summary>
        /// <param name="changeArgs">Change arguments</param>
        void OnWorldChanged(WorldChangeArgs changeArgs);
    }
}
