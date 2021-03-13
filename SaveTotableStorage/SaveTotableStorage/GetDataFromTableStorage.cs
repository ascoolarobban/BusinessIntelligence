using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Azure.Cosmos.Table;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

namespace SaveTotableStorage.Models

{
    public static class GetDataFromTableStorage
    {
        [FunctionName("GetDataFromTableStorage")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            [Table("Messages")] CloudTable cloudTable,
            ILogger log)
        {

            IEnumerable<GhostMessages> results = await cloudTable.ExecuteQuerySegmentedAsync(new TableQuery<GhostMessages>(), null);
            results = results.OrderBy(ts => ts.Timestamp);


            return new OkObjectResult(results);
        }
    }
}
