using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildTools {
    class Program {
        static void Main(string[] args) {

            string aspApiSolutionDirectory = args[0].Trim('"');
            string rootDirectory = new DirectoryInfo(aspApiSolutionDirectory).Parent.FullName;
            string appsDestinationDirectory = Path.Combine(rootDirectory, "api\\api\\apps");
            string appsSourceDirectory = Path.Combine(rootDirectory, "ember-apps");

            if (!Directory.Exists(appsDestinationDirectory))
                Directory.CreateDirectory(appsDestinationDirectory);

            foreach (string appDirectory in Directory.GetDirectories(appsSourceDirectory)) {
                Build(appDirectory);

                string appName = Path.GetFileName(appDirectory);
                string distPath = Path.Combine(appDirectory, "dist");
                string destinationPath = Path.Combine(appsDestinationDirectory, appName);
                Empty(destinationPath);
                Copy(distPath, destinationPath);
            }

        }

        static void Build(string emberAppDirectory) {

            string defaultCurrentDirectory = Environment.CurrentDirectory;
            Environment.CurrentDirectory = emberAppDirectory;

            try {
                var process = new Process();
                process.StartInfo.FileName = "ember";
                process.StartInfo.Arguments = "build --environment=production";
                process.Start();
                process.WaitForExit();
            } finally {
                Environment.CurrentDirectory = defaultCurrentDirectory;
            }

        }

        static void Empty(string path) {

            var directoryInfo = new DirectoryInfo(path);
            if (!directoryInfo.Exists)
                return;

            foreach (var file in directoryInfo.GetFiles())
                file.Delete();

            foreach (var directory in directoryInfo.GetDirectories())
                directory.Delete(true);
        }

        static void Copy(string sourcePath, string destinationPath) {

            //Create directories
            foreach (string directoryPath in Directory.GetDirectories(sourcePath, "*", SearchOption.AllDirectories))
                Directory.CreateDirectory(directoryPath.Replace(sourcePath, destinationPath));

            //Copy all the files & replaces any files with the same name
            foreach (string filePath in Directory.GetFiles(sourcePath, "*.*", SearchOption.AllDirectories).Where(f => !f.EndsWith(".gitkeep")))
                File.Copy(filePath, filePath.Replace(sourcePath, destinationPath), true);
        }
    }
}