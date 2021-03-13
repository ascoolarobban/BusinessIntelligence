using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Azure.Cosmos.Table;
using EntityProperty = Microsoft.Azure.Cosmos.Table.EntityProperty;
using ITableEntity = Microsoft.Azure.Cosmos.Table.ITableEntity;
using TableEntity = Microsoft.WindowsAzure.Storage.Table.TableEntity;

namespace SaveTotableStorage.Models
{
    public class GhostMessages : TableEntity, ITableEntity
    {
        public string deviceId { get; set; }
        public Boolean Activity { get; set; }
        public string GhostSensor { get; set; }
        public Int64 TimeStamp { get; set; }
        public void ReadEntity(IDictionary<string, EntityProperty> properties, OperationContext operationContext)
        {
            throw new NotImplementedException();
        }

        public IDictionary<string, EntityProperty> WriteEntity(OperationContext operationContext)
        {
            throw new NotImplementedException();
        }
    }
}
