using Radikool6.Entities;

namespace Radikool6.BackgroundTask
{
    public class RadikoRecorder : Recorder, IRecorder
    {
        public RadikoRecorder(ReserveTask task) : base(task)
        {

        }
    }
}