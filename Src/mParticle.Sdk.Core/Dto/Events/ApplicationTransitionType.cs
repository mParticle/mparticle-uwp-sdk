namespace mParticle.Sdk.Core.Dto.Events
{
    // ReSharper disable InconsistentNaming
    
    /// <summary>
    /// This enum represents the application transition types
    /// 
    /// IMPORTANT: Do not change the casing or refactor this enum.  The SDK depends on these values.
    /// </summary>
    public enum ApplicationTransitionType : byte
    {
        Unknown = 0,

        /// <summary>
        /// The application has been initialized
        /// </summary>
        app_init = 1,

        /// <summary>
        /// The application has exited
        /// </summary>
        app_exit = 2,

        /// <summary>
        /// The application is going to the background
        /// </summary>
        app_back = 3,

        /// <summary>
        /// The application is coming to the foreground
        /// </summary>
        app_fore = 4,
    }

    // ReSharper restore InconsistentNaming

}
