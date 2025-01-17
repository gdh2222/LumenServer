using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLib
{

    /* Memo
     * 일반적으로 유저 계정 ( Google, Apple, GameCenter, Firebase .. ) 정보를 체크할때
     * String => Token 정보 한개만 존재하면 정보 체크가 가능하다.
     * 하지만 추후 확장을 위해서, String ( JSON ) 으로 데이터를 수신받고 Deserialize 해서 사용하도록 구조를 설계
     */

    #region Credentials

    public class CredentialBase
    {
        /// <summary>
        /// 계정고유토큰
        /// </summary>
        public string AccountToken { get; set; } = string.Empty;
    }


    public partial class Credential_Guest : CredentialBase
    {
        // AccountToken
        // 클라이언트에서 발급한 GUID
    }


    public partial class Credential_GameCenter : CredentialBase
    {
        // AccountToken
        // 게임센터의 토큰정보
    }

    public partial class Credential_GooglePlay : CredentialBase
    {
        // AccountToken
        // 구글의 토큰정보
    }

    public partial class Credential_Firebase : CredentialBase
    {
        // AccountToken
        // 파이어베이스 로그인 토큰정보

    }
    #endregion
}
