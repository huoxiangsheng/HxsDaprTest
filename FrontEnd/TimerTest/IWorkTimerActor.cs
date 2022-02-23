using Dapr.Actors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FrontEnd.TimerTest
{
    interface IWorkTimerActor : IActor
    {       
        Task<bool> Approve();
        Task RegisterTimer();
        Task UnregisterTimer();
        Task RegisterReminder();
        Task UnregisterReminder();

    }
}
