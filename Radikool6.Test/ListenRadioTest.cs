using System;
using System.Linq;
using System.Threading.Tasks;
using Radikool6.BackgroundTask;
using Radikool6.Classes;
using Radikool6.Entities;
using Radikool6.Radio;
using Xunit;

namespace Radikool6.Test
{
    public class ListenRadioTest
    {
        [Fact]
        public async Task GetStationsTest()
        {
            var stations = await ListenRadio.GetStations();
            Assert.NotEmpty(stations);
        }
        
        [Fact]
        public async Task GetProgramsTest()
        {
            var stations = await ListenRadio.GetStations();
            var programs = await ListenRadio.GetPrograms(stations.First());
            Assert.NotEmpty(programs);
        }

        [Fact]
        public async Task RecorderTest()
        {
            var recorder = new ListenRadioRecorder(new Config());
            await recorder.Start();
        }
    }
}