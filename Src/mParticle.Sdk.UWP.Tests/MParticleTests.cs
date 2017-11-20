
using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Windows.Foundation;

namespace mParticle.Sdk.UWP.Tests
{
    [TestClass]
    public class MParticleTests
    {

        [TestInitialize]
        public void SetupTests()
        {
            MParticle.Instance = null;
        }

        [TestCleanup]
        public void CleanupTests()
        {
            MParticle.Instance = null;
        }

        public static IAsyncAction ExecuteOnUIThread(Windows.UI.Core.DispatchedHandler action)
        {
            return Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, action);
        }

        [TestMethod]
        public async Task TestMParticleStartNoOptionsAsync()
        {
            
            Exception e = null;

            await ExecuteOnUIThread(async () =>
            {
                try
                {
                    await MParticle.StartAsync(null);
                }
                catch (Exception ex)
                {
                    e = ex;
                }
                    
            });

            Assert.IsNotNull(e);
            Assert.AreEqual(typeof(InvalidOperationException), e.GetType());
        }

        [TestMethod]
        public async Task TestMParticleStartCorrectOptionsAsync()
        {
            await ExecuteOnUIThread(async () =>
            {
                await MParticle.StartAsync
                (
                    MParticleOptions.Builder("foo", "bar").Build()
                );
            });
            
            Assert.IsNotNull(MParticle.Instance);
        }

        [TestMethod]
        public async Task TestLogEventAsync()
        {
            await ExecuteOnUIThread(async () =>
            {
                await MParticle.StartAsync
                (
                     MParticleOptions.Builder("foo", "bar").Build()
                );
            });

           
            CustomEvent customEvent = CustomEvent.Builder("foo").Build();
            MParticle.Instance.LogEvent(customEvent);
        }

        [TestMethod]
        public void TestCreateEvent()
        {
            CustomEvent customEvent = CustomEvent.Builder("foo").Build();
            Assert.AreEqual("foo", customEvent.EventName);
            Assert.AreEqual(CustomEventType.Other, customEvent.EventType);

            customEvent = CustomEvent.Builder("foo").Type(CustomEventType.Search).Build();
            Assert.AreEqual("foo", customEvent.EventName);
            Assert.AreEqual(CustomEventType.Search, customEvent.EventType);

            Exception e = null;
            try
            {
                customEvent = CustomEvent.Builder(null).Build();
            }
            catch (Exception ex)
            {
                e = ex;
            }
            Assert.IsNotNull(e);
            Assert.AreEqual(typeof(ArgumentException), e.GetType());
        }
    }
}
