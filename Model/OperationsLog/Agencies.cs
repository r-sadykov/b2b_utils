using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace B2B_Utils.Model.OperationsLog
{
    internal class Agencies
    {
        public string AGENCY_CONFIG_FILE=Environment.CurrentDirectory+"\\AppData\\Configuration\\Agencies.json";
        public List<Agent> AgenciesList { get; }
        public List<IgnoreWord> IgnoreWordList { get; }
        public List<DatabaseConfiguration> DBConfiguration { get; }

        public Agencies()
        {
            AgenciesList = new List<Agent>();
            IgnoreWordList = new List<IgnoreWord>();
            DBConfiguration = new List<DatabaseConfiguration>();
            string conf = GetJsonContent();
            GetAgencies(conf);
            GetIgnoreWords(conf);
            GetDatabaseConfiguration(conf);
        }

        private string GetJsonContent()
        {
            string json;
            using (StreamReader streamReader = new StreamReader(AGENCY_CONFIG_FILE)) {
                json = streamReader.ReadToEnd();
            }
            return json;
        }

        private bool GetAgencies(string conf)
        {
            dynamic agents = JsonConvert.DeserializeObject<dynamic>(conf);
            foreach(var item in agents.Agencies) {
                Agent agent = new Agent() {
                    Name = item.Name,
                    Code = item.Code
                };
                AgenciesList.Add(agent);
            }
            if (AgenciesList.Count > 0) return true;
            else return false;
        }

        private bool GetIgnoreWords(string conf)
        {
            dynamic words = JsonConvert.DeserializeObject<dynamic>(conf);
            foreach (var item in words.IgnoreWords) {
                IgnoreWord word = new IgnoreWord() {
                    Word = item.Word
                };
                IgnoreWordList.Add(word);
            }
            if (IgnoreWordList.Count > 0) return true;
            else return false;
        }

        private bool GetDatabaseConfiguration(string conf)
        {
            dynamic words = JsonConvert.DeserializeObject<dynamic>(conf);
            foreach (var item in words.Databases) {
                DatabaseConfiguration dbConfig = new DatabaseConfiguration() {
                    DatabaseName = item.DatabaseName,
                    DatabaseSource = item.DatabaseSource,
                    DatabaseFile = item.DatabaseFile,
                    DatabaseSecurity = item.Security,
                    DatabaseProvider=item.Provider
                };
                DBConfiguration.Add(dbConfig);
            }
            if (DBConfiguration.Count > 0) return true;
            else return false;
        }

        internal class Agent
        {
            [JsonProperty("Name")]
            public string Name { get; set; }
            [JsonProperty("Code")]
            public string Code { get; set; }
        }

        internal class IgnoreWord
        {
            [JsonProperty("Word")]
            public string Word { get; set; }
        }

        internal class DatabaseConfiguration
        {
            [JsonProperty("DatabaseName")]
            public string DatabaseName { get; set; }
            [JsonProperty("DatabaseSource")]
            public string DatabaseSource { get; set; }
            [JsonProperty("DatabaseFile")]
            public string DatabaseFile { get; set; }
            [JsonProperty("Security")]
            public string DatabaseSecurity { get; set; }
            [JsonProperty("Provider")]
            public string DatabaseProvider { get; set; }
        }
    }
}
