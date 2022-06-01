namespace Infrastructure {

	internal interface IDisposer : IController {
		public void OnDispose();
	}

}