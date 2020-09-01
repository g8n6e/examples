using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;

namespace ServiceAutoTest.Models
{
    public enum CallType
    {
        [Description("REST")]
        REST,
        [Description("SOAP")]
        SOAP
    }

    //public enum WSDL
    //{
    //    [Description("URL")]
    //    URL,
    //    [Description("File")]
    //    File,
    //    [Description("NA")]
    //    NA
    //}


    public class TestCaseConfig
    {

        public TestCaseConfig(IConfiguration config, string configFileName)
        {
            var tcConfig = new TestCaseConfig();
            config.Bind(tcConfig);

            var headers = new Dictionary<string, string>();
            config.GetSection("Headers").Bind(headers);
            tcConfig.Headers = headers.ToList();

            configureTestCase(tcConfig.EndPoint1, tcConfig.EndPoint2, tcConfig.Headers, tcConfig.CallType, tcConfig.HttpMethod, tcConfig.SoapAction, tcConfig.PayloadFilePath, configFileName);
        }

        private TestCaseConfig()
        {

        }
        public TestCaseConfig(string endpoint1, string endpoint2, List<KeyValuePair<string, string>> headers, CallType callType, string httpMethod, string soapAction="", string payloadFilePath="", string configFileName="")
        {
            configureTestCase(endpoint1, endpoint2, headers, callType, httpMethod, soapAction, payloadFilePath, configFileName);
        }

        private void configureTestCase(string endpoint1, string endpoint2, List<KeyValuePair<string, string>> headers, CallType callType, string httpMethod, string soapAction = "", string payloadFilePath = "", string configFileName = "")
        {
            this.EndPoint1 = endpoint1;
            this.EndPoint2 = endpoint2;
            this.Headers = headers;
            this.CallType = callType;
            this.PayloadFilePath = payloadFilePath;
            this.ConfigFileName = configFileName;

            if (this.CallType == CallType.REST)
            {
                this.HttpMethod = httpMethod.ToUpper();
            }
            if (this.CallType == CallType.SOAP)
            {
                if (string.IsNullOrEmpty(soapAction))
                {
                    throw new Exception($"Test case config for comparing endpoint1: '{endpoint1}' and endpoint2: '{endpoint2}' for a SOAP request is missing required SOAP Action parameter");
                }
                this.SoapAction = soapAction;
                this.HttpMethod = "POST";
            }
        }

        public string EndPoint1 { get; set; }
        public string EndPoint2 { get; set; }
        public List<KeyValuePair<string, string>> Headers { get; set; } = new List<KeyValuePair<string, string>>();
        public CallType CallType { get; set; }
        public string HttpMethod { get; set; }
        public string SoapAction { get; set; }
        public string PayloadFilePath { get; set; }
        public string ConfigFileName { get; set; }
    }
}
