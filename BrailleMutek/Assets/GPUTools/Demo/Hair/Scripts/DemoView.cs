using UnityEngine;
using UnityEngine.UI;

#pragma warning disable 649

namespace GPUTools.HairDemo.Scripts
{
    public class DemoView : MonoBehaviour
    {
        [SerializeField] private Button play;
        [SerializeField] private Button stop;
        [SerializeField] private Button next;
        [SerializeField] private Button prev;
        [SerializeField] private Button rotate;

        [SerializeField] private GameObject[] styles;

        [SerializeField] private ConstantRotation rotation;

        private int currentStyleIndex;

        private void Start ()
        {
            SetStartStyle();

	        play.onClick.AddListener(OnClickPlay);
	        stop.onClick.AddListener(OnClickStop);
	        next.onClick.AddListener(OnClickNext);
	        prev.onClick.AddListener(OnClickPrev);
	        rotate.onClick.AddListener(OnClickRotate);
        }

        private void OnClickRotate()
        {
            rotation.Speed += 200;

            if (rotation.Speed >= 800)
            {
                rotation.Speed = 0;
            }
            
        }

        private void OnClickPrev()
        {
            CurrentStyleIndex--;
        }

        private void OnClickNext()
        {
            CurrentStyleIndex++;
        }

        private void OnClickStop()
        {
            CurrentStyle.GetComponent<Animator>().enabled = false;
        }

        private void OnClickPlay()
        {
            CurrentStyle.GetComponent<Animator>().enabled = true;
        }

        private GameObject CurrentStyle
        {
            get { return styles[currentStyleIndex]; }
        }

        private int CurrentStyleIndex
        {
            set
            {
                currentStyleIndex = value;

                if(currentStyleIndex < 0)
                    currentStyleIndex = styles.Length - 1;
                if (currentStyleIndex > styles.Length - 1)
                    currentStyleIndex = 0;

                ApplyStyle();
            }
            get { return currentStyleIndex; }
        }

        private void ApplyStyle()
        {
            for (var i = 0; i < styles.Length; i++)
            {
                var style = styles[i];
                style.SetActive(i == currentStyleIndex);
            }
        }

        private void SetStartStyle()
        {
            for (var i = 0; i < styles.Length; i++)
            {
                var style = styles[i];
                if (style.activeSelf)
                    CurrentStyleIndex = i;
            }
        }
    }
}
