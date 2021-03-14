using System;
using System.Net.Http;
using System.Text;
using Microsoft.Azure.EventHubs;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using IoTHubTrigger = Microsoft.Azure.WebJobs.EventHubTriggerAttribute;

namespace SaveTotableStorage
{
    public static class SaveToTableStorage
    {
        private static HttpClient client = new HttpClient();

        [FunctionName("SaveToTableStorage")]
        [return: Table("Messages")]
        public static GhostMessages Run([IoTHubTrigger("messages/events", Connection = "IotHubConnection", ConsumerGroup = "SendToTable")] EventData message, ILogger log)
        {
            try
            {




                var payload = JsonConvert.DeserializeObject<GhostMessages>(Encoding.UTF8.GetString(message.Body.Array));
                payload.PartitionKey = "GhostMessage";
                payload.RowKey = Guid.NewGuid().ToString();

                log.LogInformation("Saving data to Table Storage.");
                return payload;
            }
            catch
            {
                log.LogInformation("Failed to Deserialize message. No data was save to Table Storage");
            }

            return null;

        }
    }
}