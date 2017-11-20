using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using mParticle.Sdk.Core.Dto.Events;

namespace mParticle.Sdk.UWP
{
    public class IdentityApiRequest
    {
        public IReadOnlyDictionary<UserIdentityType, string> UserIdentities { get; }
        public IdentityApiRequestBuilder.OnUserAlias UserAliasDelegate { get; }

        private IdentityApiRequest() { }
        internal IdentityApiRequest(IdentityApiRequestBuilder identityApiRequestBuilder)
        {
            if (identityApiRequestBuilder.userIdentities != null)
            {
                UserIdentities = new ReadOnlyDictionary<UserIdentityType, string>(identityApiRequestBuilder.userIdentities);
            }
            else
            {
                UserIdentities = new ReadOnlyDictionary<UserIdentityType, string>(new Dictionary<UserIdentityType, string>());
            }
            UserAliasDelegate = identityApiRequestBuilder.userAliasDelegate;
        }

        public static IdentityApiRequestBuilder EmptyUser()
        {
            return new IdentityApiRequestBuilder();
        }

        public static IdentityApiRequestBuilder WithUser(MParticleUser user)
        {
            return new IdentityApiRequestBuilder(user);
        }

        public class IdentityApiRequestBuilder
        {
            internal IDictionary<UserIdentityType, string> userIdentities;
            internal OnUserAlias userAliasDelegate;

            public IdentityApiRequestBuilder() { }

            public IdentityApiRequestBuilder(MParticleUser user)
            {
                if (user != null)
                {
                    this.UserIdentities(user.UserIdentities.ToDictionary(kvp => kvp.Key, kvp => kvp.Value));
                }
            }

            public IdentityApiRequestBuilder Email(string email)
            {
                return UserIdentity(UserIdentityType.Email, email);
            }

            public IdentityApiRequestBuilder CustomerId(string customerId)
            {
                return UserIdentity(UserIdentityType.CustomerId, customerId);
            }

            public IdentityApiRequestBuilder UserIdentity(UserIdentityType identityType, string identityValue)
            {
                if (this.userIdentities == null)
                {
                    this.UserIdentities(new Dictionary<UserIdentityType, string>());
                }
                this.userIdentities[identityType] = identityValue;
                return this;
            }

            public IdentityApiRequestBuilder UserIdentities(IDictionary<UserIdentityType, string> userIdentities)
            {
                this.userIdentities = userIdentities;
                return this;
            }

            public IdentityApiRequestBuilder UserAliasDelegate(OnUserAlias userAliasDelegate)
            {
                this.userAliasDelegate = userAliasDelegate;
                return this;
            }

            public delegate void OnUserAlias(MParticleUser oldUser, MParticleUser newUser);

            public IdentityApiRequest Build()
            {
                return new IdentityApiRequest(this);
            }
        }
    }
}