using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    [RequireComponent(typeof(ScrollRect))]
    [RequireComponent(typeof(RectTransform))]
    public class UIRecycleViewSystemMyth<T> : MonoBehaviour
    {
        /*[SerializeField] protected List<T> tableData = new List<T>();            // list 항목의 데이터를 저장*/
        [SerializeField] protected GameObject cellBase = null;  // 복사 원본 셀
        [SerializeField] private RectOffset padding;            // 스크롤할 내용의 패딩
        [SerializeField] private float spacingHeight = 4.0f;    // 각 셀의 간격
        [SerializeField] private RectOffset visibleRectPadding = null; // visibleRect의 패딩
        [SerializeField] private MythBox mythBox;

        private LinkedList<UIRecycleViewCell<T>> cells = new LinkedList<UIRecycleViewCell<T>>(); // 셀 저장 리스트

        private Rect visibleRect;               // 리스트 항목을 셀의 형태로 표시하는 범위를 나타내는 사각형
        private Vector2 prevScrollPos;          // 바로 전의 스크롤 위치를 저장

        public RectTransform CachedRectTransform => GetComponent<RectTransform>();
        public ScrollRect CachedScrollRect => GetComponent<ScrollRect>();

        protected virtual void Awake()
        {
            cellBase.SetActive(false);          // 복사 원본 셀은 비활성화
            // CachedScrollRect.onValueChanged.AddListener(OnScrollPosChanged);    // Scroll Rect 컴포넌트 On Value Changed 이벤트의 이벤트 리스너를 설정
        }

        ///<summary>
        //테이블뷰 초기화 함수
        ///</summary>
        protected void InitializeTableView()
        {
            UpdateScrollViewSize();     // 스크롤할 내용의 크기 갱신
            UpdateVisibleRect();        // VisibleRect를 갱신

            Vector2 cellTop = new Vector2(0.0f, -padding.top);
            for (int i = 0; i < GameDataManager.Instance.CombinationDatas.Count; i++)
            {
                float cellHeight = GetCellHeightAtIndex(i);
                Vector2 cellBottom = cellTop + new Vector2(0.0f, -cellHeight);
                if ((cellTop.y <= visibleRect.y && cellTop.y >= visibleRect.y - visibleRect.height) ||
                    (cellBottom.y <= visibleRect.y && cellBottom.y >= visibleRect.y - visibleRect.height))
                {
                    UIRecycleViewCell<T> cell = CreateCellForIndex(GameDataManager.Instance.CombinationDatas[i].targetID);
                    cell.Top = cellTop;
                    // break;
                }
                cellTop = cellBottom + new Vector2(0.0f, spacingHeight);
            }

            // visibleRect의 범위에 빈 곳이 있으면 셀을 작성
            SetFillVisibleRectWithCells();

            SetEpicCombinationProbability();
        }

        ///<summary>
        /// 셀의 높이값을 리턴하는 함수
        /// </summary>
        /// <returns>The cell height at index.</returns>
        /// <param name="index">Index.</param>
        protected virtual float GetCellHeightAtIndex(int index)
        {
            // 실제 값을 반환한느 처리는 상속한 클래스에서 구현
            // 셀마다 크기가 다를 경우, 상속받은 클래스에서 재구현
            return cellBase.GetComponent<RectTransform>().sizeDelta.y;
        }
        ///<summary>
        /// 스크롤할 내용 전체의 높이를 갱신하는 함수
        /// </summary>
        protected void  UpdateScrollViewSize()
        {
            //스크롤할 내용 전체의 높이를 계산
            float contentHeight = 0.0f;
            for (int i = 0; i < GameDataManager.Instance.CombinationDatas.Count; i++)
            {
                contentHeight += GetCellHeightAtIndex(i);
                if (i > 0)
                {
                    contentHeight += spacingHeight;
                }
            }

            //스크롤할 내용의 높이를 설정
            Vector2 sizeDelta = CachedScrollRect.content.sizeDelta;
            sizeDelta.y = padding.top + contentHeight + padding.bottom;
            CachedScrollRect.content.sizeDelta = sizeDelta;
        }

        ///<summary>
        /// 셀을 생성하는 함수
        /// </summary>
        /// <returns>The cell for index.</returns>
        /// <param name="key">Index.</param>
        private UIRecycleViewCell<T> CreateCellForIndex(int key)
        {
            //복사 원본 셀을 이용해 새롤운 셀을 생성

            GameObject obj = Instantiate(cellBase);
            obj.SetActive(true);
            UIRecycleViewCell<T> cell = obj.GetComponent<UIRecycleViewCell<T>>();

            if (cellBase == null)
            {
                Debug.Log("cellBase is null");
                return null;
            }

            if (obj == null)
            {
                Debug.Log("Failed to instantiate cellBase");
                return null;
            }

            if (cell == null)
            {
                Debug.Log("UIRecycleViewCell component is missing on cellBase");
                return null;
            }

            // 부모 요소를 바꾸면 스케일이나 크기를 잃어버리므로 변수에 저장
            Vector3 scale = cell.transform.localScale;
            Vector2 sizeDelta = cell.CachedRectTransform.sizeDelta;
            Vector2 offsetMin = cell.CachedRectTransform.offsetMin;
            Vector2 offsetMax = cell.CachedRectTransform.offsetMax;

            cell.transform.SetParent(cellBase.transform.parent);

            // 셀의 스케일과 크기를 설정
            cell.transform.localScale = scale;
            cell.CachedRectTransform.sizeDelta = sizeDelta;
            cell.CachedRectTransform.offsetMin = offsetMin;
            cell.CachedRectTransform.offsetMax = offsetMax;

            // 지정된 인덱스가 붙은 리스트 항목에 대응하는 셀로 내용을 갱신
/*            UpdateCellForIndex(cell, index);*/

            UIRecycleViewCellMyth btn = cell as UIRecycleViewCellMyth;
            btn.Init(key, mythBox);

            cells.AddLast(cell);

            return cell;
        }

        ///<summary>
        ///VisibleRect를 갱신하기 위한 함수
        /// </summary>
        private void UpdateVisibleRect()
        {
            // visibleRect의 위치는 스크롤할 내용의 기준으로부터 상대적인 위치임
            visibleRect.x = CachedScrollRect.content.anchoredPosition.x + visibleRectPadding.left;
            visibleRect.y = -CachedScrollRect.content.anchoredPosition.y + visibleRectPadding.top;

            // visibleRect의 크기는 스크롤 뷰의 크기 + 패딩
            visibleRect.width = CachedRectTransform.rect.width + visibleRectPadding.left + visibleRectPadding.right;
            visibleRect.height = CachedRectTransform.rect.height + visibleRectPadding.top + visibleRectPadding.bottom;
        }

        ///<summary>
        /// VisubleRect 범위에 표시될 만큼의 셀을 생성하여 배치하는 함수
        /// </summary>
        private void SetFillVisibleRectWithCells()
        {
            // 셀이 없다면 아무 일도 하지 않음
            if (cells.Count < 1)
            {
                return;
            }

            // 표시된 마지막 셀에 대응하는 리스트 항목의 다음 리스트 항목이 있고
            // 또한 그 셀이 visibleRect의 범위에 들어온다면 대응하는 셀을 작성
            UIRecycleViewCell<T> lastCell = cells.Last.Value;
            int nextCellDataIndex = lastCell.Index + 1;
            Vector2 nextCellTop = lastCell.Bottom + new Vector2(0.0f, -spacingHeight);

            while (nextCellDataIndex < GameDataManager.Instance.CombinationDatas.Count && nextCellTop.y >= visibleRect.y - visibleRect.height)
            {
                UIRecycleViewCell<T> cell = CreateCellForIndex(nextCellDataIndex);
                cell.Top = nextCellTop;

                lastCell = cell;
                nextCellDataIndex = lastCell.Index + 1;
                nextCellTop = lastCell.Bottom + new Vector2(0.0f, -spacingHeight);
            }
        }

        public void SetEpicCombinationProbability()
        {
            foreach (var cell in cells)
            {
                (cell as UIRecycleViewCellMyth)?.SetProbablilty();
            }
        }
    }
}

