using Agebull.Common.OAuth;
using Gboxt.Common.DataModel;

namespace Agebull.Common.WebApi.Auth
{
    /// <summary>
    /// �û����У��
    /// </summary>
    public interface IBearValidater
    {
        /// <summary>
        /// �����õ�ServiceKey�������ڲ����ã�
        /// </summary>
        /// <param name="token">����</param>
        /// <returns></returns>
        ApiResult  ValidateServiceKey(string token);
        /// <summary>
        /// ���AT(���Ե�¼�û�)
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        ApiResult <LoginUserInfo> VerifyAccessToken(string token);
        /// <summary>
        /// ����豸��ʶ������δ��¼�û���
        /// </summary>
        /// <param name="token">����</param>
        /// <returns></returns>
        ApiResult <LoginUserInfo> ValidateDeviceId(string token);

        /// <summary>
        /// ����豸��ʶ������δ��¼�û���
        /// </summary>
        /// <param name="uid">�û�ID</param>
        /// <returns></returns>
        ApiResult <LoginUserInfo> GetUserProfile(long uid);

        /// <summary>
        ///     ȡ���û���Ϣ
        /// </summary>
        /// <param name="token">����</param>
        /// <returns>�û���Ϣ</returns>
        ApiResult<LoginUserInfo> GetLoginUser(string token);
    }
}