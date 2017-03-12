using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyClinicalCourse.ClinicalCourse
{
    /// <summary>
    /// 見出しのモデル
    /// </summary>
    public class HeaderModel : NotificationBase
    {
        /// <summary>
        /// 見出しのモデルの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="label">見出しのラベル</param>
        /// <param name="width">見出しの幅</param>
        public HeaderModel(string label, double width)
        {
            this.Label = label;
            this.Width = width;
        }

        private string _label = string.Empty;
        /// <summary>
        /// 見出しのラベル を取得または設定します。
        /// </summary>
        public string Label
        {
            get => this._label;
            set => SetProperty(ref this._label, value);
        }

        private double _width = default(double);
        /// <summary>
        /// 見出しの幅を表す数値 を取得または設定します。
        /// </summary>
        public double Width
        {
            get => this._width;
            set => SetProperty(ref this._width, value);
        }
    }
}
