using System;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using RelevantCodes.ExtentReports;

namespace SeleniumWebLocator.Base
{
    public class BaseTest
    {
        protected static App App;
        protected static IWebDriver Driver;
        private static string _browserType = "ie";
        protected static string WebsiteBaseUrl = "";

        // Schreibt die Aufrufe von Trace nach Console.Out und somit in das Resharper Output-Window!
        //private static TraceListener _listener;

        public static void InitTests(TestContext testContext)
        {
            if (App != null)
                return;

            Trace.Listeners.Add(new ConsoleTraceListener());

            testContext.WriteLine($"Test initialized '{testContext.TestName}'.");
            testContext.WriteLine("Instantiation of WebDriver ... ");

            WebsiteBaseUrl = ConfigurationManager.AppSettings["WebsiteBaseUrl"];
            _browserType = ConfigurationManager.AppSettings["BrowserType"];
            var timeOutStr = ConfigurationManager.AppSettings["DefaultTimeOutInSeconds"] ?? "4";
            var timeOut = int.Parse(timeOutStr);

            try
            {
                switch (_browserType.ToLower())
                {
                    case "ie":
                        var service = InternetExplorerDriverService.CreateDefaultService();
                        service.SuppressInitialDiagnosticInformation = true;
                        Driver = new InternetExplorerDriver(service);
                        break;
                    case "firefox":
                        Driver = new FirefoxDriver();
                        break;
                    case "chrome":
                        Driver = new ChromeDriver();
                        break;
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine("Instantiation of WebDriver failed: " + ex.Message);
                throw;
            }

            var reportPath = testContext.TestDir.Replace(" ", "_") + "_Report";

            if (!Directory.Exists(reportPath))
            {
                Directory.CreateDirectory(reportPath);
            }

            ReportFile = Path.Combine(reportPath, $"{_browserType.ToLower()}-report.html");
            var extent = new ExtentReports(ReportFile, CultureInfo.GetCultureInfo("de"), ReplaceExisting: true, Order: DisplayOrder.NewestFirst);

            ReportLogger = new ReportLogger(reportPath, extent, Driver);
            App = new App(Driver, WebsiteBaseUrl, ReportLogger, timeOut);

            Driver.Manage().Window.Size = new System.Drawing.Size(1600, 1000);

            App.Open("/");

        }

        public TestContext TestContext { get; set; }

        public static IReportLogger ReportLogger { get; private set; }

        public static string ReportFile { get; set; }

        [AssemblyCleanup]
        protected static void CleanupTests()
        {

            Driver.Close();

            //Trace.Listeners.Remove(_listener);

            ReportLogger.Close();

            var reportFile = new FileInfo(ReportFile);
            if (reportFile.DirectoryName != null)
            {
                // ReSharper disable once PossibleNullReferenceException
                File.Copy(ReportFile, Path.Combine(reportFile.Directory.Parent.FullName, $"{_browserType.ToLower()}-report.html"), true);
            }
        }


        [TestInitialize]
        public void TestcaseInitialize()
        {
            StartTest(TestContext.TestName);
        }

        public static void StartTest(string testName)
        {
            ReportLogger.StartTest(testName);
        }

        [TestCleanup]
        public void TestcaseCleanUp()
        {
            EndTest();
        }

        public static void EndTest()
        {
            if (ReportLogger.HasError)
            {
                ReportLogger.Log(ReportLogStatus.Fail, "Test ended with errors!");
            }
            else
            {
                ReportLogger.Log(ReportLogStatus.Pass, "Test ended successfully!");
            }
            ReportLogger.EndTest();
            if (ReportLogger.HasError)
                Assert.Fail($"Test ended with errors! See report 'file://{ReportFile}'");
        }


        public static void TestFailed(Exception ex)
        {
            ReportLogger.Log(ex, $"Error: {ex.Message}");
            ReportLogger.Log(ex, $"Stacktrace: {ex.StackTrace}");

            ReportLogger.EndTest();
            Assert.Fail($"Test ended with exception! See report 'file://{ReportFile}'.\n  {ex.Message}:\nStacktrace: {ex.StackTrace}");
        }

    }


}