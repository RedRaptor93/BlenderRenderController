using System;
using Gtk;
using UI = Gtk.Builder.ObjectAttribute;


namespace BlenderRenderController
{
    class DetailDialog : Dialog
    {
#pragma warning disable 649
        [UI] Image dialogIcon;
        [UI] TextBuffer detailsBuffer;
        [UI] Label dialogMsgLbl;
#pragma warning restore 649

        private DetailDialog(Tuple<Builder, CssProvider> elements, string root) 
            : base(elements.Item1.GetObject(root).Handle)
        {
            elements.Item1.Autoconnect(this);
            StyleContext.AddProviderForScreen(Screen, elements.Item2, 800);
        }

        public DetailDialog(Window parent, string message, string details, MessageType type)
            : this(Glade.LoadUI("Dialogs.glade", "brc_style.css"), "DetailDialog")
        {
            TransientFor = parent;
            dialogMsgLbl.Text = message;
            detailsBuffer.Text = details;
            SetDialogTitleAndIcon(type);
        }

        string GetResponseText(ResponseType responseType)
        {
            switch (responseType)
            {
                case ResponseType.DeleteEvent:
                    return Stock.Lookup(Stock.Delete).Label;
                case ResponseType.Ok:
                    return Stock.Lookup(Stock.Ok).Label;
                case ResponseType.Cancel:
                    return Stock.Lookup(Stock.Cancel).Label;
                case ResponseType.Close:
                    return Stock.Lookup(Stock.Close).Label;
                case ResponseType.Yes:
                    return Stock.Lookup(Stock.Yes).Label;
                case ResponseType.No:
                    return Stock.Lookup(Stock.No).Label;
                case ResponseType.Apply:
                    return Stock.Lookup(Stock.Apply).Label;
                case ResponseType.Help:
                    return Stock.Lookup(Stock.Help).Label;
                case ResponseType.Reject:
                    goto case ResponseType.No;
                case ResponseType.Accept:
                case ResponseType.None:
                default:
                    goto case ResponseType.Ok;
            }
        }

        void SetDialogTitleAndIcon(MessageType type)
        {
            string stockTitle = type.ToString();
            string iconName = "dialog-";
            
            switch (type)
            {
                case MessageType.Info:
                    iconName += "information";
                    break;
                case MessageType.Warning:
                    iconName += "warning";
                    break;
                case MessageType.Question:
                    iconName += "question";
                    break;
                case MessageType.Error:
                    iconName += "error";
                    break;
                case MessageType.Other:
                default:
                    iconName = "image-missing";
                    stockTitle = null;
                    break;
            }

            dialogIcon.Pixbuf = IconTheme.GetForScreen(Screen).LoadIcon(iconName, 48, 0);
            Title = stockTitle;
        }
    }
}
