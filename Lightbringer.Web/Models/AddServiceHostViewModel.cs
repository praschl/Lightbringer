﻿using System.Collections.Generic;
using Lightbringer.Web.Store;
using Lightbringer.Web.Store.Store;

namespace Lightbringer.Web.Models
{
    public class AddServiceHostViewModel
    {
        public string ServiceHostUrl { get; set; }

        public string Error { get; set; }
        public IReadOnlyCollection<ServiceHost> ServiceHosts { get; set; }
    }
}