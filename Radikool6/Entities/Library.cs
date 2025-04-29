using Newtonsoft.Json;

namespace Radikool6.Entities
{
    public class Library
    {
        public string Id { get; set; }
        public string FileName { get; set; }
        public byte[] FileBinary { get; set; }
        public string Size { get; set; }
        public string created { get; set; }

        [JsonIgnore]
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

        public Program Program
        {
            get => _program;
            set
            {
                _program = value;
                _programJson = JsonConvert.SerializeObject(value);
            }
        }

        private Program _program;
    }
}
