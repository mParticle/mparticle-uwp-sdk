using mParticle.Sdk.Core.Dto.Identity;

namespace mParticle.Sdk.UWP
{
    public class IdentityApiResult
    {
        public MParticleUser User { get; internal set; }
        public ErrorResponse Error { get; internal set; }
        public bool Successful { get { return User != null; } }
    }
}