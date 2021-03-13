using IoTHubTrigger = Microsoft.Azure.WebJobs.EventHubTriggerAttribute;

using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Azure.EventHubs;
using System.Text;
using System.Net.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;

namespace SaveTotableStorage.Models
{
    public static class SaveToTableStorage
    {
        private static HttpClient client = new HttpClient();

        [FunctionName("SaveToTableStorage")]
        [return: Table("Messages")]
        public static GhostMessages Run([IoTHubTrigger("messages/events", Connection = "IotHubConnection")] EventData message, ILogger log)
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