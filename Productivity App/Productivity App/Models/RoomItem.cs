using System;
using System.Collections.Generic;
using System.Text;

namespace Productivity_App.Models
{
   public class RoomItem
    {
        public long Id { get; set; }
        public string Temp { get; set; }
        public string Humy { get; set; }
        public bool IslightOn { get; set; }
    }
}
