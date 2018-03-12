using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.Extensions;
using RelevantCodes.ExtentReports;

namespace SeleniumWebLocator.Base
{
    public class ReportLogger : IReportLogger
    {
        private readonly ExtentReports _extent;
        private readonly IWebDriver _webDriver;
        private readonly Stack<ExtentTest> _tests;
        private readonly string _screenShotDir;

        public ReportLogger(string reportPath, ExtentReports extent, IWebDriver webDriver)
        {
            _extent = extent;
            _webDriver = webDriver;
            HasError = false;
            _tests = new Stack<ExtentTest>();
            _screenShotDir = Path.Combine(reportPath, "Screenshots");
            if (!Directory.Exists(_screenShotDir))
            {
                Directory.CreateDirectory(_screenShotDir);
            }
        }

        public void StartTest(string testName)
        {
            ExtentTest parent = null;
            var parentText = "";
            if (_tests.Count > 1)
            {
                parent = _tests.ToArray()[1];
                parentText = parent.Description + "-";
            }

            var test = _extent.StartTest($"{parentText}{testName}", $"{parentText}{testName}");

            parent?.AppendChild(test);

            _tests.Push(test);


        }
        public void EndTest()
        {
            var test = _tests.Pop();
            if (test != null)
            {
                _extent.EndTest(test);
            }
        }


        private string GetScreenshot()
        {
            var screenShotFile = Path.Combine(_screenShotDir, $"{CurrentTest.Description}-{DateTime.Now.Ticks}.png");
            _webDriver.TakeScreenshot().SaveAsFile(screenShotFile, ScreenshotImageFormat.Png);
            return screenShotFile;
        }

        public bool Log(bool success, string msg, string details = "")
        {
            if (!success)
            {
                details += CurrentTest.AddScreenCapture(GetScreenshot());
                HasError = true;
            }

            CurrentTest.Log(success ? LogStatus.Pass : LogStatus.Fail, msg, details);
            return success;
        }


        public void Log(Exception ex, string message)
        {
            var details = CurrentTest.AddScreenCapture(GetScreenshot());
            CurrentTest.Log(LogStatus.Fail, "Exception occured!", details);
            CurrentTest.Log(LogStatus.Fail, message, ex);
            HasError = true;
        }

        private LogStatus GetLogStatus(ReportLogStatus status)
        {
            return (LogStatus)status;
        }

        public void Log(ReportLogStatus status, string message, string details = "")
        {
            var logStatus = GetLogStatus(status);
            if (logStatus == LogStatus.Fail || logStatus == LogStatus.Error)
            {
                var sc = GetScreenshot();
                details += CurrentTest.AddScreenCapture(sc);
                HasError = true;
            }
            CurrentTest.Log(logStatus, message, details);
        }

        public void AddScreenShot(string msg)
        {
            CurrentTest.Log(LogStatus.Info, msg, CurrentTest.AddScreenCapture(GetScreenshot()));
            Thread.Sleep(50);
        }

        public void Close()
        {

            while (_tests.Count > 0)
            {
                var test = _tests.Pop();
                _extent.EndTest(test);
            }

            _extent.Flush();
            _extent.Close();
        }

        public ExtentTest CurrentTest => _tests.Any() ? _tests.Peek() : null;

        public bool HasError { get; private set; }

    }
}