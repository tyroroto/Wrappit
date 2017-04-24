using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WrapitApplication
{
    class AppControllerBinder
    {
        private Form _form;
        public AppControllerBinder(Form form)
        {
            _form = form;
        }

        //TODO ShowMessage with callback

        public void ShowMessage(string msg)
        {//Read Note
            MessageBox.Show(msg);
        }

        public bool SetScreenPosition(int x, int y)
        {
            //TODO Set ScreenSize
            return true;
        }

        public bool SetScreenSize(int x, int y, int width, int height)
        {
            //TODO Set ScreenSize
            return true;

        }

        public bool SetFullScreen(bool en)
        {
            //TODO Set ScreenSize
            return true;
        }

        public bool SetAppTitle(string title)
        {
            //TODO Set ScreenSize
            return true;
        }
    }
}
