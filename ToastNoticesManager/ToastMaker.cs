using DesktopToast;
using MongoDBHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ToastNoticesManager
{
    public static class ToastMaker
    {
        public static void SendToastAsync(string body, NoticeType type)
        {
            var audio = GetAudioFromType(type);
            var title = GetTitleFromType(type);

            var request = new ToastRequest
            {
                ToastAudio = audio,
                ToastTitle = title,
                ToastBody = body,
                ShortcutFileName = "zRageAdminMain.lnk",
                ShortcutTargetFilePath = Assembly.GetExecutingAssembly().Location,
                AppId = "zRageAdminMain",
            };

            ToastManager.ShowAsync(request).RunSynchronously();
        }

        private static ToastAudio GetAudioFromType(NoticeType type)
        {
            ToastAudio audio;
            switch (type)
            {
                case NoticeType.PlayerConnected:
                    {
                        audio = ToastAudio.Default;
                        break;
                    }
                case NoticeType.NextMapSelected:
                    {
                        audio = ToastAudio.Silent;
                        break;
                    }
                case NoticeType.MapChanged:
                    {
                        audio = ToastAudio.Silent;
                        break;
                    }
                case NoticeType.OnlinePlayersValueReached:
                    {
                        audio = ToastAudio.Reminder;
                        break;
                    }
                case NoticeType.ServerNotResponding:
                    {
                        audio = ToastAudio.LoopingAlarm;
                        break;
                    }
                case NoticeType.LevelOfMapReached:
                    {
                        audio = ToastAudio.Mail;
                        break;
                    }
                default:
                    {
                        audio = ToastAudio.Default;
                        break;
                    }
            }

            return audio;
        }

        private static string GetTitleFromType(NoticeType type)
        {
            string title;
            switch (type)
            {
                case NoticeType.PlayerConnected:
                    {
                        title = "Player connected!";
                        break;
                    }
                case NoticeType.NextMapSelected:
                    {
                        title = "Next map selected!";
                        break;
                    }
                case NoticeType.MapChanged:
                    {
                        title = "Map has changed";
                        break;
                    }
                case NoticeType.OnlinePlayersValueReached:
                    {
                        title = "Number of online players reached";
                        break;
                    }
                case NoticeType.ServerNotResponding:
                    {
                        title = "Server not responding";
                        break;
                    }
                case NoticeType.LevelOfMapReached:
                    {
                        title = "Map level reached";
                        break;
                    }
                default:
                    {
                        title = "Not handled notice";
                        break;
                    }
            }

            return title;
        }
    }
}
