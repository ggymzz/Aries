﻿using Aries.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace Aries.Lib
{
    public static class ServerConfigExtention
    {
        public static string ToJsonString(this ServerConfig sc)
        {
            return JsonHelper.SerializeObject(sc);
        }
    }
    public static class ServiceConfigService
    {

        public static WarpMessage WarpMessage;
        public static readonly string ARIESDIR = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\Aries\";
        public static readonly string FILE = ARIESDIR+ @"Aries.json";
        static string LoadFile()
        {
            try
            {
                using (var reader = new StreamReader(FILE))
                {
                    return reader.ReadToEnd();
                }
            }
            catch
            {

                return LoadDefault();
            }

        }

        static string LoadDefault()
        {

            return JsonHelper.SerializeObject(new
            {
                configs = new ServerConfig[] {
                new ServerConfig {
                    ID=1,
                    ServerName="Kevin's Server",
                    Host="kevinconan.vicp.cc"
                },
                new ServerConfig {
                    ID=2,
                    ServerName="创意冒险岛",
                    Host="117.25.135.249",
                    LoginPort = 8585,
                    ChannelStartPort = 7675,
                    ChannelEndPort = 7679
                }
            },
                lastId = 0
            });
        }

        private static Dictionary<int, ServerConfig> serverConfigs;
        public static int LastId { get; set; }

        public static Dictionary<int, ServerConfig> LoadAll()
        {
            if (serverConfigs == null)
            {
                var config = new { lastId = 0, configs = new ServerConfig[0] };
                config = JsonHelper.DeserializeAnonymousType(LoadFile(), config);
                LastId = config.lastId;

                var q = from c in config.configs
                        select c;
                serverConfigs = q.ToDictionary(sc => sc.ID);
            }

            return serverConfigs;
        }

        public static void SaveAll()
        {
            if (!Directory.Exists(ARIESDIR))
            {
                Directory.CreateDirectory(ARIESDIR);
            }

            using (var writer = new StreamWriter(FILE))
            {
                writer.Write(JsonHelper.SerializeObject(new { configs = serverConfigs.Values, lastId = LastId }));
            }
       
        }

    }
}
