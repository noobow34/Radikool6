using System;
using System.Timers;

namespace Radikool6.BackgroundTask
{
    public class Core
    {
        private readonly Timer _timer;
        

        public Core()
        {
            _timer = new Timer(1000);
            _timer.Elapsed += (sender, eventArgs) =>
            {
                
            };
           
        }
        
        public void Run()
        {
            _timer.Start();
        }
    }
}