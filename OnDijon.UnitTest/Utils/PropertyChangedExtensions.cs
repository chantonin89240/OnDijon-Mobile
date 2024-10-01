using System;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OnDijon.UnitTest.Utils
{
    public static class PropertyChangedExtensions
    {
        public static async Task<bool> WaitForPropertyChanged(this INotifyPropertyChanged owner, string propertyName, Action action, int timeoutMs = 1000)
        {
            return await WaitForPropertiesChanged(owner, new string[] { propertyName }, action, timeoutMs);
        }

        public static async Task<bool> WaitForPropertiesChanged(this INotifyPropertyChanged owner, string[] propertiesNames, Action action, int timeoutMs = 1000)
        {
            var cancellationToken = new CancellationTokenSource(timeoutMs).Token;
            var task = Task.Run(() =>
            {
                var propertiesChanged = propertiesNames.ToDictionary(key => key, value => false);
                bool allPropertiesChanged = false;

                owner.PropertyChanged += (sender, e) =>
                {
                    if (propertiesChanged.ContainsKey(e.PropertyName))
                    {
                        propertiesChanged[e.PropertyName] = true;
                        allPropertiesChanged = propertiesChanged.All(p => p.Value);
                    }
                };

                action.Invoke();

                while (!allPropertiesChanged)
                    cancellationToken.ThrowIfCancellationRequested();
            }, cancellationToken);

            try
            {
                await task;
            }
            catch (OperationCanceledException e)
            {
                Console.WriteLine($"{nameof(OperationCanceledException)} thrown with message: {e.Message}");
            }

            return !task.IsCanceled;
        }
    }
}
