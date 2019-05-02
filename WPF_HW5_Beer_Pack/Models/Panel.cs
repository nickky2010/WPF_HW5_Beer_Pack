using System.Collections.Generic;

namespace WPF_HW5_Beer_Pack.Models
{
    class Panel
    {
        public string PanelId { get; set; }
        public string PanelName { get; set; }
        public float HingeOffset { get; set; }
        public float PanelWidth { get; set; }
        public float PanelHeight { get; set; }
        public int AttachedToSide { get; set; }
        public float PointNullX { get; set; }
        public float PointNullY { get; set; }
        public int RouteRotate { get; set; }
        public bool Rotate { get; set; }
        public List<Panel> СhildPanel;

        public Panel()
        {
            СhildPanel = new List<Panel>();
        }
    }
}
