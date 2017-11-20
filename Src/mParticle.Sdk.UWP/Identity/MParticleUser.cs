using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using mParticle.Sdk.Core.Dto.Events;

namespace mParticle.Sdk.UWP
{
    public sealed class MParticleUser
    {
        public long Mpid { get; }

        private readonly PersistenceManager persistenceManager;

        private MParticleUser() { }
        internal MParticleUser(long mpid, PersistenceManager persistenceManager)
        {
            this.Mpid = mpid;
            this.persistenceManager = persistenceManager;
        }

        public IReadOnlyDictionary<UserIdentityType, string> UserIdentities
        {
            get
            {
                var identities = persistenceManager.UserIdentities(Mpid);
                return new ReadOnlyDictionary<UserIdentityType, string>
                    (
                        identities.ToDictionary(x => x.Name, x => x.Identity)
                    );
            }
            internal set
            {
                if (value == null)
                {
                    persistenceManager.SetUserIdentities(Mpid, null);
                }
                else
                {
                    var currentIdentities = persistenceManager.UserIdentities(Mpid);

                    foreach (var identity in value)
                    {
                        UserIdentity currentIdentity = null;
                        var matchedIdentities = currentIdentities.Where(id => id.Name == identity.Key);
                        if (matchedIdentities.Count() > 0)
                        {
                            currentIdentity = matchedIdentities.First();
                            currentIdentity.Identity = identity.Value;
                        }
                        else
                        { 
                            currentIdentity = new UserIdentity()
                            {
                                Name = identity.Key,
                                Identity = identity.Value,
                                IsFirstSeen = true,
                                DateFirstSet = DateTimeOffset.Now.ToUnixTimeMilliseconds()
                            };
                            currentIdentities.Add(currentIdentity);
                        }
                        
                    }
                    persistenceManager.SetUserIdentities(Mpid, currentIdentities);
                }
            }
        }

        public async Task<IdentityApiResult> ModifyAsync(IdentityApiRequest identityApiRequest)
        {
            return await MParticle.Instance.Identity.ModifyAsync(this, identityApiRequest);
        }

        public IReadOnlyDictionary<string, object> UserAttributes
        {
            get
            {
                var userAttributes = persistenceManager.UserAttributes(Mpid);
                return new ReadOnlyDictionary<string, object>(userAttributes);
            }
        }

        public void UserTag(string key)
        {
            var attributes = UserAttributes.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            attributes[key] = null;
            persistenceManager.SetUserAttributes(Mpid, attributes);
        }

        public void UserAttribute(string key, string value)
        {
            var attributes = UserAttributes.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            attributes[key] = value;
            persistenceManager.SetUserAttributes(Mpid, attributes);
        }

        public void UserAttribute(string key, IList<string> value)
        {
            var attributes = UserAttributes.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            attributes[key] = value;
            persistenceManager.SetUserAttributes(Mpid, attributes);
        }

        public void RemoveUserAttribute(string key)
        {
            var attributes = UserAttributes.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            attributes.Remove(key);
            persistenceManager.SetUserAttributes(Mpid, attributes);
        }

        public override bool Equals(object obj)
        {
            var user = obj as MParticleUser;
            return user != null &&
                   Mpid == user.Mpid;
        }

        public override int GetHashCode()
        {
            return 348581357 + Mpid.GetHashCode();
        }
    }
}