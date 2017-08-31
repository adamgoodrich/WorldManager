using System;

namespace WAPI
{
    /// <summary>
    /// 
    /// Base class for any World API extensions.
    /// 
    /// Use this to inject data or behaviour to be executed in LateUpdate.
    /// 
    /// Because this is a generic class, make sure you dont mess with any other 
    /// extensions that may be present.
    /// 
    /// </summary>
    [Serializable]
    public class WorldManagerDataExtension
    {
        /// <summary>
        /// Add your handler here if you want things to happen in Update
        /// </summary>
        public virtual void Update()
        {
        }

        /// <summary>
        /// Add your own handler here if you want things to happen in the LateUpdate
        /// </summary>
        public virtual void LateUpdate()
        {
        }
    }
}
