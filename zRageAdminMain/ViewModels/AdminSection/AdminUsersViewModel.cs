using MongoDB.Driver;
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
using static MongoDB.Bson.Serialization.BsonDeserializationContext;

namespace zRageAdminMain.ViewModels.AdminSection
{
    public class AdminUsersViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public bool EditMode { get; set; }
        public UserModel DisplayedUser { get; set; }
        public ObservableCollection<UserModel> Users { get; set; }
        public ObservableCollection<UserGroupModel> AvaliableGroups { get; set; }
        public ObservableCollection<UserGroupModel> DefinedGroups { get; set; }
        public UpdateAvaliableGroupsCommand UpdateAvaliableGroupsCommand { get; set; }
        public AddSelectedUserGroupCommand AddSelectedUserGroupCommand { get; set; }
        public RemoveSelectedUserGroupCommand RemoveSelectedUserGroupCommand { get; set; }
        public CreateNewOrEditExistingUserCommand CreateNewOrEditExistingUserCommand { get; set; }
        public CancelAndClearUserActionCommand CancelAndClearUserActionCommand { get; set; }
        public RemoveUserCommand RemoveUserCommand { get; set; }
        public LoadUserForEditionCommand LoadUserForEditionCommand { get; set; }
        public AdminUsersViewModel()
        {
            DisplayedUser = new UserModel();
            Users = new ObservableCollection<UserModel>();
            AvaliableGroups = new ObservableCollection<UserGroupModel>();
            DefinedGroups = new ObservableCollection<UserGroupModel>();

            UpdateAvaliableGroupsCommand = new UpdateAvaliableGroupsCommand(this);
            AddSelectedUserGroupCommand = new AddSelectedUserGroupCommand(this);
            RemoveSelectedUserGroupCommand = new RemoveSelectedUserGroupCommand(this);

            CreateNewOrEditExistingUserCommand = new CreateNewOrEditExistingUserCommand(this);
            CancelAndClearUserActionCommand = new CancelAndClearUserActionCommand(this);
            RemoveUserCommand = new RemoveUserCommand(this);
            LoadUserForEditionCommand = new LoadUserForEditionCommand(this);

            if (DesignerProperties.GetIsInDesignMode(new DependencyObject()))
            {
                #region Design Placeholders
                EditMode = true;

                DisplayedUser.Name = "Daniel Hidalgo";
                DisplayedUser.Login = "danielh";
                DisplayedUser.GodMode = true;
                DisplayedUser.Enabled = true;
                DisplayedUser.CreatedDate = DateTime.Now.AddHours(-96);

                Users.AddRange(new List<UserModel>
                {
                    DisplayedUser,
                    new UserModel
                    {
                        Name = "Vitor Sanchez",
                        Login = "vitors",
                        Enabled = true,
                        CreatedDate = DateTime.Now.AddHours(-46)
                    },
                    new UserModel
                    {
                        Name = "Fabio Pedro",
                        Login = "fape97",
                        Enabled = true,
                        CreatedDate = DateTime.Now.AddHours(-486)
                    },
                    new UserModel
                    {
                        Name = "Diego Grava",
                        Login = "diegogr",
                        Enabled = false,
                        CreatedDate = DateTime.Now.AddHours(-41)
                    }
                });

                AvaliableGroups.AddRange(new List<UserGroupModel>
                {
                    new UserGroupModel { GroupCode = "admin", GroupName = "Administrator" },
                    new UserGroupModel { GroupCode = "User1", GroupName = "User 1P" },
                    new UserGroupModel { GroupCode = "User3", GroupName = "Mod" },
                    new UserGroupModel { GroupCode = "User23", GroupName = "Special" },
                    new UserGroupModel { GroupCode = "User45", GroupName = "Helper" }
                });

                DefinedGroups.AddRange(new List<UserGroupModel>
                {
                    new UserGroupModel { GroupCode = "admin1", GroupName = "Administrator Junior" },
                    new UserGroupModel { GroupCode = "User67", GroupName = "User xP" }
                });
                #endregion
            }
            else
            {
                DisplayedUser.Enabled = true;
                UpdateAvaliableGroups();
                UpdateUsers();
            }
        }

        public void UpdateAvaliableGroups()
        {
            var groups = UserGroupModel.GetAll();
            AvaliableGroups.Clear();

            foreach (var group in groups)
            {
                if (!DefinedGroups.Where(x => x.GroupCode == group.GroupCode).Any())
                    AvaliableGroups.Add(group);
            }
        }

        public void UpdateUsers()
        {
            var auxausers = UserModel.GetAll();
            Users.Clear();

            foreach (var user in auxausers)
            {
                if (!Users.Contains(user))
                    Users.Add(user);
            }
        }

        public void ApplyDefinedGroups()
        {
            foreach (var group in DefinedGroups)
            {
                if (AvaliableGroups.ToList().Exists(x => x.Id == group.Id))
                    continue;

                DisplayedUser.UserGroupsIds.Add(group.Id);
            }
        }
    }
}
