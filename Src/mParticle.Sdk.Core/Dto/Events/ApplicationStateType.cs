
namespace mParticle.Sdk.Core.Dto.Events
{
    // ReSharper disable InconsistentNaming

    /// <summary>
    /// The application state
    /// </summary>
    public enum ApplicationStateType : byte
    {
        Unknown = 0,

        /// <summary>
        /// The application was not running
        /// </summary>
        not_running = 1,

        /// <summary>
        /// The application was running in the background
        /// </summary>
        background = 2,

        /// <summary>
        /// The application was running in the foreground
        /// </summary>
        foreground = 3,
    }

    // ReSharper restore InconsistentNaming
}
