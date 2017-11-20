using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace mParticle.Sdk.UWP
{
    public sealed class CustomEvent
    {
        internal CustomEvent(CustomEventBuilder customEventBuilder)
        {
            if (string.IsNullOrWhiteSpace(customEventBuilder.eventName))
            {
                throw new ArgumentException(message: "Event name cannot be null or empty.", paramName: "Event name");
            }
            this.EventName = customEventBuilder.eventName;
            this.EventType = customEventBuilder.eventType ?? CustomEventType.Other;

            if (customEventBuilder.customAttributes != null)
            {
                this.CustomAttributes = new ReadOnlyDictionary<string, string>(customEventBuilder.customAttributes);
            }

            if (customEventBuilder.customFlags != null)
            {
                this.CustomFlags = new ReadOnlyDictionary<string, List<String>>(customEventBuilder.customFlags);
            }

            this.EventLength = customEventBuilder.eventLength;
        }

        public string EventName { get; private set; }
        public CustomEventType EventType { get; private set; }
        public long? EventLength { get; private set; }
        public ReadOnlyDictionary<string, string> CustomAttributes { get; private set; }
        public ReadOnlyDictionary<string, List<string>> CustomFlags { get; private set; }

        public static CustomEventBuilder Builder(string eventName)
        {
            return new CustomEventBuilder(eventName);
        }

        public class CustomEventBuilder
        {
            internal string eventName;
            internal CustomEventType? eventType;
            internal long? eventLength;
            internal IDictionary<string, string> customAttributes;
            internal IDictionary<string, List<string>> customFlags;

            public CustomEventBuilder(string eventName)
            {
                Name(eventName);
            }

            public CustomEventBuilder Name(string eventName)
            {
                this.eventName = eventName;
                return this;
            }

            public CustomEventBuilder Type(CustomEventType eventType)
            {
                this.eventType = eventType;
                return this;
            }

            public CustomEventBuilder Length(long eventLength)
            {
                this.eventLength = eventLength;
                return this;
            }

            public CustomEventBuilder CustomAttributes(IDictionary<string, string> customAttributes)
            {
                this.customAttributes = customAttributes;
                return this;
            }

            public CustomEventBuilder CustomFlags(IDictionary<string, List<string>> customFlags)
            {
                this.customFlags = customFlags;
                return this;
            }

            public CustomEvent Build()
            {
                return new CustomEvent(this);
            }
        }
    }
}