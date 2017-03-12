namespace MyClinicalCourse
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// 経過表の1経過単位を表すモデル
    /// </summary>
    public class CourseViewModel
    {
        /// <summary>
        /// ラベル を取得または設定します。
        /// </summary>
        public string Label { get; set; } = string.Empty;

        /// <summary>
        /// 血圧 を取得または設定します。
        /// </summary>
        public double BloodPressure { get; set; } = 0.0d;

        /// <summary>
        /// 体温 を取得または設定します。
        /// </summary>
        public double BodyTemperature { get; set; } = 0.0d;

        /// <summary>
        /// 脈拍 を取得または設定します。
        /// </summary>
        public double Pulse { get; set; } = 0.0d;

        /// <summary>
        /// 呼吸数 を取得または設定します。
        /// </summary>
        public double Breathing { get; set; } = 0.0d;

        /// <summary>
        /// 表項目 を取得または設定します。
        /// </summary>
        public IDictionary TableItems { get; set; }
    }
}
