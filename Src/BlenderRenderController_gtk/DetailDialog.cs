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
#pragma warning restore 649

        private DetailDialog(Tuple<Builder, CssProvider> elements, string root) 
            : base(elements.Item1.GetObject(root).Handle)
        {
            elements.Item1.Autoconnect(this);
            StyleContext.AddProviderForScreen(Screen, elements.Item2, 800);
        }

        public DetailDialog(string message, string title, string details, Window parent, MessageType type, 
                            ButtonsType buttons = ButtonsType.Ok)
            : this(Glade.LoadUI("DetailDialog.glade", "brc_style.css"), "DetailedDialog")
        {
            SetupButtons(buttons);
            SetDialogIcon(type);
            detailsBuffer.Text = details;
            this.TransientFor = parent;
        }



        void SetupButtons(ButtonsType type)
        {
            switch (type)
            {
                case ButtonsType.Ok:
                    SetupButtons(null, null, ResponseType.Ok);
                    break;
                case ButtonsType.Close:
                    SetupButtons(null, null, ResponseType.Close);
                    break;
                case ButtonsType.Cancel:
                    SetupButtons(null, null, ResponseType.Cancel);
                    break;
                case ButtonsType.YesNo:
                    SetupButtons(null, ResponseType.Yes, ResponseType.No);
                    break;
                case ButtonsType.OkCancel:
                    SetupButtons(null, ResponseType.Ok, ResponseType.Cancel);
                    break;
                default:
                    throw new Exception("Unexpected ButtonType");
            }
        }

        void SetupButtons(params ResponseType?[] responses)
        {
            foreach (var r in responses)
            {
                if (r.HasValue)
                {
                    AddButton(GetResponseText(r.Value), r.Value);
                }
            }
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

        void SetDialogIcon(MessageType type)
        {
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
                    break;
            }

            var icTheame = IconTheme.GetForScreen(Screen);
            dialogIcon.Pixbuf = icTheame.LoadIcon(iconName, 48, 0);
        }

    }
}
