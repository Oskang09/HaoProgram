using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HaoProgram
{
    public abstract class IJsonObject<T>
    {
        public IJsonObject(string data)
        {
            DATA = data;
        }

        public static string DATA;

        public List<string> GetListString()
        {
            return Directory.GetFiles(DATA)
                .Select(Path.GetFileNameWithoutExtension)
                .ToList();
        }
        public List<T> GetList()
        {
            List<T> objs = new List<T>();
            foreach (var file in Directory.GetFiles(DATA))
            {
                objs.Add(JsonConvert.DeserializeObject<T>(File.ReadAllText(file)));
            }
            return objs;
        }

        public T LoadJson(string path)
        {
            return JsonConvert.DeserializeObject<T>(File.ReadAllText(DATA + "/" + path + ".json"));
        }
        public void DeleteJson(string path)
        {
            File.Delete(DATA + "/" + path + ".json");
        }
        public void SaveJson(string path)
        {
            try
            {
                using (StreamWriter sw = File.CreateText(DATA + "/" + path + ".json"))
                {
                    JsonSerializer json = new JsonSerializer();
                    json.Serialize(sw, this);
                }
            }
            catch
            {
                MessageBox.Show($"Filename {path} not supported!", "FilenameNotSupportedException");
            }
        }

        public bool Exists(string verify)
        {
            return File.Exists(DATA + "/" + verify + ".json") ? true : false;
        }
    }
}
