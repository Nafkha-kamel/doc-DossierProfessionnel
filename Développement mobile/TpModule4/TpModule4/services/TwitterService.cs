using System;
using System.Collections.Generic;
using System.Text;
using TpModule4.models;

namespace TpModule4.services
{
    class TwitterService : ITwitterService
    {
        public bool authenticate(string userName, string motDePasse)
        {
            return true; 
        }

        public List<Tweet> getTweets(string chaine)
        {
            var listeTweets = new List<Tweet> { new Tweet("1","23/12/2009", "Joyeux évennement !", "kamel", "1", "kimo" ),
                new Tweet("2","14/01/2011", "Joyeux évennement !", "Nafkha", "2", "NK" ),
                new Tweet("3","17/03/2015", "C'est la fête !", "Sirine", "3", "Sarouna" ),
            };
            return listeTweets;
        }
    }
}
