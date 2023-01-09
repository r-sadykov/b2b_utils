using System;
using System.Collections.Generic;
using Microsoft.Win32;
using B2B_Utils.Model;
using System.IO;

namespace B2B_Utils.Model
{
    public class SearchStatModel
    {
        private readonly Dictionary<string, OpenFileDialog> _ofd;
        private readonly List<SearchStat> SearchStatList;
        public List<string> TotalGds;

        public SearchStatModel(Dictionary<string, OpenFileDialog> _ofd)
        {
            this._ofd = _ofd;
            SearchStatList = new List<SearchStat>();
            TotalGds= new List<string>();
        }

        public List<SearchStat> GetStat()
        {
            foreach (KeyValuePair<string, OpenFileDialog> entry in _ofd)
            {
                List<string> content = new List<string>();
                using (StreamReader fsReader = new StreamReader(entry.Value.FileName))
                {
                    while (!fsReader.EndOfStream)
                    {
                        content.Add(fsReader.ReadLine());
                    }
                    fsReader.Close();
                }
                foreach (string con in content)
                {
                    string[] arr = con.Split(new char[] { ';' });
                    SearchStat stat = new SearchStat();
                    string temp = arr[0].Substring(0, con.IndexOf('T'));
                    string temp2 = arr[0].Substring(con.IndexOf('T') + 1);
                    stat.Date = temp;
                    temp = temp2.Substring(0, temp2.IndexOf('Z'));
                    temp2 = temp2.Substring(temp2.IndexOf('Z') + 1);
                    stat.Time = temp;
                    temp = temp2.Trim(new char[] { '[', ']' });
                    stat.Gmt = temp;
                    stat.Server = entry.Key;
                    stat.ServerId = arr[1];
                    stat.Tenant = arr[2];
                    stat.AgencyNumber = arr[3];
                    stat.SP = arr[4];
                    stat.Agent = arr[5];
                    stat.Routes = arr[6];
                    stat.Pax = arr[7];
                    stat.Class = arr[8];
                    stat.TotalFlightsCount = arr[9];
                    stat.TotalFlightsAfterFilter = arr[10];
                    stat.TotalRespTime = arr[11];
                    stat.TotalPreProcessingTime = arr[12];
                    stat.TotalSearchTime = arr[13];
                    stat.TotalPostProcessingTime = arr[14];
                    for(int i = 15; i < arr.Length; i += 6)
                    {
                        SearchStatGds gds = new SearchStatGds() {
                            Gds = arr[i]
                        };
                        if (!TotalGds.Contains(gds.Gds)) TotalGds.Add(gds.Gds);
                        if (gds.Gds.Equals("null") || gds.Gds.Equals(""))
                        {
                            gds.FlightCount = "null";
                            gds.RespTime = "null";
                            gds.BlackWhiteListHit = "null";
                            gds.Error = "null";
                            gds.CacheHit = "null";
                        }
                        else
                        {
                            gds.FlightCount = arr[i + 1];
                            gds.RespTime = arr[i + 2];
                            gds.BlackWhiteListHit = arr[i + 3];
                            gds.Error = arr[i + 4];
                            gds.CacheHit = arr[i + 5];
                        }
                        stat.GdsList.Add(gds);
                    }
                    SearchStatList.Add(stat);
                }
            }
            TotalGds.Sort();
            return SearchStatList;
        }
    }
}