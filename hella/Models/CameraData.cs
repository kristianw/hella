using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace hella.Models
{
    public class ContainerModel
    {
        [JsonProperty(PropertyName = "counts")]
        public List<Count> Counts { get; set; }
    }

    public class Count
    {
        [JsonProperty(PropertyName = "data")]
        public List<CameraData> Datas { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
    }

    public class CameraData
    {
        [JsonProperty(PropertyName = "class")]
        string ObjectClass { get; set; }

        [JsonProperty(PropertyName = "in")]
        string InCount { get; set; }

        [JsonProperty(PropertyName = "out")]
        string OutCount { get; set; }


    }
}