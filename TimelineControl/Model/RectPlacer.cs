using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace RectanglePlacer.Biz
{
    public class PlacableRect
    {
        public Rect rect;
        public object source;
    }

    /// <summary>
    /// Rectを水平軸上で移動して
    /// それぞれがが重ならないように配置する
    /// </summary>
    public class RectPlacer
    {
        #region variables

        private double _minPos;
        private double _maxPos;

        public List<PlacableRect> placedRects;

        #endregion

        /// <summary>
        /// コンストラクター
        /// </summary>
        /// <param name="minPos">x軸上の最小値</param>
        /// <param name="maxPos">x軸上の最大値</param>
        public RectPlacer(double minPos, double maxPos)
        {
            _minPos = minPos;
            _maxPos = maxPos;
        }

        #region methods

        /// <summary>
        /// 2つのRectを比較して同じ緯線上に存在しないことを確認する
        /// </summary>
        /// <param name="rect1">比較対象の1</param>
        /// <param name="rect2">比較対象の2</param>
        /// <returns>同じ緯線上に存在する場合はtrue</returns>
        private bool IsConflictRectVertical(ref Rect rect1, ref Rect rect2)
        {
            if (rect1.Y <= rect2.Y && rect1.Y + rect1.Height >= rect2.Y)
            {
                return true;
            }

            if (rect2.Y <= rect1.Y && rect2.Y + rect2.Height >= rect1.Y)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 2つのRectを比較して同じ経線上に存在しないことを確認する
        /// </summary>
        /// <param name="rect1">比較対象の1</param>
        /// <param name="rect2">比較対象の2</param>
        /// <returns>同じ経線上に存在する場合はtrue</returns>
        private bool IsConflictRectHorizontal(ref Rect rect1, ref Rect rect2)
        {
            if (rect1.X <= rect2.X && rect1.X + rect1.Width > rect2.X)
            {
                return true;
            }

            if (rect2.X <= rect1.X && rect2.X + rect2.Width > rect1.X)
            {
                return true;
            }

            return false;
        }


        /// <summary>
        /// 2つのRectを比較してコンフリクトする可能性のあるRectのリストを返す
        /// </summary>
        /// <param name="placable">判定対象のRect</param>
        /// <param name="cmpList">コンフリクト判定するRectのリスト</param>
        /// <returns>コンフリクト可能性のあるRectのリスト</returns>
        private List<PlacableRect> GetConflictRects(PlacableRect placable, List<PlacableRect> cmpList)
        {
            List<PlacableRect> retRects = new List<PlacableRect>();
            if (placable.rect.Height == 0)
            {
                return retRects;
            }

            foreach (var rect in cmpList)
            {
                if (IsConflictRectVertical(ref placable.rect, ref rect.rect))
                {
                    if (rect.rect.Height == 0)
                    {
                        continue;
                    }
                    retRects.Add(rect);
                }
            }

            return retRects;
        }

        /// <summary>
        /// Rectの理論上の最大幅を設定する
        /// </summary>
        /// <param name="list">設定するRectのリスト</param>
        private void SetMaxWidth(List<PlacableRect> list)
        {
            double targetWidth = _maxPos - _minPos;

            foreach (var rect in list)
            {
                int conflictCount = GetConflictRects(rect, list).Count();

                if (conflictCount == 0)
                {
                    rect.rect.Width = targetWidth;
                }
                else
                {
                    rect.rect.Width = targetWidth / conflictCount;
                }
            }
        }

        /// <summary>
        /// コンフリクトを起こさないWidth幅を検査して返す
        /// </summary>
        /// <param name="current">座標上のポジション</param>
        /// <param name="rectList">コンフリクト判定をするリスト</param>
        /// <returns>コンフリクトを起こさない程度の幅</returns>
        private double FindUsableWidth(double current, List<PlacableRect> rectList)
        {
            double maxRight = _maxPos;

            foreach (var rect in rectList)
            {
                if (current > rect.rect.X)
                {
                    continue;
                }

                if (maxRight > rect.rect.X)
                {
                    maxRight = rect.rect.X;
                }
            }

            return maxRight - current;
        }

        /// <summary>
        /// 元のWidthを無視して、設定しうるRect領域を検索して設定する
        /// </summary>
        /// <param name="rect">設定先のRect</param>
        /// <param name="rectList">コンフリクト判定をするリスト</param>
        private void FindSpaceAndSetRect(ref Rect rect, List<PlacableRect> rectList)
        {
            // 既にWidthが0ならどうしようもない
            if (rect.Width == 0)
            {
                return;
            }

            rect.X = _minPos;
            rect.Width = 0;

            SetPlacableRect(ref rect, rectList);

            rect.Width = FindUsableWidth(rect.X, rectList);
        }


        /// <summary>
        /// 設定しうるRect領域を検索して設定する
        /// </summary>
        /// <param name="rect">設定先のRect</param>
        /// <param name="rectList">コンフリクト判定をするリスト</param>
        private void SetPlacableRect(ref Rect rect, List<PlacableRect> rectList)
        {
            foreach (var conf in rectList)
            {
                if (!IsConflictRectHorizontal(ref rect, ref conf.rect))
                {
                    continue;
                }

                rect.X = conf.rect.X + conf.rect.Width;

                // 現状のサイズではスペースが見つからない場合はwidthを0にしてスペースを探しにいく
                if (rect.X + rect.Width > _maxPos)
                {
                    FindSpaceAndSetRect(ref rect, rectList);
                }
                else
                {
                    SetPlacableRect(ref rect, rectList);
                }
            }
        }

        /// <summary>
        /// X軸上のポイントを設定する
        /// </summary>
        /// <param name="rect">設定したいRect</param>
        private void SetLeftOneRect(PlacableRect rect)
        {
            var conflicts = GetConflictRects(rect, placedRects);

            rect.rect.X = _minPos;

            SetPlacableRect(ref rect.rect, conflicts);
        }

        /// <summary>
        /// 引数のRectを干渉しないようにX軸上に移動して設定する
        /// </summary>
        /// <param name="targetRects">設定したいRectのリスト</param>
        public void Place(List<PlacableRect> targetRects)
        {
            // 最長widthを確定
            SetMaxWidth(targetRects);

            placedRects = new List<PlacableRect>();

            foreach (var rect in targetRects)
            {
                SetLeftOneRect(rect);
                placedRects.Add(rect);
            }
        }

        #endregion
    }
}
