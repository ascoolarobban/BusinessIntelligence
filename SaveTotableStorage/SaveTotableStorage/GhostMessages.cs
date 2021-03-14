using System;
using System.Collections.Generic;
using Microsoft.Azure.Cosmos.Table;
using EntityProperty = Microsoft.Azure.Cosmos.Table.EntityProperty;
using ITableEntity = Microsoft.Azure.Cosmos.Table.ITableEntity;
using TableEntity = Microsoft.WindowsAzure.Storage.Table.TableEntity;

namespace SaveTotableStorage
{
    public class GhostMessages : TableEntity
    {
        public string deviceId { get; set; }
        public Boolean Activity { get; set; }
        public string GhostSensor { get; set; }
        public Int64 TimeStamp { get; set; }
    }
}
