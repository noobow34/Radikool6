namespace Radikool6.Entities
{
    public class EncodeSetting
    {
        /// <summary>
        /// フォーマットID
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 表示名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// サンプリングレート
        /// </summary>
        public string SamplingRate { get; set; }

        /// <summary>
        /// ビットレート
        /// </summary>
        public string BitRate { get; set; }

        /// <summary>
        /// 音量
        /// </summary>
        public string Volume { get; set; }

        /// <summary>
        /// 引数を指定するか
        /// </summary>
        public bool IsUseArgs  { get; set; }

        /// <summary>
        /// 編集用ffmpegのコマンド
        /// </summary>
        public string Args { get; set; }

        /// <summary>
        /// ffmpegのコマンド
        /// </summary>
        public string Cmd { get; set; }

        /// <summary>
        /// 拡張子
        /// </summary>
        public string Ext { get; set; }

        /// <summary>
        /// コーデック
        /// </summary>
        public string Codec { get; set; }

        /// <summary>
        /// 元のフォーマットID
        /// </summary>
        public int BaseId { get; set; }

        /// <summary>
        /// 既定の録音形式か
        /// </summary>
        public bool IsDefault { get; set; }
    }
}