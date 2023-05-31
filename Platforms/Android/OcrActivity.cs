using Android.App;
using Android.Gms.Vision.Texts;
using Android.Gms.Vision;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Android.Graphics;
using System.Text;
using static Android.Gms.Vision.Detector;
using Android.Content.PM;
using CommunityToolkit.Mvvm.Messaging;

namespace MauiApp6.Platforms.Android;

[Activity(Label = "OcrActivity",
       ScreenOrientation = ScreenOrientation.Portrait,
       MainLauncher = false)]
public class OcrActivity : Activity, ISurfaceHolderCallback, IProcessor
{
    private SurfaceView cameraView;
    private TextView textViewOcr;
    private CameraSource cameraSource;

    public static string TAG = "OcrActivity";

    //public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
    //{
    //    switch (requestCode)
    //    {
    //        case RequestCameraPermissionID:
    //            {
    //                if (grantResults[0] == Permission.Granted)
    //                {
    //                    cameraSource.Start(cameraView.Holder);
    //                }
    //            }
    //            break;
    //    }
    //}

    protected override void OnCreate(Bundle savedInstanceState)
    {
        base.OnCreate(savedInstanceState);


        // Set our view from the "main" layout resource  
        SetContentView(Resource.Layout.Ocr);
        cameraView = FindViewById<SurfaceView>(Resource.Id.surface_view);
        textViewOcr = FindViewById<TextView>(Resource.Id.text_view_ocr);

        TextRecognizer txtRecognizer = new TextRecognizer.Builder(ApplicationContext).Build();
        if (!txtRecognizer.IsOperational)
            Log.Error(TAG, "OCR resources not loaded");
        else
        {
            cameraSource = new CameraSource.Builder(ApplicationContext, txtRecognizer).SetFacing(CameraFacing.Back).SetRequestedPreviewSize(1280, 1024).SetRequestedFps(2.0f).SetAutoFocusEnabled(true).Build();
            cameraView.Holder.AddCallback(this);
            txtRecognizer.SetProcessor(this);
        }
    }

    public void SurfaceChanged(ISurfaceHolder holder, [GeneratedEnum] Format format, int width, int height) { }

    public void SurfaceCreated(ISurfaceHolder holder)
    {
        cameraSource.Start(cameraView.Holder);
    }

    public void SurfaceDestroyed(ISurfaceHolder holder)
    {
        cameraSource.Stop();
    }

    public void ReceiveDetections(Detections detections)
    {
        SparseArray items = detections.DetectedItems;
        if (items.Size() != 0)
        {
            textViewOcr.Post(() => {

                for (int i = 0; i < items.Size(); ++i)
                {
                    // Gestisco la verifica del dato
                    string testo = ((TextBlock)items.ValueAt(i)).Value.ToUpper();

                    textViewOcr.Text = testo;

                }

            });
        }
    }

    public void Release() { }
}