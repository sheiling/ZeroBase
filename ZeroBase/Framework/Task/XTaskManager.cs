using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace ZeroBase
{
    public sealed class XTaskManager : XObject
    {
        private Dictionary<int, XTask> tasks = new Dictionary<int, XTask>();

        private readonly static XTaskManager instance = new XTaskManager();
        XTaskManager()
        {
        }

        public static XTaskManager Instance
        {
            get { return instance; }
        }

        public Dictionary<int, XTask> Tasks
        {
            get { return tasks; }
        }

        public void BindTask(int taskId, XTask task, string name)
        {
            if (tasks.ContainsKey(taskId) == false)
            {
                task.TaskId = taskId;
                task.Name = name;
                tasks.Add(taskId, task);
            }
        }

        public XTask FindTaskById(int taskId)
        {
            if (tasks.ContainsKey(taskId) == false)
            {
                return null;
            }
            return tasks[taskId];
        }
        public int FindTakdByTask(XTask task)
        {
            foreach (var ts in tasks)
            {
                if (ts.Value == task)
                {
                    return ts.Key;
                }
            }
            return 999;
        }
    }
}
