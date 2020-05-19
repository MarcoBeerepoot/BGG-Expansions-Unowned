using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;
using System.Xml;
using com.mbpro.BGGExpUnowned.model;

namespace com.mbpro.BGGExpUnowned.API
{
    abstract class APICommand
    {
        public static readonly string BASE_URL = "https://boardgamegeek.com/";
        public static readonly string API_URL = BASE_URL + "xmlapi2/";
        protected XmlDocument XDoc = new XmlDocument();
        protected abstract bool Load();
        protected abstract void Process();
        protected List<BoardGame> Result = new List<BoardGame>();
        public virtual List<BoardGame> Execute()
        {
            Load();
            Process();
            return Result;
        }
        
    }
}
