using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeslaKwhMeter.Base.Models.Prometheus
{
    internal class QueryResult
    {
        public string status { get; set; }
        public QueryResultData data { get; set; }
    }

    internal class QueryResultData
    {
        public string resultType { get; set; }
        public List<QueryResultVector> result { get; set; }
    }

    internal class QueryResultVector
    {
        //public List<QueryResultMetric> metric { get; set; }
        public List<object> value { get; set; }
    }

    internal class QueryResultMetric
    {
        public string __name__ { get; set; }
        public string domain { get; set; }
        public string entity { get; set; }
        public string friendly_name { get; set; }
        public string instance { get; set; }
        public string job { get; set; }
    }
}
