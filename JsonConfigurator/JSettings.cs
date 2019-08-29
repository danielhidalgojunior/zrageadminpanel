using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace JsonConfigurator
{
    public class JSettings
    {
        private string jsonFile;
        public JSettings(string file = @"config\settings.json")
        {
            if (File.Exists(file))
            {
                jsonFile = file;
            }
            else
                MessageBox.Show("Missing configuration file.\npath: config\\settings.json");
        }

        public T Load<T>(string file = @"config\settings.json")
        {
            try
            {
                var json = File.ReadAllText(file);
                var obj = JsonConvert.DeserializeObject<T>(json);

                if (obj != null)
                {
                    Console.WriteLine("configs loaded" + "\n" + obj.ToString());
                }

                return obj;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Corrupted configuration file." + ex);
                return default;
            }
        }

        public void Save<T>(T settings)
        {
            if (jsonFile == null)
                throw new Exception("Missing json file path");

            try
            {
                var json = JsonConvert.SerializeObject(settings, Formatting.Indented);
                File.WriteAllText(jsonFile, json);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error when trying to save settings." + "\n" + ex);
            }
        }
    }
}
