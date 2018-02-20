using System;
using System.Configuration;
using System.Xml;

namespace Core.Configuration
{
    /// <summary>
    /// Represents a Config
    /// </summary>
    public partial class MyConfig : IConfigurationSectionHandler
    {
        /// <summary>
        /// Creates a configuration section handler.
        /// </summary>
        /// <param name="parent">Parent object.</param>
        /// <param name="configContext">Configuration context object.</param>
        /// <param name="section">Section XML node.</param>
        /// <returns>The created section handler object.</returns>
        public object Create(object parent, object configContext, XmlNode section)
        {
            var config = new MyConfig();

            var startupNode = section.SelectSingleNode("Startup");
            config.IgnoreStartupTasks = GetBool(startupNode, "IgnoreStartupTasks");

            var redisCachingNode = section.SelectSingleNode("RedisCaching");
            config.RedisCachingEnabled = GetBool(redisCachingNode, "Enabled");
            config.RedisCachingConnectionString = GetString(redisCachingNode, "ConnectionString");

            var dbConnection = section.SelectSingleNode("DBConnection");
            config.DataConnectionString = GetString(dbConnection, "ConnectionString");

            var uploadPath = section.SelectSingleNode("UploadPath");
            config.UploadPath = GetString(uploadPath, "Path");

            var PriviewTimeOut = section.SelectSingleNode("PriviewTimeOut");
            config.PriviewTimeOut = Convert.ToInt32(GetString(PriviewTimeOut,"value"));

            var remoteStorageNode = section.SelectSingleNode("RemoteStorage");
            RemoteStorageConfigure = new RemoteStorageConfigure()
            {
                Website = GetString(remoteStorageNode, "Website"),
                Downwebsite = GetString(remoteStorageNode, "Downwebsite"),
                Bucket = GetString(remoteStorageNode, "Bucket"),
                Appkey = GetString(remoteStorageNode, "Appkey"),
                Appid = GetString(remoteStorageNode, "Appid")
            };

            return config;
        }

        private string GetString(XmlNode node, string attrName)
        {
            return SetByXElement<string>(node, attrName, Convert.ToString);
        }

        private bool GetBool(XmlNode node, string attrName)
        {
            return SetByXElement<bool>(node, attrName, Convert.ToBoolean);
        }

        private T SetByXElement<T>(XmlNode node, string attrName, Func<string, T> converter)
        {
            if (node == null || node.Attributes == null) return default(T);
            var attr = node.Attributes[attrName];
            if (attr == null) return default(T);
            var attrVal = attr.Value;
            return converter(attrVal);
        }

        /// <summary>
        /// 忽略开始时初始化任务
        /// </summary>
        public bool IgnoreStartupTasks { get; set; }

        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        public string DataConnectionString { get; private set; }

        /// <summary>
        /// Indicates whether we should use Redis server for caching (instead of default in-memory caching)
        /// </summary>
        public bool RedisCachingEnabled { get; private set; }

        /// <summary>
        /// Redis connection string. Used when Redis caching is enabled
        /// </summary>
        public string RedisCachingConnectionString { get; private set; }

        /// <summary>
        /// 上传文件目录
        /// </summary>
        public string UploadPath { get; private set; }
        
        /// <summary>
        /// 预览超时时间
        /// </summary>
        public int PriviewTimeOut { get; private set; }

        public RemoteStorageConfigure RemoteStorageConfigure { get; private set; }
    }

    public class RemoteStorageConfigure
    {
        [ConfigurationProperty("Website", IsRequired = true)]
        public string Website { get; set; }
        [ConfigurationProperty("Downwebsite", IsRequired = true)]
        public string Downwebsite { get; set; }
        [ConfigurationProperty("Bucket", IsRequired = true)]
        public string Bucket { get; set; }
        [ConfigurationProperty("Appkey", IsRequired = true)]
        public string Appkey { get; set; }
        [ConfigurationProperty("Appid", IsRequired = true)]
        public string Appid { get; set; }
    }
}
