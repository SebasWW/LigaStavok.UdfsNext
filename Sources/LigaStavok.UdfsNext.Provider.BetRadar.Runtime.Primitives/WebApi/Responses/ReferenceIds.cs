using System.Collections.Generic;
using System;
using System.Linq;

namespace LigaStavok.UdfsNext.Provider.BetRadar.WebApi.Responses
{
    public static class ReferenceIds
    {
        
        public static Dictionary<string, string> Parse(dynamic dynamicXml)
        {
            if (dynamicXml == null)
            {
                return new Dictionary<string, string>();
            }

            return ((IEnumerable<dynamic>) dynamicXml.GetReferenceIdList())
                .ToDictionary(x => (string)x.Name, x => (string)x.Value);
        }
    }
}