using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using Newtonsoft.Json.Linq;

namespace PopupExample {
    public enum TaskStatus {
        Urgent = 0,
        Uncompleted = 1,
        Completed = 2
    }

    public class EmployeeTasksRepository {
        public IList<EmployeeTask> EmployeeTasks { get; private set; }

        public EmployeeTasksRepository() {
            IList<EmployeeTask> tasks = LoadTasks();
            UpdateSource(tasks);
            EmployeeTasks = tasks;
        }

        IList<EmployeeTask> LoadTasks() {
            System.Reflection.Assembly assembly = GetType().Assembly;
            Stream stream = assembly.GetManifestResourceStream("EmployeeTasks.json");
            JObject jObject = JObject.Parse(new StreamReader(stream).ReadToEnd());
            List<EmployeeTask> list = jObject["EmployeeTasks"].ToObject<List<EmployeeTask>>().Take(30).ToList();
            return new BindingList<EmployeeTask>(list);
        }

        void UpdateSource(IList<EmployeeTask> tasks) {
            Random random = new Random();
            for (int i = 0; i < tasks.Count; i++) {
                EmployeeTask task = tasks[i];
                task.StartDate = DateTime.Now.AddDays(random.Next(7) + 1);
                task.DueDate = task.StartDate.AddDays(random.Next(3) + 1);
                task.Status = (TaskStatus)(i < 2 ? 0 : i < tasks.Count * 2 / 3 ? 1 : 2);
            }
        }
    }

    public class EmployeeTask : INotifyPropertyChanged {
        public EmployeeTask() {
            UrgentTaskCommand = new Command(() => Status = TaskStatus.Urgent);
            CompleteTaskCommand = new Command(() => Status = TaskStatus.Completed);
            UnCompleteTaskCommand = new Command(() => Status = TaskStatus.Uncompleted);
        }

        public int Id { get; set; }
        public int ParentId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Employee { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime DueDate { get; set; }
        public int Priority { get; set; }

        public ICommand UrgentTaskCommand { get; }
        public ICommand CompleteTaskCommand { get; }
        public ICommand UnCompleteTaskCommand { get; }

        TaskStatus status;
        public TaskStatus Status {
            get => this.status;
            set {
                this.status = value;
                OnPropertyChanged();
            }
        }

        void OnPropertyChanged([CallerMemberName] string name = null) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion
    }
}

