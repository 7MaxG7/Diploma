﻿using TMPro;
using UnityEngine;


namespace UI {

	internal sealed class PermanentUiView : MonoBehaviour, IPermanentUiView {
		[SerializeField] private Canvas _permanentCanvas;
		[SerializeField] private CanvasGroup _loadingCurtainCanvasGroup;
		[SerializeField] private TMP_Text _loadingCurtainText;

		public GameObject GameObject => gameObject;
		public Canvas PermanentCanvas => _permanentCanvas;
		public CanvasGroup LoadingCurtainCanvasGroup => _loadingCurtainCanvasGroup;
		public TMP_Text LoadingCurtainText => _loadingCurtainText;
	}

}