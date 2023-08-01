using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services
{
    public interface IAnalytics
    {
        Task Initialize();
        void LogEvent(string name, Dictionary<string, object> parameters);
    }
}