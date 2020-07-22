using System;
using System;
using System.Collections.Generic;
using System.ComponentModel;


namespace LigaStavok.UdfsNext.Provider.BetRadar.WebApi.Responses
{
    public partial class SportCategoriesList
    {
        public IEnumerable<Category> Categories { get; set; }

        public DateTimeOffset? GeneratedOn { get; set; }

        public Sport Sport { get; set; }

        public static SportCategoriesList Parse(dynamic dynamicXml)
        {
            if (dynamicXml == null)
            {
                return null;
            }

            var builder = new SportCategoriesList
            {
                Categories  = Category.ParseList(dynamicXml.Categories?.GetCategoryList()),
                GeneratedOn = dynamicXml.GeneratedAt<DateTimeOffset>(),
                Sport       = Responses.Sport.Parse(dynamicXml.Sport)
            };

            return builder;
        }
    }
}