
namespace mParticle.Sdk.Core.Dto.Events
{
    public enum LocationTrackingMode
    {
        // ReSharper disable InconsistentNaming - the sdk depends on this casing.

        /// <summary>
        /// allow the app to decide 
        /// </summary>
        appdefined,

        /// <summary>
        /// force the sdk to look enable location tracking
        /// </summary>
        forceenable,

        /// <summary>
        /// force the sdk to honor that the app is disabling location tracking
        /// </summary>
        forcedisable,

        // ReSharper restore InconsistentNaming
    }
}
