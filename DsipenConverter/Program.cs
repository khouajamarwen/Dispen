using DsipenConverter;
using System.Xml;

//string tab = "	";
//string fileContent = File.ReadAllText("text.txt");
//List<string> ls= fileContent.Split(tab).ToList();

XmlDocument doc = new XmlDocument();
doc.Load("config.xml");
string InDirectory = doc.DocumentElement.SelectSingleNode("/Config/InputDirectory").InnerText;

using var watcher = new FileSystemWatcher(InDirectory);

watcher.NotifyFilter = NotifyFilters.Attributes
                        | NotifyFilters.CreationTime
                        | NotifyFilters.DirectoryName
                        | NotifyFilters.FileName
                        | NotifyFilters.LastAccess
                        | NotifyFilters.LastWrite
                        | NotifyFilters.Security
                        | NotifyFilters.Size;

watcher.Created += FileWatcher.OnCreated;
watcher.Error += FileWatcher.OnError;
//watcher.Changed += FileWatcher.OnCreated;

//watcher.Filter = "*.txt";
watcher.IncludeSubdirectories = true;
watcher.EnableRaisingEvents = true;

//Console.ReadLine();
 ManualResetEvent _quitEvent = new ManualResetEvent(false);

Console.CancelKeyPress += (sender, eArgs) => {
    _quitEvent.Set();
    eArgs.Cancel = true;
};

// kick off asynchronous stuff 

_quitEvent.WaitOne();
//new ManualResetEvent(false).WaitOne();