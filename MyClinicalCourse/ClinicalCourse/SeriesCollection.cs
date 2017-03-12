using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyClinicalCourse.ClinicalCourse
{
    /// <summary>
    /// シリーズのコレクションクラス
    /// </summary>
    public class SeriesCollection : List<Series>
    {
        /// <summary>
        /// 空で、規定の初期量を備えた、Seriesクラスの新しいインスタンスを初期化します。
        /// </summary>
        public SeriesCollection() : base() { }

        /// <summary>
        /// 空で、指定した初期量を備えた、Seriesクラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="capacity">初期量</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public SeriesCollection(int capacity) : base(capacity) { }

        /// <summary>
        /// 指定したコレクションからコピーした要素を格納し、コピーされる要素の数を格納できるだけの容量を備えた、Seriesクラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="collection">コピー元コレクション</param>
        /// <exception cref="ArgumentNullException"></exception>
        public SeriesCollection(IEnumerable<Series> collection) : base(collection) { }
    }
}
