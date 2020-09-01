using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.XmlDiffPatch;
using System.IO;
using System.Xml;
using System.IO.Compression;
using System.Linq;
using Microsoft.Extensions.Configuration;
using LoggerService;
using System.Threading;

namespace ServiceAutoTest.Models
{
    public class AutoTestCase
    {
        public readonly string ReportFileName = "reportResults.html";

        public readonly string PayloadFileName = "payload.txt";

        protected readonly ILoggerManager logger;

        public AutoTestCase(string testCaseDirectoryPath, TestCaseConfig config, string payload, ILoggerManager logger, bool allowArchving)
        {
            this.CurrentFolderPath = testCaseDirectoryPath;
            this.TestCaseConfig = config;
            this.Payload = payload;
            this.logger = logger;
            this.AllowArchiving = allowArchving;
        }

        public AutoTestCase(string testCaseDirectoryPath, TestCaseConfig testCaseConfig, string payloadFileName, string reportResultsFileName, ILoggerManager logger, bool allowArchiving)
        {
            this.CurrentFolderPath = testCaseDirectoryPath;
            this.logger = logger;
            this.AllowArchiving = allowArchiving;

            this.PayloadFileName = payloadFileName;
            this.ReportFileName = reportResultsFileName;
         
            this.TestCaseConfig = testCaseConfig;
            if (string.IsNullOrEmpty(this.TestCaseConfig.PayloadFilePath))
            {               
                this.Payload = File.ReadAllText(Path.Combine(this.CurrentFolderPath, PayloadFileName));
            }
            else
            {
                this.Payload = File.ReadAllText(this.TestCaseConfig.PayloadFilePath);
                this.WriteTextFile(Path.Combine(this.CurrentFolderPath, PayloadFileName), this.Payload).Wait();
            }
                      
        }

        public TestCaseConfig TestCaseConfig { get; set; }
        public string Payload { get; set; }

        public string CurrentFolderPath { get; set; }
        public bool AllowArchiving { get; set; }

        private HttpMethod GetHttpMethod(string httpMethod)
        {
            httpMethod = httpMethod.ToUpper();
            switch (httpMethod)
            {
                case "POST": return HttpMethod.Post;
                case "PUT": return HttpMethod.Put;
                case "GET": return HttpMethod.Get;
                case "DELETE": return HttpMethod.Delete;
                case "PATCH": return HttpMethod.Patch;
                default: return null;
            }
        }

