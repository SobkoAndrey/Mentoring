using NLog;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace MvcMusicStore
{
    public class LoggerManager
    {
        public static Logger GetLogger()
        {
            return LogManager.GetCurrentClassLogger();
        }

        public static void GenerateReport()
        {
            var logQuery = new MSUtil.LogQueryClass();
            var input = new MSUtil.COMTextLineInputContextClass();

            var result = logQuery.Execute(@"SELECT * FROM D:\MentoringGit\mentor7-16\LoggingAndMonitoring\Task\MvcMusicStore\logs\*.log", input);

            int errorCount = 0;
            int debugCount = 0;
            int infoCount = 0;
            var errors = new List<string>();

            while (!result.atEnd())
            {
                var record = result.getRecord();
                string text = record.getValue(2);

                var values = text.Split('|').ToList();
                for(var i = 0; i < values.Count; i++)
                {
                    values[i] = values[i].Trim();
                }

                if (values[1] == "INFO")
                    infoCount++;
                else if (values[1] == "DEBUG")
                    debugCount++;
                else if (values[1] == "ERROR")
                {
                    errorCount++;
                    errors.Add(text);
                }

                result.moveNext();
            }

            string errorsCountString = "Count of error logs = " + errorCount;
            string debugCountString = "Count of debug logs = " + debugCount;
            string infoCountString = "Count of info logs = " + infoCount;
            List<string> list = new List<string>();
            list.Add(errorsCountString);
            list.Add(debugCountString);
            list.Add(infoCountString);
            list.AddRange(errors);

            var path = @"C:\Report.txt";

            if (File.Exists(path))
                File.Delete(path);

            File.AppendAllLines(path, list);
        }
    }
}