using System.IO;

namespace Entitas
{

    public static class EntitasResources
    {

        public static string GetVersion()
        {
            var assembly = typeof(Entity).Assembly;
            var stream = assembly.GetManifestResourceStream("version.txt");

            if(null == stream)
            {
                stream = assembly.GetManifestResourceStream("Entitas.version.txt");
            }

            if(null == stream)
            {
                stream = assembly.GetManifestResourceStream("Assembly-CSharp.Entitas.version.txt");
            }

            //string version;
            //using (var reader = new StreamReader(stream))
            //{
            //    version = reader.ReadToEnd();
            //}

            //return version;
            return "1.5.2";
        }
    }
}
