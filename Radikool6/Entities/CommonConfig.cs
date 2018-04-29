namespace Radikool6.Entities
{
    public class CommonConfig
    {
        /// <summary>
        /// radikoプレミアムメールアドレス
        /// </summary>
        public string RadikoEmail { get; set; }
        
        /// <summary>
        /// radikoプレミアムパスワード
        /// </summary>
        public string RadikoPassword { get; set; }
        
        /// <summary>
        /// 録音ファイル名
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// 放送終了後タイムフリーで録音するまでの待ち時間(分)
        /// </summary>
        public int TimeFreeMargin { get; set; } = 30;
    }
}