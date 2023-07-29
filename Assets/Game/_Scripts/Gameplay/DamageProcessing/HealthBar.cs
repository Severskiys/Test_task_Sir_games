using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace Gameplay.DamageProcessing
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private Image _fillableImage;
        [SerializeField] private Image _fillableBackImage;

        private float _maxHp;
        
        public void Init(float maxHp)
        {
            _maxHp = maxHp;
            _fillableImage.fillAmount = 1.0f;
            _fillableBackImage.fillAmount = 1.0f;
            _canvasGroup.Show(0.25f);
        }

        public void ChangeValue(float currentHp, float timer)
        {
            float targetFillAmount = Mathf.Clamp01(currentHp / _maxHp);
            _fillableImage.fillAmount = targetFillAmount;
            _fillableBackImage.DOFillAmount(targetFillAmount, timer).SetEase(Ease.InQuart);
        }

        public void Hide()
        {
            _canvasGroup.Hide();
        }

        private void Awake() => Hide();
    }
}