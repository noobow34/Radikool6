using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Radikool6.Classes
{
    public class Define
    {
        public class Radiko
        {
            /// <summary>
            /// Radiko
            /// </summary>
            public const string Home = "http://radiko.jp/";

            /// <summary>
            /// 週刊番組表
            /// </summary>
            public const string WeeklyTimeTable = "http://radiko.jp/v3/program/station/weekly/[stationCode].xml";

            /// <summary>
            /// 今日の番組表
            /// </summary>
            public const string DailyTimeTable = "http://radiko.jp/v2/api/program/today?area_id={area_id}";

            /// <summary>
            /// 今日の番組表(放送局指定)
            /// </summary>
            public const string TodayStationTimeTable = "http://radiko.jp/v2/api/program/station/today?station_id={station_id}";

            /// <summary>
            /// 地域判定用
            /// </summary>
            public const string AreaCheck = "http://radiko.jp/area/";

            /// <summary>
            /// ログインURL
            /// </summary>
          //  public const string Login = "https://radiko.jp/ap/member/login/login";
            public const string Login = "https://radiko.jp/ap/member/login/login_page";
            //   https://radiko.jp/ap/member/login/login_page

            /// <summary>
            /// ログインチェック
            /// </summary>
            public const string LoginCheck = "https://radiko.jp/ap/member/webapi/member/login/check";

            /// <summary>
            /// ログアウトURL
            /// </summary>
            public const string Logout = "http://radiko.jp/ap/member/webapi/member/logout";

            /// <summary>
            /// プレミアム登録URL
            /// </summary>
            public const string Regist = "https://radiko.jp/ap/member/regist/regist_mail_page?premium=1";

            /// <summary>
            /// 種別
            /// </summary>
            public const string TypeName = "radiko";

            /// <summary>
            /// 放送局画像
            /// </summary>
            public const string StationImage = "http://radiko.jp/station/logo/[CH]/logo_large.png";

            /// <summary>
            /// 放送局一覧(すべて)
            /// </summary>
            public const string StationListFull = "http://radiko.jp/v3/station/region/full.xml";

            /// <summary>
            /// 放送局一覧(都道府県ごと)
            /// </summary>
            public const string StationListPref = "http://radiko.jp/v2/station/list/{area_id}.xml";

            public const string Thumbnail = "http://radiko.jp/station/logo/[CH]/logo_small.png";

            /// <summary>
            /// 認証URL1
            /// </summary>
            public const string Auth1 = "https://radiko.jp/v2/api/auth1_fms";

            /// <summary>
            /// 認証URL2
            /// </summary>
            public const string Auth2 = "https://radiko.jp/v2/api/auth2_fms";

            /// <summary>
            /// rtmpdumpの引数
            /// </summary>
            public const string RtmpdumpArgs = "-r \"rtmpe://f-radiko.smartstream.ne.jp/[CH]/_definst_\" -a \"[CH]/_definst_\" -W \"[SWF]\" -C S: -C S: -C S: -C S:[TOKEN] -y \"simul-stream.stream?ucid=null\"";

            /// <summary>
            /// rtmpgwの引数
            /// </summary>
            public const string RtmpGwArgs = "-r \"rtmpe://f-radiko.smartstream.ne.jp/[CH]/_definst_\" -a \"[CH]/_definst_\" -W \"http://radiko.jp/apps/js/flash/myplayer-release.swf?station_id=[CH]\" --conn S: --conn S: --conn S: --conn S:[TOKEN] -y \"simul-stream.stream?ucid=null\" --live --device [IP] --sport [PORT]";

            public const string RawFFmpegArgs = "";

            /// <summary>
            /// stream
            /// </summary>
            public const string StationStream = "http://radiko.jp/v2/station/stream_smh_multi/[CH].xml";

            public const string FfmpegArgs = "";

        }
    }
}
