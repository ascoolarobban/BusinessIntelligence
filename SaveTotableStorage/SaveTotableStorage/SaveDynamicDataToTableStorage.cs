using IoTHubTrigger = Microsoft.Azure.WebJobs.EventHubTriggerAttribute;

using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Azure.EventHubs;
using System.Text;
using System.Net.Http;
using Microsoft.Extensions.Logging;

namespace SaveTotableStorage
{
    public static class SaveDynamicDataToTableStorage
    {
        private static HttpClient client = new HttpClient();

        [FunctionName("SaveDynamicDataToTableStorage")]
        public static void Run([IoTHubTrigger("messages/events", Connection = "IotHubConnection", ConsumerGroup = "dynamic")] EventData message, ILogger log)
        {
            log.LogInformation($"C# IoT Hub trigger function processed a message: {Encoding.UTF8.GetString(message.Body.Array)}");
        }
    }
}