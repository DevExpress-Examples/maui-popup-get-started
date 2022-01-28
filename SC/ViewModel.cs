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
    public class ViewModel : INotifyPropertyChanged {
        readonly EmployeeTasksRepository repository;

        public int ItemHandle { get; private set; }
        public EmployeeTask ActiveItem { get; private set; }

        string popupTitle;
        public string PopupTitle {
            get => this.popupTitle;
            set {
                this.popupTitle = value;
                OnPropertyChanged();
            }
        }

        bool buttonPinVisible = false;
        public bool ButtonPinVisible {
            get => this.buttonPinVisible;
            set {
                this.buttonPinVisible = value;
                OnPropertyChanged();
            }
        }

        bool buttonDoneVisible = false;
        public bool ButtonDoneVisible {
            get => this.buttonDoneVisible;
            set {
                this.buttonDoneVisible = value;
                OnPropertyChanged();
            }
        }

        bool buttonToDoVisible = false;
        public bool ButtonToDoVisible {
            get => this.buttonToDoVisible;
            set {
                this.buttonToDoVisible = value;
                OnPropertyChanged();
            }
        }

        bool isOpenPopup;
        public bool IsOpenPopup {
            get => this.isOpenPopup;
            set {
                this.isOpenPopup = value;
                OnPropertyChanged();
            }
        }

        public IList<EmployeeTask> ItemSource => this.repository.EmployeeTasks;

        public ViewModel(EmployeeTasksRepository repository) {
            this.repository = repository;
        }

        public void PreparePopupAndOpen(EmployeeTask item, int handle) {
            ActiveItem = item;
            ItemHandle = handle;

            PopupTitle = item.Name;
            ButtonPinVisible = item.Status == TaskStatus.Uncompleted;
            ButtonDoneVisible = item.Status == TaskStatus.Urgent || item.Status == TaskStatus.Uncompleted;
            ButtonToDoVisible = item.Status == TaskStatus.Completed;

            IsOpenPopup = true;
        }

        void OnPropertyChanged([CallerMemberName] string name = null) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion
    }

    
}

