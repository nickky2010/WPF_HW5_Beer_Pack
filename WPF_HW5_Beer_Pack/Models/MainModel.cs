using System.Collections.Generic;

namespace WPF_HW5_Beer_Pack.Models
{
    class MainModel
    {
        public int RootX { get; set; }
        public int RootY { get; set; }
        public int OriginalDocumentHeight { get; set; }
        public int OriginalDocumentWidth { get; set; }
        public List<Panel> Panels { get; set; }
        public MainModel()
        {
            Panels = new List<Panel>();
        }
    }
}
