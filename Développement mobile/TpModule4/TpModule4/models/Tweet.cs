using System;
using System.Collections.Generic;
using System.Text;

namespace TpModule4.models
{
    class Tweet
    {
     

        public string Id { get; set; }
        public string DateCreation { get; set; }
        public string Texte { get; set; }
        public string NomUser { get; set; }
        public string IdUser { get; set; }
        public string PseudoUser { get; set; }

        public Tweet(string v1, string v2, string v3, string v4, string v5, string v6)
        {
        }
    }
}
