using System;
using System.Collections.Generic;
using Convai.Scripts.Runtime.Attributes;
using Newtonsoft.Json;
using UnityEngine;

namespace Convai.Scripts.Editor.Setup.AccountsSection {

    [Serializable]
    public class UsageData {
        [JsonProperty( "plan_name" )] [ReadOnly] [SerializeField]
        public string planName;

        [JsonProperty( "expiry_ts" )] [ReadOnly] [SerializeField]
        public string expiryTs;

        [JsonProperty( "daily_limit" )] [ReadOnly] [SerializeField]
        public int dailyLimit;

        [JsonProperty( "monthly_limit" )] [ReadOnly] [SerializeField]
        public int monthlyLimit;

        [JsonProperty( "extended_isAllowed" )] [ReadOnly] [SerializeField]
        public bool extendedIsAllowed;

        [JsonProperty( "daily_usage" )] [ReadOnly] [SerializeField]
        public int dailyUsage;

        [JsonProperty( "monthly_usage" )] [ReadOnly] [SerializeField]
        public int monthlyUsage;

        [JsonProperty( "extended_usage" )] [ReadOnly] [SerializeField]
        public int extendedUsage;
    }

    [Serializable]
    public class Metric {
        [JsonProperty( "id" )] [ReadOnly] [SerializeField]
        public string Id;

        [JsonProperty( "name" )] [ReadOnly] [SerializeField]
        public string Name;

        [JsonProperty( "units" )] [ReadOnly] [SerializeField]
        public string Units;

        [JsonProperty( "usage_details" )] [ReadOnly] [SerializeField]
        public List<UsageDetail> UsageDetails;

        [JsonProperty( "is_extended" )] [ReadOnly] [SerializeField]
        public bool? IsExtended;
    }

    [Serializable]
    public class UsageDetail {
        [JsonProperty( "label" )] [ReadOnly] [SerializeField]
        public string Label;

        [JsonProperty( "limit" )] [ReadOnly] [SerializeField]
        public double Limit;

        [JsonProperty( "usage" )] [ReadOnly] [SerializeField]
        public double Usage;
    }

    [Serializable]
    public class DetailedUsage {
        [JsonProperty( "metrics" )] [ReadOnly] [SerializeField]
        public List<Metric> Metrics;
    }

}