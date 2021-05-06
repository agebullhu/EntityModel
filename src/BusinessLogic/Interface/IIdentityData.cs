/*****************************************************
(c)2016-2021 by ZeroTeam
����: ����ˮ
����: Agebull.EntityModel.CoreAgebull.DataModel
����:2016-06-07
�޸�: -
*****************************************************/

using System;

namespace Agebull.EntityModel.Common
{
    /// <summary>
    ///     ��ʾ��ȫ��Ψһ��ʶ������
    /// </summary>
    public interface IGlobalKey
    {
        /// <summary>
        ///     ���ֱ�ʶ
        /// </summary>
        /// <value>int</value>
        Guid Key { get; set; }
    }

    /// <summary>
    ///     ��ʾ��Ψһ���ֱ�ʶ������
    /// </summary>
    public interface IIdentityData<TPrimaryKey>
    {
        /// <summary>
        ///     ���ֱ�ʶ
        /// </summary>
        /// <value>int</value>
        TPrimaryKey Id { get; set; }
    }

    /// <summary>
    ///     ��ʾ��Ψһ���ֱ�ʶ������
    /// </summary>
    public interface IIdentityData : IIdentityData<long>
    {
    }

    /// <summary>
    ///     ��ʾʹ��ѩ����ķ�������������
    /// </summary>
    public interface ISnowFlakeId : IIdentityData<long>
    {

    }

    /// <summary>
    ///     ��ʾ�б��������
    /// </summary>
    public interface ITitle
    {
        /// <summary>
        ///     ����
        /// </summary>
        /// <value>int</value>
        string Title { get; }
    }
    /// <summary>
    ///     ��ʾ��Ψһ���ֱ�ʶ������
    /// </summary>
    public interface IUnionUniqueEntity
    {
        /// <summary>
        ///     ��Ϻ��Ψһֵ
        /// </summary>
        string UniqueValue { get; }
    }

}