        public async Task<string> Run()
        {
            if (this.AllowArchiving)
            {
                // Delete log files older than today since they are already archived
                var pattern = "*logfile*";
                var logFiles = Directory.GetFiles(this.CurrentFolderPath, pattern);
                foreach (string logFile in logFiles)
                {
                    FileInfo fi = new FileInfo(logFile);
                    if (fi.LastAccessTime < DateTime.Now.AddDays(-1))
                        fi.Delete();
                }
                this.ArchiveFiles();
            }

            // Remove any existing responses and reports. Although they get overwritten this will prevent confusion should an error occur and a report or response isn't overwritten
            var allowedExtensions = new[] { "endpoint1.response.json", "endpoint2.response.json", "endpoint1.resp", "endpoint2.resp", this.ReportFileName };
            var filesToDelete = Directory
                .GetFiles(this.CurrentFolderPath)
                .Where(file => allowedExtensions.Any(file.ToLower().EndsWith))
                .ToList();

            foreach (var file in filesToDelete)
            {
                File.Delete(file);
            }

            HttpResponseMessage message1 = null;
            HttpResponseMessage message2 = null;

            var string1 = string.Empty;
            var string2 = string.Empty;

            if (TestCaseConfig.CallType == CallType.SOAP)
            {
                message1 = await SendSoapRequest(TestCaseConfig.EndPoint1, TestCaseConfig.SoapAction, TestCaseConfig.Headers, Payload);
                logger.LogInfo($"{TestCaseConfig.HttpMethod} SOAP payload to {TestCaseConfig.EndPoint1}");
                message2 = await SendSoapRequest(TestCaseConfig.EndPoint2, TestCaseConfig.SoapAction, TestCaseConfig.Headers, Payload);
                logger.LogInfo($"{TestCaseConfig.HttpMethod} SOAP payload to {TestCaseConfig.EndPoint2}");

                string1 = await message1.Content.ReadAsStringAsync();
                string2 = await message2.Content.ReadAsStringAsync();
            }

            if (TestCaseConfig.CallType == CallType.REST)
            {
                HttpMethod httpMethod = GetHttpMethod(this.TestCaseConfig.HttpMethod);
                if (httpMethod == null)
                {
                    logger.LogError($"The config located at [{TestCaseConfig.ConfigFileName}] has an invalid HttpMethod value of [{this.TestCaseConfig.HttpMethod}]. Must be one of the following: 'GET', 'POST', 'PUT', 'PATCH', 'DELETE'");
                    throw new Exception($"The config located at [{TestCaseConfig.ConfigFileName}] has an invalid HttpMethod value of [{this.TestCaseConfig.HttpMethod}]. Must be one of the following: 'GET', 'POST', 'PUT', 'PATCH', 'DELETE'");
                }
                logger.LogInfo($"{TestCaseConfig.HttpMethod} REST payload to {TestCaseConfig.EndPoint1}");
                message1 = await SendRestRequest(TestCaseConfig.EndPoint1, httpMethod, TestCaseConfig.Headers, Payload);
                logger.LogInfo($"{TestCaseConfig.HttpMethod} REST payload to {TestCaseConfig.EndPoint2}");
                message2 = await SendRestRequest(TestCaseConfig.EndPoint2, httpMethod, TestCaseConfig.Headers, Payload);

                string1 = await message1.Content.ReadAsStringAsync();
                string2 = await message2.Content.ReadAsStringAsync();

                var json1Path = Path.Combine(this.CurrentFolderPath, "endpoint1.response.json");
                Task json1WriteTask = this.WriteTextFile(json1Path, string1);

                var json2Path = Path.Combine(this.CurrentFolderPath, "endpoint2.response.json");
                Task json2WriteTask = this.WriteTextFile(json2Path, string2);

                Task.WaitAll(json1WriteTask, json2WriteTask);
                string1 = (JsonConvert.DeserializeXmlNode(string1, "root")).OuterXml;
                string2 = (JsonConvert.DeserializeXmlNode(string2, "root")).OuterXml;

            }
          
            string message1Path = Path.Combine(this.CurrentFolderPath, "endpoint1.resp");
            logger.LogInfo($"Creating response from endpoint1 at report at {message1Path}");

            Task resp1Task = this.WriteTextFile(message1Path, string1);
            
            string message2Path = Path.Combine(this.CurrentFolderPath, "endpoint2.resp");
            logger.LogInfo($"Creating response from endpoint2 at report at {message2Path}");

            Task resp2Task = this.WriteTextFile(message2Path, string2);

            Task.WaitAll(resp1Task, resp2Task);
           
            string htmlResultPath = Path.Combine(this.CurrentFolderPath, this.ReportFileName);   
            logger.LogInfo($"Generating report at {htmlResultPath}");          
            GenerateXmlDiffReport(message1Path, string1, message2Path, string2, htmlResultPath, XmlDiffOptions.IgnoreChildOrder | XmlDiffOptions.IgnorePrefixes | XmlDiffOptions.IgnoreWhitespace | XmlDiffOptions.IgnoreComments);

            return htmlResultPath;
        }



