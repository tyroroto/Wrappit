using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;
using CefSharp;
using CefSharp.WinForms;

namespace WrapitApplication
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            InitializeCef();
        }

        void InitializeCef()
        {
            var settings = new CefSettings();
            settings.RegisterScheme(new CefCustomScheme
            {
                SchemeName = CefSharpSchemeHandlerFactory.SchemeName,
                SchemeHandlerFactory = new CefSharpSchemeHandlerFactory()
            });
            Cef.Initialize(settings);

            var browser = new ChromiumWebBrowser("wrapper://app/bomber/index.html")
            {
                Dock = DockStyle.Fill
            };
            browser.ConsoleMessage += BrowserOnConsoleMessage;
            appPanel.Controls.Add(browser);

        }

        private void BrowserOnConsoleMessage(object sender, ConsoleMessageEventArgs consoleMessageEventArgs)
        {
            Console.WriteLine("Browser Log : "+consoleMessageEventArgs.Message);
        }
    }



    public class CefSharpSchemeHandler : ResourceHandler
    {
        public override bool ProcessRequestAsync(IRequest request, ICallback callback)
        {
            var uri = new Uri(request.Url);
            Console.WriteLine("Direct to "+ request.Url);
            var fileName = request.Url.Substring(request.Url.IndexOf("://") + 2);
            Task.Run(() =>
            {
                using (callback)
                {
                    Stream stream = null;

                    //if (string.Equals(fileName, "/PostDataTest.html", StringComparison.OrdinalIgnoreCase))
                    //{
                    //    var postDataElement = request.PostData.Elements.FirstOrDefault();
                    //    stream = ResourceHandler.GetMemoryStream("Post Data: " + (postDataElement == null ? "null" : postDataElement.GetBody()), Encoding.UTF8);
                    //}

                    //if (string.Equals(fileName, "/PostDataAjaxTest.html", StringComparison.OrdinalIgnoreCase))
                    //{
                    //    var postData = request.PostData;
                    //    if (postData == null)
                    //    {
                    //        stream = ResourceHandler.GetMemoryStream("Post Data: null", Encoding.UTF8);
                    //    }
                    //    else
                    //    {
                    //        var postDataElement = postData.Elements.FirstOrDefault();
                    //        stream = ResourceHandler.GetMemoryStream("Post Data: " + (postDataElement == null ? "null" : postDataElement.GetBody()), Encoding.UTF8);
                    //    }
                    //}

                    if (File.Exists(Application.StartupPath + fileName))
                    {

                        stream = new FileStream(Application.StartupPath + fileName, FileMode.Open);
                        var fileExtension = Path.GetExtension(fileName);
                        MimeType = ResourceHandler.GetMimeType(fileExtension);

                    }

                    if (stream == null)
                    {
                        callback.Cancel();
                    }
                    else
                    {
                        //Reset the stream position to 0 so the stream can be copied into the underlying unmanaged buffer
                        stream.Position = 0;
                        //Populate the response values - No longer need to implement GetResponseHeaders (unless you need to perform a redirect)
                        ResponseLength = stream.Length;
                        StatusCode = (int)HttpStatusCode.OK;
                        Stream = stream;
                        callback.Continue();
                    }
                }
            });

            return true;
            
        }
    }

    public class CefSharpSchemeHandlerFactory : ISchemeHandlerFactory
    {
        public const string SchemeName = "wrapper";

        public IResourceHandler Create(IBrowser browser, IFrame frame, string schemeName, IRequest request)
        {
            var uri = new Uri(request.Url);
            var fileName = request.Url.Substring(request.Url.IndexOf("://")+2);
            string resource;
            if (schemeName == SchemeName)
            {
                return new CefSharpSchemeHandler();

            }
            return null;
        }
    }
}