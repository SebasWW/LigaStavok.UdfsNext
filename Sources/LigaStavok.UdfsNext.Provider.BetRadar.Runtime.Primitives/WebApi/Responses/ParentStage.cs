using System;
using System.ComponentModel;


namespace LigaStavok.UdfsNext.Provider.BetRadar.WebApi.Responses
{
    public partial class ParentStage
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public DateTimeOffset? Scheduled { get; set; }

        public DateTimeOffset? ScheduledEnd { get; set; }

        public string Type { get; set; }

        public static ParentStage Parse(dynamic dynamicXml)
        {
            if (dynamicXml == null)
            {
                return null;
            }

            var builder = new ParentStage
            {
                Id           = dynamicXml.Id,
                Name         = dynamicXml.Name,
                Scheduled    = dynamicXml.Scheduled<DateTimeOffset?>(),
                ScheduledEnd = dynamicXml.ScheduledEnd<DateTimeOffset?>(),
                Type         = dynamicXml.Type
            };

            return builder;
        }
    }
}