        private void GenerateXmlDiffReport(string sourceXmlFile, string sourceXmlContent, string changedXmlFile, string changedXmlContent, string resultHtmlViewFile, XmlDiffOptions options)
        {           
            MemoryStream diffgram = new MemoryStream();
            XmlTextWriter diffgramWriter = new XmlTextWriter(new StreamWriter(diffgram));
            var sourceXmlStream = new MemoryStream(sourceXmlContent.ToByteArray());
            XmlReader sourceXmlReader = XmlReader.Create(new StreamReader(sourceXmlStream));
            XmlReader changedXmlReader = XmlReader.Create(new StreamReader(new MemoryStream(changedXmlContent.ToByteArray())));

            logger.LogInfo("Comparing " + sourceXmlFile + " & " + changedXmlFile);
            XmlDiff xmlDiff = new XmlDiff(options);
            bool bIdentical = xmlDiff.Compare(sourceXmlReader, changedXmlReader, diffgramWriter);

            logger.LogInfo("Files compared " + (bIdentical ? "identical." : "different."));

            var resultMS = new MemoryStream();
            var resultSW = new StreamWriter(resultMS);
            TextWriter resultHtml = resultSW;
           
            //Wrapping
            resultHtml.Write("<html><style>td{ max-width:1000px; }</style><body><table>");
           
            diffgram.Seek(0, SeekOrigin.Begin);
            XmlDiffView xmlDiffView = new XmlDiffView();
            sourceXmlStream.Position = 0;
            XmlTextReader sourceReader = new XmlTextReader(sourceXmlStream);
            
            sourceReader.XmlResolver = null;
            xmlDiffView.Load(sourceReader, new XmlTextReader(diffgram));
            //This gets the differences but just has the 
            //rows and columns of an HTML table
            xmlDiffView.SideBySideHtmlHeader(sourceXmlFile, changedXmlFile, bIdentical, resultHtml);
            xmlDiffView.GetHtml(resultHtml);
            xmlDiffView.GetHtml(resultHtml);

            resultHtml.WriteLine("</table></table></body></html>");
            using (var fs = new FileStream(resultHtmlViewFile, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite))
            {
                resultMS.WriteTo(fs);
            }
            resultSW.Close();
            resultMS.Close();
            resultHtml.Close();

            logger.LogInfo(resultHtmlViewFile + " saved successfully.");
        }


        private async Task<HttpResponseMessage> SendSoapRequest(string baseUrl, string soapAction, List<KeyValuePair<string, string>> headers, string xmlPayload)
        {
            using (var client = new HttpClient())
            {             
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));
                var httpContent = new StringContent(xmlPayload, Encoding.UTF8, "application/xml");
                client.DefaultRequestHeaders.Add("SOAPAction", soapAction);

                foreach (var header in headers)
                {
                    client.DefaultRequestHeaders.Add(header.Key, header.Value);
                }

                return await client.PostAsync(baseUrl, httpContent);
            }
        }

        private async Task<HttpResponseMessage> SendRestRequest(string url, HttpMethod httpMethod, List<KeyValuePair<string, string>> headers, string jsonPayload)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var httpContent = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                foreach (var header in headers)
                {
                    client.DefaultRequestHeaders.Add(header.Key, header.Value);
                }

                var request = new HttpRequestMessage(httpMethod, url);
                request.Content = httpContent;
                return await client.SendAsync(request);
            }
        }

        private async Task<string> ReadTextFile(string filePath)
        {
            string content = string.Empty;
            using (var inStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (StreamReader reader = new StreamReader(inStream))
            {
                content = await reader.ReadToEndAsync();
                reader.Close();
            }
            return content;
        }

        private async Task WriteTextFile(string filePath, string content)
        {
            using (var outStream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite))
            {
                using (StreamWriter writer = new StreamWriter(outStream))
                {
                    await writer.WriteAsync(content);
                }
            }
        }

    

        private void ArchiveFiles()
        {
            logger.LogInfo($"Archiving Files for test case {this.CurrentFolderPath}");
            string startPath = this.CurrentFolderPath;
            string archivePath = Path.Combine(this.CurrentFolderPath, "Archive");
            Directory.CreateDirectory(archivePath);
            var respFile = Path.Combine(this.CurrentFolderPath, "endpoint1.resp");
            var lastCreated = DateTime.Now;
 
            if (File.Exists(respFile))
            {
                lastCreated = File.GetCreationTime(Path.Combine(this.CurrentFolderPath, "endpoint1.resp"));
            }
            var zipName = lastCreated.ToString("yyyyMMddHHmmss") + ".zip";
            string zipPath = Path.Combine(archivePath, zipName);
            var files = Directory.GetFiles(startPath);

            if (files.Any())
            {
                using (FileStream zipToOpen = new FileStream(zipPath, FileMode.Create))
                {
                    using (ZipArchive archive = new ZipArchive(zipToOpen, ZipArchiveMode.Create))
                    {
                        foreach (string file in files)
                        {
                            ZipArchiveEntry readmeEntry = archive.CreateEntryFromFile(file, Path.GetFileName(file));
                        }
                    }
                }            
            }             
        }
    }
}
