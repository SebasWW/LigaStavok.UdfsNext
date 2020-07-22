using System;
using System.Collections.Generic;
using System.ComponentModel;


namespace LigaStavok.UdfsNext.Provider.BetRadar.WebApi.Responses
{
    public partial class CoverageInfo
    {
        public IEnumerable<Coverage> Coverages { get; set; }

        
        public string Level { get; set; }
        
        public bool LiveCoverage { get; set; }

        public static CoverageInfo Parse(dynamic dynamicXml)
        {
            if (dynamicXml == null)
            {
                return null;
            }

            var builder = new CoverageInfo
            {
                Coverages    = Coverage.ParseList(dynamicXml.GetCoverageList()),
                Level        = dynamicXml.Level<string>(),
                LiveCoverage = dynamicXml.LiveCoverage<bool>(),
            };

            return builder;
        }
    }
}