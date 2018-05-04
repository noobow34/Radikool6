namespace Radikool6.Entities
{
    public class Config
    {
        /// <summary>
        /// radikoプレミアムメールアドレス
        /// </summary>
        public string RadikoEmail { get; set; } = "";

        /// <summary>
        /// radikoプレミアムパスワード
        /// </summary>
        public string RadikoPassword { get; set; } = "";

        /// <summary>
        /// 録音ファイル名
        /// </summary>
        public string FileName { get; set; } = "[TITLE]_[SYEAR][SMONTH][SDAY]";

        /// <summary>
        /// 放送終了後タイムフリーで録音するまでの待ち時間(分)
        /// </summary>
        public int TimeFreeMargin { get; set; } = 30;

        /// <summary>
        /// タグ　タイトル
        /// </summary>
        public string TagTitle { get; set; } = "[TITLE]";

        /// <summary>
        /// タグ　アーティスト
        /// </summary>
        public string TagArtist { get; set; } = "[CAST]";

        /// <summary>
        /// タグ　アルバム
        /// </summary>
        public string TagAlbum { get; set; } = "[TITLE]";

        /// <summary>
        /// タグ　ジャンル
        /// </summary>
        public string TagGenre { get; set; } = "[CH_NAME]";

        /// <summary>
        /// タグ　コメント
        /// </summary>
        public string TagComment { get; set; } = "[SYEAR]年[SMONTH]月[SDAY]日";

        /// <summary>
        /// サンプリングレート
        /// </summary>
        public string SamplingRate { get; set; } = "44100";

        /// <summary>
        /// ビットレート
        /// </summary>
        public string BitRate { get; set; } = "128k";

        /// <summary>
        /// 音量
        /// </summary>
        public string Volume { get; set; } = "256";
    }
}