using System;
using System.Collections.Generic;
using System.Text;

namespace com.mbpro.BGGExpUnowned.model
{
     public class BoardGame
    {
        public BoardGame(long ID, string Name)
        {
            this.ID = ID;
            this.Name = Name;
        }

        public long ID { get; set; }
        public string Name { get; set; }
    }
}
