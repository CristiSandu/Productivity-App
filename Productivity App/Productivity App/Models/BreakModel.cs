using System;
using System.Collections.Generic;
using System.Text;

namespace Productivity_App.Models
{
   public class BreakModel
    {
        public DateTime BrakeTimeStart { get; set; }
        public DateTime BrakeTimeEnd { get; set; }
        public bool NotificationSent { get; set; } = false;
        //public string AllDate { set => return BrakeTimeStart + "-" + BrakeTimeEnd; get; }
    }
}
