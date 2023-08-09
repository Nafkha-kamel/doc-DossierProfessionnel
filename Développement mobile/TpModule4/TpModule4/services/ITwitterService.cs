using System;
using System.Collections.Generic;
using System.Text;
using TpModule4.models;

namespace TpModule4.services
{
    interface ITwitterService
    {
        bool authenticate(string userName, string motDePasse);
        List<Tweet> getTweets(string chaine);

    }
}
