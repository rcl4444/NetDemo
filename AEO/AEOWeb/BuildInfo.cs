using System;
using System.IO;
using System.Reflection;
namespace AEOWeb
{
    public class BuildInfo
    {
        static BuildInfo INSTANCE;

        public string text
        {
            get;
            set;
        }

        public BuildInfo()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "AEOWeb.resource.build.txt";

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                text = reader.ReadToEnd();
            }
        }

        public static string getBuildNumber()
        {
            if (INSTANCE == null)
            {
                INSTANCE = new BuildInfo();
            }
            return INSTANCE.text;
        }
    }
}