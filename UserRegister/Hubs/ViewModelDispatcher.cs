﻿using SignalR.Hubs;

namespace UserRegister.Hubs
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