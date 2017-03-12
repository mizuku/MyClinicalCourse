namespace MyClinicalCourse.ClinicalCourse
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Media;

    /// <summary>
    /// 軸目盛りのモデル
    /// </summary>
    public class AxisTickModel : NotificationBase, IEquatable<AxisTickModel>
    {
        static private double compareMinValue = Math.Pow(10, -3);

        /// <summary>
        /// 軸目盛りを表すモデルの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="tickType">目盛りの種類</param>
        /// <param name="minimum">軸の最小値</param>
        /// <param name="distance">この目盛りの最小値からの差分の値</param>
        /// <param name="oneSize">値1あたりの描画上の大きさを表す数値</param>
        /// <param name="tick">目盛りの種類軸の描画に使用する Brush</param>
        /// <param name="label">ラベル文字列</param>
        public AxisTickModel(TickType tickType, double minimum, double distance, double oneSize, Brush tick, string label = null)
        {
            this.TickType = tickType;
            this.Minimum = minimum;
            this.Distance = distance;
            this.OneSize = oneSize;
            this.Tick = tick;
            this.Label = label ?? (TickType.Major == tickType ? (minimum + distance).ToString() : null);
        }

        private string _label = string.Empty;
        /// <summary>
        /// 軸に表示するラベル を取得または設定します。
        /// </summary>
        public string Label
        {
            get => this._label;
            internal set => SetProperty(ref _label, value);
        }

        private double _minimum = 0.0d;
        /// <summary>
        /// 軸の最小値 を取得または設定します。
        /// </summary>
        public double Minimum
        {
            get => this._minimum;
            internal set
            {
                if (SetProperty(ref _minimum, value))
                {
                    RaisePropertyChanged(nameof(Label));
                }
            }
        }

        private double _distance = 0.0d;
        /// <summary>
        /// ラベルの値を表す数値 を取得または設定します。
        /// </summary>
        public double Distance
        {
            get => this._distance;
            internal set
            {
                if (SetProperty(ref _distance, value))
                {
                    RaisePropertyChanged(nameof(Position));
                }
            }
        }

        private double _oneSize = 0.0d;
        /// <summary>
        /// 値1あたりの描画上の大きさを表す数値 を取得または設定します。
        /// </summary>
        public double OneSize
        {
            get => this._oneSize;
            internal set
            {
                if (SetProperty(ref _oneSize, value))
                {
                    RaisePropertyChanged(nameof(Position));
                }
            }
        }

        /// <summary>
        /// ラベルの位置を表す数値 を取得または設定します。
        /// </summary>
        public double Position
        {
            get => this.Distance * this.OneSize;
        }

        private TickType _tickType = TickType.Major;
        /// <summary>
        /// 目盛りの種類 を取得または設定します。
        /// </summary>
        public TickType TickType
        {
            get => this._tickType;
            internal set
            {
                if (SetProperty(ref this._tickType, value))
                {
                    RaisePropertyChanged(nameof(TickSize));
                }
            }
        }

        private Brush _tick = null;
        /// <summary>
        /// 軸の描画に使用する Brush を取得または設定します。
        /// </summary>
        public Brush Tick
        {
            get => this._tick;
            internal set => SetProperty(ref this._tick, value);
        }

        /// <summary>
        /// 目盛りの大きさ を取得します。
        /// </summary>
        public double TickSize { get => TickType.Major == TickType ? 10.0d : 5.0d; }

        public bool Equals(AxisTickModel other)
        {
            if (null == other) return false;
            return Math.Abs((this.OneSize * this.Distance) - (other.OneSize * other.Distance)) < compareMinValue;
        }

        public override int GetHashCode()
        {
            int h0 = this.OneSize.GetHashCode();
            int h1 = this.Distance.GetHashCode();
            return h0 ^ h1;
        }
    }

}
