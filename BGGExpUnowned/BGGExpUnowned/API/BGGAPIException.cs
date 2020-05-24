using System;
using System.Collections.Generic;
using System.Text;

namespace com.mbpro.BGGExpUnowned.API
{
    class BGGAPIException : Exception
    {
        public BGGAPIException()
        {
        }

        public BGGAPIException(string message)
            : base(message)
        {
        }

        public BGGAPIException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
