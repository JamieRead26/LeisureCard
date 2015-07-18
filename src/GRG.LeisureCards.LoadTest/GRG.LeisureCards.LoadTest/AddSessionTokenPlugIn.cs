﻿using System.ComponentModel;
using Microsoft.VisualStudio.TestTools.WebTesting;

namespace GRG.LeisureCards.LoadTest
{
    [DisplayName("Add API Session Token Header")]
    [Description("Adds session token header to each API request")]
    public class AddSessionTokenPlugIn : WebTestPlugin
    {
        public override void PreRequestDataBinding(object sender, PreRequestDataBindingEventArgs e)
        {
          // if (e.Request.Url.IndexOf((string)e.WebTest.Context["API"])==0)

            object sessionToken;
            if (e.WebTest.Context.TryGetValue("SessionToken", out sessionToken))
                e.Request.Headers.Add("SessionToken", (string)sessionToken);
        }
    }
}
