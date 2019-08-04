using MongoDBHelper.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using zRageAdminMain.ViewModels.Commands.AdminSection;

namespace zRageAdminMain.ViewModels.AdminSection
{
    public class AdminPermissionGroupsViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public ObservableCollection<UserGroupModel> Groups { get; set; }
        public UserGroupModel DisplayedGroup { get; set; }
        public string CommandsStr { get; set; }
        public bool EditMode { get; set; }

        public CreateNewOrEditExistingGroupCommand CreateNewOrEditExistingGroupCommand { get; set; }
        public CancelAndClearGroupActionCommand CancelAndClearGroupActionCommand { get; set; }
        public RemoveGroupCommand RemoveGroupCommand { get; set; }
        public UpdateAvaliableGroupsCommand UpdateAvaliableGroupsCommand { get; set; }
        public LoadGroupForEditionCommand LoadGroupForEditionCommand { get; set; }

        public AdminPermissionGroupsViewModel()
        {
            CreateNewOrEditExistingGroupCommand = new CreateNewOrEditExistingGroupCommand(this);
            CancelAndClearGroupActionCommand = new CancelAndClearGroupActionCommand(this);
            RemoveGroupCommand = new RemoveGroupCommand(this);
            UpdateAvaliableGroupsCommand = new UpdateAvaliableGroupsCommand(this);
            LoadGroupForEditionCommand = new LoadGroupForEditionCommand(this);

            DisplayedGroup = new UserGroupModel();
            Groups = new ObservableCollection<UserGroupModel>();

            if (DesignerProperties.GetIsInDesignMode(new DependencyObject()))
            {
                #region Design Placeholders

                CommandsStr = "sm_ban\nsm_kick\nsm_mute";
                EditMode = true;

                Groups.AddRange(new List<UserGroupModel>
                {
                    new UserGroupModel
                    {
                        CreatedDate = DateTime.Now,
                        GroupCode = "Admin 2",
                        GroupName = "Moderator",
                        PermissionRank = 9
                    },
                    new UserGroupModel
                    {
                        CreatedDate = DateTime.Now,
                        GroupCode = "Admin 3",
                        GroupName = "Helper",
                        PermissionRank = 7
                    },
                    new UserGroupModel
                    {
                        CreatedDate = DateTime.Now,
                        GroupCode = "Admin 5",
                        GroupName = "Rookie",
                        PermissionRank = 5
                    }
                });

                DisplayedGroup.CreatedDate = DateTime.Now;
                DisplayedGroup.GroupCode = "admin";
                DisplayedGroup.GroupName = "Administrator 1";
                DisplayedGroup.PermissionRank = 10;
                DisplayedGroup.AvaliableCommands = new List<string>
                {
                    "sm_ban",
                    "sm_kick",
                    "sm_mute"
                };
                #endregion
            }
            else
            {
                UpdateAvaliableGroups();
            }
        }

        public void ApplyDefinedCommands()
        {
            var list = CommandsStr.Replace("\r", "").Split('\n').ToList();

            DisplayedGroup.AvaliableCommands = list.Where(x => x.Count() > 2).ToList();
        }

        public void UpdateAvaliableGroups()
        {
            var auxgroups = UserGroupModel.GetAll();
            Groups.Clear();

            foreach (var user in auxgroups)
            {
                if (!Groups.Contains(user))
                    Groups.Add(user);
            }
        }
    }
}
