using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevExpress.Maui.CollectionView;
using DevExpress.Maui.Editors;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Essentials;

namespace PopupExample {
    public partial class MainPage : ContentPage {
        bool isAnimated;

        ViewModel VM { get; }

        public MainPage() {
            InitializeComponent();
            BindingContext = VM = new ViewModel(new EmployeeTasksRepository());
        }

        void OnTap(object sender, CollectionViewGestureEventArgs e) {
            VM.PreparePopupAndOpen(e.Item as EmployeeTask, e.ItemHandle);
        }

        void DismissPopup(object sender, EventArgs e) {
            VM.IsOpenPopup = false;
        }

        void PinClick(object sender, EventArgs e) {
            VM.ActiveItem.Status = TaskStatus.Urgent;
            OnStatusChanged();
        }

        void DoneClick(object sender, EventArgs e) {
            VM.ActiveItem.Status = TaskStatus.Completed;
            OnStatusChanged();
        }

        void ToDoClick(object sender, EventArgs e) {
            VM.ActiveItem.Status = TaskStatus.Uncompleted;
            OnStatusChanged();
        }

        void DeleteClick(object sender, EventArgs e) {
            VM.IsOpenPopup = false;
            this.collectionView.DeleteItem(VM.ItemHandle);
        }

        void OnStatusChanged() {
            if (this.isAnimated) return;

            VM.IsOpenPopup = false;

            IList<EmployeeTask> source = VM.ItemSource;
            int newItemHandle = 0;
            switch (VM.ActiveItem.Status) {
                case TaskStatus.Urgent:
                    newItemHandle = 0;
                    break;
                case TaskStatus.Completed:
                    newItemHandle = source.Count() - 1;
                    break;
                case TaskStatus.Uncompleted:
                    newItemHandle = source.Where(t => t.Status == TaskStatus.Urgent).Count();
                    break;
            }

            if (VM.ItemHandle == newItemHandle)
                return;

            this.isAnimated = true;
            Device.BeginInvokeOnMainThread(() =>
                this.collectionView.MoveItem(VM.ItemHandle, newItemHandle, () => this.isAnimated = false)
            );
        }
    }

    class ItemDataTemplateSelector : DataTemplateSelector {
        protected override DataTemplate OnSelectTemplate(object item, BindableObject container) {
            if (!(item is EmployeeTask task))
                return null;

            switch (task.Status) {
                case TaskStatus.Urgent:
                    return UrgentDataTemplate;
                case TaskStatus.Completed:
                    return CompletedDataTemplate;
                case TaskStatus.Uncompleted:
                default:
                    return UncompletedDataTemplate;
            }
        }

        public DataTemplate UrgentDataTemplate { get; set; }
        public DataTemplate CompletedDataTemplate { get; set; }
        public DataTemplate UncompletedDataTemplate { get; set; }
    }
}

