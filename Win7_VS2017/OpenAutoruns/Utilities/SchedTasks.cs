using System;
using System.Collections.ObjectModel;
using System.Xml;

// `Interop.TaskScheduler.dll`
using TaskScheduler;

namespace OpenAutoruns.Utilities
{
    /// <summary>
    /// Autorun based on Scheduled Tasks
    /// </summary>
    class SchedTask : Base
    {
        // Search Scheduled Tasks
        public static void SearchSchedTasks(ref ObservableCollection<SchedTask> schedTasks)
        {
            var taskScheduler = new TaskSchedulerClass();
            taskScheduler.Connect(null, null, null, null);
            ITaskFolder folder = taskScheduler.GetFolder("\\");
            IRegisteredTaskCollection tasks = folder.GetTasks(1);

            foreach (IRegisteredTask task in tasks)
            {
                string imagePath = "";
                var xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(task.Xml);
                foreach (XmlNode xmlNode in xmlDoc.DocumentElement.ChildNodes)
                {
                    if (xmlNode.Name == "Actions")
                    {
                        imagePath = xmlNode.ChildNodes[0].InnerText;
                        imagePath = Filter(imagePath);
                        Console.WriteLine(imagePath);
                        break;
                    }
                }
                if (imagePath == "") continue;

                var schedTask = new SchedTask
                {
                    Entry = task.Name,
                    Description = Tool.GetDescription(imagePath),
                    Publisher = Tool.GetPublisher(imagePath),
                    ImagePath = imagePath,
                    TimeStamp = Tool.GetTimeStamp(imagePath),
                };

                schedTasks.Add(schedTask);
            }
        }

        public static string Filter(string imagePath)
        {
            // to lower case
            imagePath = imagePath.ToLower();

            // remove double quotes
            if (imagePath.StartsWith("\""))
            {
                imagePath = imagePath.Substring(1, imagePath.IndexOf('\"', 1) - 1);
            }

            // remove trailing parameters
            if (imagePath.Contains("exe"))
            {
                imagePath = imagePath.Substring(0, imagePath.IndexOf(".exe") + 4);
            }

            // begin with `\??\`
            if (imagePath.StartsWith(@"\??\"))
            {
                imagePath = imagePath.Substring(4);
            }

            // begin with `\`
            if (imagePath.StartsWith(@"\"))
            {
                imagePath = imagePath.Substring(1);
            }

            return imagePath;
        }
    }
}