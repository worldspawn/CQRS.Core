using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SignalR.Hubs;

namespace Loveboat.Hubs
{
    public class ViewModelDispatcher : Hub
    {
        public void Join(string[] viewModelTypes)
        {
            if (viewModelTypes != null)
                foreach (var type in viewModelTypes)
                    Groups.Add(Context.ConnectionId, "vm_" + type);
        }
    }
}