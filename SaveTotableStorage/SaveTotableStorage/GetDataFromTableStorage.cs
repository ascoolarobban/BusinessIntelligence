using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
namespace SaveTotableStorage

    {
        public class GhostMessages : TableEntity
        {
            public string deviceId { get; set; }
            public Boolean Activity { get; set; }
            public string GhostSensor { get; set; }
            public Int64 TimeStamp { get; set; }
        }
        public static class GetDataFromTableStorage
        {
            
            [FunctionName("GetDataFromTableStorage")]
            public static async Task<IActionResult> Run(
                [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
                [Table("Messages")] CloudTable cloudTable,
                ILogger log)
            {
                string limit = req.Query["limit"];
                string orderby = req.Query["orderby"];
                IEnumerable<GhostMessages> results = await cloudTable.ExecuteQuerySegmentedAsync(new TableQuery<GhostMessages>(), null);
                if(orderby == "desc")
                {
                    results = results.OrderByDescending(ts=>ts.TimeStamp);
                }
           
                if(limit != null)
                {
                    results = results.Take(Int32.Parse(limit));
                }

                return results != null
                    ? (ActionResult) new OkObjectResult(results)
                    : new BadRequestObjectResult("[]") ;
                


                return new OkObjectResult(results);
            }
    }
    }
