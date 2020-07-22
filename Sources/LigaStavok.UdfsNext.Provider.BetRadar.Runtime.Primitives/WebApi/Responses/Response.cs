using System.ComponentModel;


namespace LigaStavok.UdfsNext.Provider.BetRadar.WebApi.Responses
{
    public partial class Response
    {
        public string Action { get; set; }
        
        public string Message { get; set; }

        public string ResponseCode { get; set; }
        
        public static Response Parse(dynamic dynamicXml)
        {
            if (dynamicXml == null)
            {
                return null;
            }

            var builder = new Response
            {
                Action       = dynamicXml.Action,
                Message      = dynamicXml.Message,
                ResponseCode = dynamicXml.ResponseCode,
            };

            return builder;
        }
    }
}