using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace openfin.wpfhost
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        const string version = "stable";

        const string subscriptionName = "Oden.Security";
        const string uuid = "FXBOOKING_ODN01";
        const string topic = "Oden.Security";

        MessageChannel dataMessageChannel;

        public MainWindow()
        {
            InitializeComponent();

            //Runtime options is how we set up the OpenFin Runtime environment

            var runtimeOptions = new Openfin.Desktop.RuntimeOptions
            {
                Version = version,
                EnableRemoteDevTools = true,
                RemoteDevToolsPort = 9090
            };

            var runtime = Openfin.Desktop.Runtime.GetRuntimeInstance(runtimeOptions);

            runtime.Error += (sender, e) =>
            {
                Console.Write(e);
            };

            runtime.Connect(() =>
            {
                // Initialize the communication channel after the runtime has connected
                // but before launching any applications or EmbeddedViews
                dataMessageChannel = new MessageChannel(runtime.InterApplicationBus, uuid, topic);
            });

            //Initialize the grid view by passing the runtime Options and the ApplicationOptions
            var fileUri = new Uri(System.IO.Path.GetFullPath(@"web-content\test-ang\index.html")).ToString();
            OpenFinEmbeddedView.Initialize(runtimeOptions, new Openfin.Desktop.ApplicationOptions(subscriptionName, uuid, fileUri));

            //Once the grid is ready get the data and populate the list box.
            OpenFinEmbeddedView.Ready += (sender, e) =>
            {

                var t = new System.Threading.Thread(() =>
                {
                while (true)
                {
                    Thread.Sleep(2000);
                    var str = Guid.NewGuid();
                    dataMessageChannel.SendData(str.ToString());
                    }
                });
                t.Start();
            };
        }
    }
}
