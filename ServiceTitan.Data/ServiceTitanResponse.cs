//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ServiceTitan.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class ServiceTitanResponse
    {
        public int Id { get; set; }
        public Nullable<int> ClientId { get; set; }
        public Nullable<int> LeadCallid { get; set; }
        public Nullable<System.TimeSpan> duration { get; set; }
        public string from { get; set; }
        public string to { get; set; }
        public string RowServiceTitanJson { get; set; }
        public string FilePath { get; set; }
        public string assemblyairesponse { get; set; }
        public Nullable<System.DateTime> createdOn { get; set; }
        public Nullable<int> syncId { get; set; }
        public Nullable<int> ResponseStatus { get; set; }
        public string ErrorCode { get; set; }
        public Nullable<System.DateTime> ReceivedOn { get; set; }
    
        public virtual ClientMaster ClientMaster { get; set; }
        public virtual SyncHistory SyncHistory { get; set; }
    }
}
