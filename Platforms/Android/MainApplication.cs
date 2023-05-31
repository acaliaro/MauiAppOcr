using Android.App;
using Android.Runtime;

namespace MauiApp6;

[Application]
[MetaData("com.google.android.gms.vision.DEPENDENCIES", Value = "ocr")]
public class MainApplication : MauiApplication
{
	public MainApplication(IntPtr handle, JniHandleOwnership ownership)
		: base(handle, ownership)
	{
	}

	protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
}
