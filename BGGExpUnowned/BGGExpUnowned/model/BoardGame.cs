using System;
using System.Collections.Generic;
using System.Text;

namespace com.mbpro.BGGExpUnowned.model
{
     public class BoardGame
    {
        public BoardGame(long ID, string name)
        {
            this.ID = ID;
            this.name = name;
        }

        public long ID { get; set; }
        public string name { get; set; }
    }
}
