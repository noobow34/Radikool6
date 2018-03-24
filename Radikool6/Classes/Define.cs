using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using Radikool6.Entities;

namespace Radikool6.Classes
{
    public class Define
    {
        public class File
        {
            public static readonly string DbFile = Path.Combine("data", "data.db");
            public static readonly string KeyFile = Path.Combine("data", ".key");
        }

        public class Config
        {
            public const string Common = "common";
            public const string EncodeSetting = "encode_setting";
        }

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
            public const string TodayStationTimeTable =
                "http://radiko.jp/v2/api/program/station/today?station_id={station_id}";

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
            public const string Auth1 = "https://radiko.jp/v2/api/auth1";

            /// <summary>
            /// 認証URL2
            /// </summary>
            public const string Auth2 = "https://radiko.jp/v2/api/auth2";

            /// <summary>
            /// rtmpdumpの引数
            /// </summary>
            public const string RtmpdumpArgs =
                "-r \"rtmpe://f-radiko.smartstream.ne.jp/[CH]/_definst_\" -a \"[CH]/_definst_\" -W \"[SWF]\" -C S: -C S: -C S: -C S:[TOKEN] -y \"simul-stream.stream?ucid=null\"";

            /// <summary>
            /// common.js
            /// </summary>
            public const string CommonJs = "http://radiko.jp/apps/js/playerCommon.js";


            /// <summary>
            /// rtmpgwの引数
            /// </summary>
            public const string RtmpGwArgs =
                "-r \"rtmpe://f-radiko.smartstream.ne.jp/[CH]/_definst_\" -a \"[CH]/_definst_\" -W \"http://radiko.jp/apps/js/flash/myplayer-release.swf?station_id=[CH]\" --conn S: --conn S: --conn S: --conn S:[TOKEN] -y \"simul-stream.stream?ucid=null\" --live --device [IP] --sport [PORT]";

            public const string RawFFmpegArgs = "";

            /// <summary>
            /// stream
            /// </summary>
            public const string StationStream = "http://radiko.jp/v2/station/stream_smh_multi/[CH].xml";

            /// <summary>
            /// playlist.m3u8
            /// </summary>
            public const string PlayList =
                "http://f-radiko.smartstream.ne.jp/[CH]/_definst_/simul-stream.stream/playlist.m3u8";

            public const string FfmpegArgs = "";

            //                var args = $"-headers 'X-Radiko-AuthToken: {t.Result}' -i '{url}' -t 00:00:30 -acodec copy test.aac";


        }

        /// <summary>
        /// NHK関連
        /// </summary>
        public class Nhk
        {
            /// <summary>
            /// NHKホーム
            /// </summary>
            public const string Home = "";

            /// <summary>
            /// 今日の番組表
            /// </summary>
            public const string DailyTimeTable =
                "http://cgi4.nhk.or.jp/hensei/api/sche-nr.cgi?ch=net{station_id}&date={date}&tz=all&mode=xml";

            /// <summary>
            /// 放送URL
            /// </summary>
            public const string Swf =
                "http://www3.nhk.or.jp/netradio/files/swf/rtmpe.swf?buffer=2&ch={station_id}&area={area}&server_r1=rtmpe://netradio-hkr1-flash.nhk.jp/live/&stream_r1=NetRadio_HKR1_flash@108442&server_r2=rtmpe://netradio-r2-flash.nhk.jp/live/&stream_r2=NetRadio_R2_flash@63342&server_fm=rtmpe://netradio-hkfm-flash.nhk.jp/live/&stream_fm=NetRadio_HKFM_flash@108237";

            /// <summary>
            /// 種別
            /// </summary>
            public const string TypeName = "nhk";

            /// <summary>
            /// Windows Media Playerサイト
            /// </summary>
            public const string Wmp = "http://windows.microsoft.com/ja-jp/windows/windows-media-player";

            /// <summary>
            /// rtmpdumpの引数
            /// </summary>
            //  public const string RtmpdumpArgs = "--rtmp {rtmp} --app \"live\" --playpath {play_path} -W {swf} --live";
            //     public const string RtmpdumpArgs = "--rtmp {rtmp} --app \"live\" --swfhash \"89a3f354dd60c59f837a49e326fcefab78a8fa896e535c1d3ccc15872bcf31b3\" --swfsize 126752  --playpath {play_path} -W {swf} --live";
            public const string RtmpdumpArgs =
                "--rtmp {rtmp} -W http://www3.nhk.or.jp/netradio/files/swf/rtmpe_ver2015.swf --live";

            public const string StationList = "http://www3.nhk.or.jp/netradio/app/config_pc.xml";


            // JCBA用
            // rtmpdump --rtmp "rtmp://jcbasimul015-live1.sp1.fmslive.stream.ne.jp/jcbasimul015-live1/_definst_/jcbasimul015-live" --timeout 3 -B "600" --live -o "jcba.flv"

            public const string Config = "http://www.nhk.or.jp/radio/config/config_web.xml";
            //      public const string Image = "http://www3.nhk.or.jp/netradio/files/img/parts.png";

            public const string Image = "http://www.nhk.or.jp/radio/img/parts.png";

        }

        /// <summary>
        /// 規定の録音形式
        /// </summary>
        public class EncodeSettings
        {
            public readonly List<EncodeSetting> Default = new List<EncodeSetting>()
            {
                // mp3
                new EncodeSetting
                {
                    Id = "1",
                    Name = "mp3",
                    BitRate = "128k",
                    SamplingRate = "44100",
                    Volume = "256",
                    Ext = "mp3",
                    Codec = "-acodec libmp3lame",
                    BaseId = 1
                },
                // m4a
                new EncodeSetting
                {
                    Id = "2",
                    Name = "m4a",
                    BitRate = "128k",
                    SamplingRate = "44100",
                    Volume = "256",
                    Ext = "m4a",
                    Codec = "-acodec copy",
                    BaseId = 2
                }
            };
        }
    }
}
