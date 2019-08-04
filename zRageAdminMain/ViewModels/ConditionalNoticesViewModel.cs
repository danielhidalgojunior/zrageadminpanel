using MongoDB.Bson;
using MongoDBHelper;
using MongoDBHelper.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ToastNoticesManager;
using zRageAdminMain.Controls;
using zRageAdminMain.ViewModels.Commands;
using zRageAdminMain.Views;

namespace zRageAdminMain.ViewModels
{
    public class ConditionalNoticesViewModel : INotifyPropertyChanged
    {        
        public event PropertyChangedEventHandler PropertyChanged;
        public ObservableCollection<EventTriggerModel> Events { get; set; }
        public EventTriggerModel Trigger { get; set; }
        public IEnumerable<NoticeType> NoticesTypes
        {
            get => Enum.GetValues(typeof(NoticeType)).Cast<NoticeType>();
        }

        public IEnumerable<EventType> EventTypes
        {
            get => Enum.GetValues(typeof(EventType)).Cast<EventType>();
        }
        public CreateEventTriggerCommand CreateEventTriggerCommand { get; set; }
        public DeleteEventTriggerCommand DeleteEventTriggerCommand { get; set; }

        public ConditionalNoticesViewModel()
        {
            Trigger = new EventTriggerModel();
            CreateEventTriggerCommand = new CreateEventTriggerCommand(this);
            DeleteEventTriggerCommand = new DeleteEventTriggerCommand(this);
            Events = new ObservableCollection<EventTriggerModel>();

            if (DesignerProperties.GetIsInDesignMode(new DependencyObject()))
            {
                Events = new ObservableCollection<EventTriggerModel>();
                Random r = new Random();
                for (int i = 0; i < 10; i++)
                {
                    Events.Add(new EventTriggerModel
                    {
                        Notice = (NoticeType)r.Next(0, 5),
                        Event = (EventType)r.Next(0, 1),
                        Period = new DateRange
                        {
                            Start = DateTime.Now.AddDays(r.Next(-50, 20)).AddHours(r.Next(-20, 20)).AddMinutes(r.Next(-20, 20)),
                            End = DateTime.Now.AddDays(r.Next(-21, 40)).AddHours(r.Next(-20, 20)).AddMinutes(r.Next(-20, 20))
                        }
                    });
                }
            }
            else
            {
                UpdateEvents();
            }
        }

        public void UpdateEvents()
        {
            var events2 = EventTriggerModel.GetAll().OrderBy(x => x.Period.Expired);
            var events = EventTriggerModel.GetAll().Where(x => x.Enabled).OrderBy(x => x.Period.Expired);

            Events.Clear();

            foreach (var e in events)
                Events.Add(e);
        }

        public void CreateEventTrigger()
        {
            Trigger.Id = ObjectId.GenerateNewId();
            Trigger.CreatedDate = DateTime.Now;
            Trigger.Enabled = true;
            EventTriggerModel.InsertOne(Trigger);

            Trigger.Value = null;

            UpdateEvents();
        }
    }
}
