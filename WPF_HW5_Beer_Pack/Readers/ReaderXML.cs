using WPF_HW5_Beer_Pack.Interfaces;
using WPF_HW5_Beer_Pack.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml;

namespace WPF_HW5_Beer_Pack.Readers
{
    class ReaderXML : IReader<MainModel>
    {
        IFormatProvider provider = CultureInfo.InvariantCulture.NumberFormat;
        public MainModel Read(string filename)
        {
            MainModel model = new MainModel();
            XmlDocument doc = new XmlDocument();
            doc.Load(filename);
            XmlElement xRoot = doc.DocumentElement;
            model.RootX = Convert.ToInt32(Math.Round(double.Parse(xRoot.GetAttribute("rootX"), provider)));
            model.RootY = Convert.ToInt32(Math.Round(double.Parse(xRoot.GetAttribute("rootY"), provider)));
            model.OriginalDocumentHeight = int.Parse(xRoot.GetAttribute("originalDocumentHeight"));
            model.OriginalDocumentWidth = int.Parse(xRoot.GetAttribute("originalDocumentWidth"));
            foreach (XmlNode xnode in xRoot)
            {
                if (xnode.Name == "panels")
                    SetPanels(xnode, model.Panels);
            }
            return model;
        }

        private void SetPanels(XmlNode xnode, List<Panel> panels)
        {
            foreach (XmlNode childnode in xnode.ChildNodes)
            {
                Panel panel = null;
                if (childnode.Name == "item")
                {
                    panel = new Panel();
                    SetItemAttribute(panel, childnode);
                    panels.Add(panel);
                }
                foreach (XmlNode attachNode in childnode.ChildNodes)
                {
                    if (attachNode.Name == "attachedPanels")
                    {
                        List<Panel> childPanel = panels.Find(p => p.PanelId == panel.PanelId).СhildPanel;
                        SetPanels(attachNode, childPanel);
                    }
                }
            }
        }

        private void SetItemAttribute(Panel panel, XmlNode childnode)
        {
            panel.PanelId = childnode.Attributes.GetNamedItem("panelId").Value;
            panel.PanelName = childnode.Attributes.GetNamedItem("panelName").Value;
            panel.HingeOffset = float.Parse(childnode.Attributes.GetNamedItem("hingeOffset").Value, provider);
            panel.PanelWidth = float.Parse(childnode.Attributes.GetNamedItem("panelWidth").Value, provider);
            panel.PanelHeight = float.Parse(childnode.Attributes.GetNamedItem("panelHeight").Value, provider);
            panel.AttachedToSide = int.Parse(childnode.Attributes.GetNamedItem("attachedToSide").Value);
        }
    }
}
