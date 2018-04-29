using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace Radikool6.Entities
{
    public class Library
    {
        public string Id { get; set; }
        public string FileName { get; set; }
        public string Path { get; set; }

        public string ProgramJson
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_programJson) && Program != null)
                {
                    _programJson = JsonConvert.SerializeObject(Program);
                }

                return _programJson;
            }
            set
            {
                Program = JsonConvert.DeserializeObject<Program>(value);
                _programJson = value;
            }
        }

        private string _programJson;

        public Program Program { get; set; }
    }
}
