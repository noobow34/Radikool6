using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Radikool6.Classes;
using Radikool6.Entities;
using Radikool6.Radio;

namespace Radikool6.BackgroundTask
{
    public class RadikoRecorder : Recorder, IRecorder
    {
        private string _token;
        private Process _ffmpeg;

        public RadikoRecorder(CommonConfig config, ReserveTask task = null) : base(config, task)
        {
        //    Start();
        }

        public async Task TimeFree(Entities.Program program)
        {
            
            var m3U8 = await Radiko.GetTimeFreeM3U8(program);
            var arg = $"-i {m3U8} -acodec copy \"{program.Title}.aac\"";
            CreateProcess(arg);

        //    await System.Threading.Tasks.Task.Factory.StartNew(() =>
        //    {
                _ffmpeg.Start();

                _ffmpeg.BeginOutputReadLine();
                _ffmpeg.BeginErrorReadLine();
         //   });

        }

        private void CreateProcess(string arg)
        {
            _ffmpeg = new Process
            {
                StartInfo =
                {
                    FileName = "ffmpeg",
                    Arguments = arg,
                    RedirectStandardOutput = true,
                    RedirectStandardInput = true,
                    RedirectStandardError = false,
                    CreateNoWindow = true,
                    UseShellExecute = false,
                }
            };

            
            _ffmpeg.OutputDataReceived += process_OutputDataReceived;
            _ffmpeg.ErrorDataReceived += process_OutputDataReceived;
            _ffmpeg.Exited += process_Exited;
        }



        private async Task Start()
        {
            try
            {
                _token = await Radio.Radiko.GetAuthToken();
                var arg = Define.Radiko.FfmpegArgs.Replace("[TOKEN]", _token)
                    .Replace("[CH]", Task.Station.Code);
                _ffmpeg = new Process();
                _ffmpeg.StartInfo.FileName = "ffmpeg";
                _ffmpeg.StartInfo.Arguments = Define.Radiko.FfmpegArgs.Replace("[TOKEN]", _token)
                    .Replace("[CH]", Task.Station.Code);
                
                
                
                
                
                _ffmpeg.StartInfo.RedirectStandardOutput = true;
                _ffmpeg.StartInfo.RedirectStandardInput = true;
                _ffmpeg.StartInfo.RedirectStandardError  = false;
                _ffmpeg.StartInfo.CreateNoWindow = true;
                _ffmpeg.StartInfo.UseShellExecute = false;
                _ffmpeg.StartInfo.CreateNoWindow = false;
                
                
                _ffmpeg.OutputDataReceived              += new DataReceivedEventHandler(process_OutputDataReceived);
                _ffmpeg.ErrorDataReceived               += new DataReceivedEventHandler(process_OutputDataReceived);
                _ffmpeg.Exited += new EventHandler(process_Exited);
                
                
                
                var ccc = _ffmpeg.Start();


            }
            catch (Exception ex)
            {
                var a = ex.Message;
            }
         }

        
        void process_Exited(object sender, System.EventArgs e)
        {
            
        }

        void process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (e.Data == null)
            {
                this.process_Exited(sender, e);
            }
            else
            {

                var a = System.Text.Encoding.UTF8.GetBytes(e.Data);
            }

        }


    }
}