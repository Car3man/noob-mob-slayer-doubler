using System.Collections.Generic;
using System.Threading.Tasks;
using Services;
using Unity.Services.Analytics;
using Unity.Services.Core;
using UnityEngine;

namespace ServicesImpls.UnityAnalytics
{
    public class UnityAnalytics : IAnalytics
    {
        public async Task Initialize()
        {
            var options = new InitializationOptions();
            
            try
            {
                await UnityServices.InitializeAsync(options);
                AnalyticsService.Instance.StartDataCollection();
            }
            catch (ConsentCheckException e)
            {
                Debug.LogWarning("[UnityAnalytics] Initialization error: " + e.Reason);
            }
        }

        public void LogEvent(string name, Dictionary<string, object> parameters)
        {
            AnalyticsService.Instance.CustomData(name, parameters);
        }
    }
}
