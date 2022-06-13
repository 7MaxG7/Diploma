using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace Infrastructure {

	internal class LoginPanelView : MonoBehaviour {
		[SerializeField] private CanvasGroup _canvasGroup;
		[SerializeField] private TMP_InputField _usernameInputField;
		[SerializeField] private TMP_InputField _passwordInputField;
		[SerializeField] private TMP_InputField _emailInputField;
		[SerializeField] private CanvasGroup _emailInputFieldCanvasGroup;
		[SerializeField] private Toggle _emailToggleSwitcher;
		[SerializeField] private CanvasGroup _emailToggleSwitcherLabelCanvasGroup;
		[SerializeField] private Button _confirmButton;
		[SerializeField] private Button _closePanelButton;
		[SerializeField] private TMP_Text _confirmButtonText;
		[SerializeField] private TMP_Text _statusLableText;

		public CanvasGroup CanvasGroup => _canvasGroup;
		public TMP_InputField UsernameInputField => _usernameInputField;
		public TMP_InputField PasswordInputField => _passwordInputField;
		public TMP_InputField EmailInputField => _emailInputField;
		public CanvasGroup EmailInputFieldCanvasGroup => _emailInputFieldCanvasGroup;
		public Toggle EmailToggleSwitcher => _emailToggleSwitcher;
		public CanvasGroup EmailToggleSwitcherLabelCanvasGroup => _emailToggleSwitcherLabelCanvasGroup;
		public Button ConfirmButton => _confirmButton;
		public Button ClosePanelButton => _closePanelButton;
		public TMP_Text ConfirmButtonText => _confirmButtonText;
		public TMP_Text StatusLableText => _statusLableText;
	}

}