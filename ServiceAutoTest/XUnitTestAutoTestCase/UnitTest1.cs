using ServiceAutoTest.Models;
using LoggerService;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace XUnitTestAutoTestCase
{
    public class UnitTest1
    {
        [Fact]
        public async Task TestAutoTestCaseAsync()
        {
            // this is a poor example of a testcase since it relies on an existing service
            var endpoint1 = "http://test.url.com/endpoint1";
            var endpoint2 = "http://test.url.com/endpoint2";
            var callType = CallType.SOAP;
            var headers = new List<KeyValuePair<string, string>>(){
                new KeyValuePair<string, string>("Authorization", "Basic c3ZjX211bGVfdXNlcjohZG9uS2V5MTIz")
            };
            var soapAction = "SOAP_ACTION_ENDPOINT";

            var payload = "<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:sal=\"http://test.url.com/endpoint\">\n" +
                "   <soapenv:Header/>\n" +
                "   <soapenv:Body>\n" +
                "   </soapenv:Body>\n" +
                "</soapenv:Envelope>";


            ILoggerManager logger = Mock.Of<LoggerManager>();

            string tempDirectory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            Directory.CreateDirectory(tempDirectory);
            TestCaseConfig config = new TestCaseConfig(endpoint1, endpoint2, headers, callType, null, soapAction);
            AutoTestCase testCase = new AutoTestCase(tempDirectory, config, payload, logger, false);

            string report = string.Empty;
            string diff = string.Empty;

            var htmlReportPath = await testCase.Run();
            Assert.True(Path.GetDirectoryName(htmlReportPath) == tempDirectory);
        }

        [Fact]
        public void TestTestCaseConfigJsonBinding()
        {
            var json = "{\n" +
            "  \"EndPoint1\": \"http://test.url.com/endpoint1\",\n" +
            "  \"EndPoint2\": \"http://test.url.com/endpoint2\",\n" +
            "  \"Headers\": [\n" +
            "    { \"Authorization\": \"Basic abc123\" }\n" +
            "  ],\n" +
            "  \"CallType\": \"SOAP\",\n" +
            "  \"SoapAction\": \"SOAP_ACTION_ENDPOINT\"\n" +
            "}";

            var tccBuilder = new ConfigurationBuilder().AddJsonStream(new MemoryStream(Encoding.UTF8.GetBytes(json)));

            var tccConfig = tccBuilder.Build();

            var tempFile = Path.GetTempPath();
            var testCastConfig = new TestCaseConfig(tccConfig, tempFile);
            Assert.True((testCastConfig.EndPoint1 == "http://test.url.com/endpoint1"));
            Assert.True((testCastConfig.CallType == CallType.SOAP));
            Assert.True((testCastConfig.Headers == new List<KeyValuePair<string, string>>(){
                new KeyValuePair<string, string>("Authorization", "Basic abc123")
            }));

        }
    }
}
