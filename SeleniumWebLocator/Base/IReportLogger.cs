using System;

namespace SeleniumWebLocator.Base
{
    public interface IReportLogger
    {
        void StartTest(string testName);
        void EndTest();
        bool Log(bool condition, string msg, string details = "");
        void Log(Exception ex, string message);
        void Log(ReportLogStatus status, string message, string details = "");
        void AddScreenShot(string msg);
        void Close();
        bool HasError { get; }
    }
}