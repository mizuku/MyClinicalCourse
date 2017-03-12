namespace MyClinicalCourse.ClinicalCourse
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// 軸の配置場所の種類
    /// </summary>
    public enum AxisPosition : short
    {
        /// <summary>
        /// 上配置
        /// </summary>
        Top,
        /// <summary>
        /// 左配置
        /// </summary>
        Left,
        /// <summary>
        /// 右配置
        /// </summary>
        Right,
        /// <summary>
        /// 下配置
        /// </summary>
        Bottom

    }

    /// <summary>
    /// 軸の目盛りの種類
    /// </summary>
    public enum TickType : short
    {
        /// <summary>
        /// 長針
        /// </summary>
        Major,
        /// <summary>
        /// 短針
        /// </summary>
        Minor
    }

    public static class EnumExtensions
    {
        /// <summary>
        /// 軸が左右ならtrue, 上下ならfalseを取得します。
        /// </summary>
        /// <param name="position">軸の位置</param>
        /// <returns>軸の位置判定</returns>
        public static bool IsSideAxis(this AxisPosition position) => AxisPosition.Left == position || AxisPosition.Right == position;
    }
}
