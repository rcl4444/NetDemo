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

            var email = section.SelectSingleNode("Email");
            config.email_from = GetString(email, "from");
            config.email_to = GetString(email, "to");
            config.email_host = GetString(email, "host");
            config.email_port = GetString(email, "port");
            config.email_account = GetString(email, "account");
            config.email_pwd = GetString(email, "pwd");
            var PriviewTimeOut = section.SelectSingleNode("PriviewTimeOut");
            config.PriviewTimeOut = Convert.ToInt32(GetString(PriviewTimeOut,"value"));

            var oss = section.SelectSingleNode("OSSsetting");
            config.website = GetString(oss, "website");
            config.downwebsite = GetString(oss, "downwebsite");
            config.bucket = GetString(oss, "bucket");
            config.appkey = GetString(oss, "appkey");
            config.appid = GetString(oss, "appid");

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
        /// ���Կ�ʼʱ��ʼ������
        /// </summary>
        public bool IgnoreStartupTasks { get; set; }

        /// <summary>
        /// ���ݿ������ַ���
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
        /// �ϴ��ļ�Ŀ¼
        /// </summary>
        public string UploadPath { get; private set; }

        /// <summary>
        /// �ʼ�������
        /// </summary>
        public string email_from { get; private set; }

        /// <summary>
        /// �ʼ�������
        /// </summary>
        public string email_to { get; private set; }

        /// <summary>
        /// �ʼ������ͷ�����
        /// </summary>
        public string email_host { get; private set; }

        /// <summary>
        /// �ʼ������Ͷ˿�
        /// </summary>
        public string email_port { get; private set; }

        /// <summary>
        /// �ʼ��������˺�
        /// </summary>
        public string email_account { get; private set; }

        /// <summary>
        /// �ʼ�����������
        /// </summary>
        public string email_pwd { get; private set; }
        
        /// <summary>
        /// Ԥ����ʱʱ��
        /// </summary>
        public int PriviewTimeOut { get; private set; }

        /// <summary>
        /// �ϴ�վ��
        /// </summary>
        public string website { get; private set; }

        /// <summary>
        /// ����վ��
        /// </summary>
        public string downwebsite { get; private set; }

        /// <summary>
        /// OSSĿ¼
        /// </summary>
        public string bucket { get; private set; }

        /// <summary>
        /// OSS����Կ
        /// </summary>
        public string appkey { get; private set; }

        /// <summary>
        /// OSS��appId
        /// </summary>
        public string appid { get; private set; }
    }
}
