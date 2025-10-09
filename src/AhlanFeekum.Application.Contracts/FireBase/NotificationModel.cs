using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AhlanFeekum.FireBase
{
    public class NotificationModel
    {
        [JsonProperty("deviceId")]
        public string DeviceId { get; set; }

     

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("body")]
        public string Body { get; set; }

        [JsonProperty("data")]
        public DataDto Data { get; set; } = new DataDto();
    }

    public class GoogleNotification
    {      

        [JsonProperty("priority")]
        public string Priority { get; set; } = "high";
        [JsonProperty("data")]
        public DataDto Data { get; set; }
        [JsonProperty("notification")]
        public DataPayload Notification { get; set; }
    }

    public class DataPayload
    {
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("body")]
        public string Body { get; set; }

    }

    public class DataDto
    {
        [JsonProperty("ReferenceId")]
        public string ReferenceId { get; set; }

        [JsonProperty("Type")]
        public string Type { get; set; }
        [JsonProperty("Id")]
        public string Id { get; set; }
        [JsonProperty("UserId")]
        public Guid UserId { get; set; }

        [JsonProperty("IsAcknowledge")]
        public bool IsAcknowledge { get; set; } = false;
        //[JsonProperty("referenceType")]
        //public int ReferenceType { get; set; }
        //[JsonProperty("interestStatus")]
        //public int? InterestStatus { get; set; }

        //[JsonProperty("isSent")]
        //public bool? IsSent { get; set; }
    }
}

