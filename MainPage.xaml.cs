#if ANDROID
using Android.Content;
#endif

namespace MauiApp6;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
    }

    private void OnCounterClicked(object sender, EventArgs e)
    {
        Task.Run(async () =>
        {

            await MainThread.InvokeOnMainThreadAsync(async () =>
            {
                try
                {
                    await RequestPermissionAsync();

#if ANDROID
                    Intent intent = new Intent(Platform.CurrentActivity.Application.ApplicationContext, typeof(Platforms.Android.OcrActivity));
                    intent.SetFlags(ActivityFlags.NewTask);

                    Platform.CurrentActivity.Application.ApplicationContext.StartActivity(intent);
#endif
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }
            });
        });
    }

    private async Task RequestPermissionAsync()
    {
        var cameraStatus = await Permissions.CheckStatusAsync<Permissions.Camera>(); //.CheckPermissionStatusAsync(Permission.Camera);
                                                                                     //var storageStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Storage);
        if (cameraStatus != PermissionStatus.Granted /*|| storageStatus != PermissionStatus.Granted*/)
        {
            cameraStatus = await Permissions.RequestAsync<Permissions.Camera>();

            //var results = await CrossPermissions.Current.RequestPermissionsAsync(new[] { Permission.Camera/*, Permission.Storage*/ });
            //cameraStatus = results[Permission.Camera];
            //storageStatus = results[Permission.Storage];
        }

        if (cameraStatus != PermissionStatus.Granted /*&& storageStatus == PermissionStatus.Granted*/)
        {

            await Application.Current.MainPage.DisplayAlert("Error", "Grant camera permission", "Ok");
        }
    }
}

