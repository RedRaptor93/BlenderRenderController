using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gtk;

namespace BlenderRenderController
{
    class AboutWin : AboutDialog
    {
        public AboutWin(Builder builder) : base(builder.GetObject("AboutWin").Handle)
        {
            builder.Autoconnect(this);
        }
        

        void On_AboutWin_Close(object o, EventArgs e)
        {
            this.Hide();
        }
    }
}
