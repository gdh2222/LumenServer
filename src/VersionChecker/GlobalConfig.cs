namespace VersionChecker
{
    public class GlobalConfig
    {
        /// <summary>
        /// 파일리스트 관리 파일 이름
        /// </summary>
        public static readonly string AseetFileName = "AssetList.xlsx.json";

        /// <summary>
        /// CDN URL
        /// Production -> AWS CloudFront Url 로 변경
        /// </summary>
        public static string CDN_Prefix = "http://127.0.0.1:11111/";

    }
}
