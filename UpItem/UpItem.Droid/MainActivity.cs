//using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Content;
using Android.Provider;
using Android.Net;
using Xamarin.Forms;

using Java.IO;
using Android.Database;
using Android.Graphics;
using System.IO;
using System;

namespace UpItem.Droid {
    [Activity(Label = "UpItem", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity {
        App app;
        protected override void OnCreate(Bundle bundle) {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);

            LoadApplication(new App());
        }

        // 別インスタンスから復旧
        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data) {
            base.OnActivityResult(requestCode, resultCode, data);

            //リクエストコードの判定（カメラ・フォトギャラリーかどうか）
            if (requestCode == 1) {
                if (resultCode == Result.Ok) {
                    if (data.Data != null) {
                        //画像のパスを取得
                        Android.Net.Uri uri = data.Data;

                        //画像のメタデータをもとに画像の向きを確定する
                        int orientation = getOrientation(uri);

                        //バックグラウンドでbitmapの準備をする
                        BitmapWorkerTask task = new BitmapWorkerTask(this.ContentResolver, uri);
                        task.Execute(orientation);
                    }
                }
            }
        }

        // 画像の向きを確定
        public int getOrientation(Android.Net.Uri photoUri) {
            ICursor cursor = Application.ApplicationContext.ContentResolver.Query(photoUri, new String[] { MediaStore.Images.ImageColumns.Orientation }, null, null, null);

            if (cursor.Count != 1) {
                return -1;
            }

            cursor.MoveToFirst();
            return cursor.GetInt(0);
        }

        // Bitmap処理
        public class BitmapWorkerTask : AsyncTask<int, int, Bitmap> {
            private Android.Net.Uri uriReference;
            private int data = 0;
            private ContentResolver resolver;

            public BitmapWorkerTask(ContentResolver cr, Android.Net.Uri uri) {
                uriReference = uri;
                resolver = cr;
            }

            // 画像データのデコード
            protected override Bitmap RunInBackground(params int[] p) {
                //画像の向き
                data = p[0];

                Bitmap mBitmap = Android.Provider.MediaStore.Images.Media.GetBitmap(resolver, uriReference);
                Bitmap myBitmap = null;

                if (mBitmap != null) {
                    //画像の向きを設定する
                    Matrix matrix = new Matrix();
                    if (data != 0) {
                        matrix.PreRotate(data);
                    }

                    myBitmap = Bitmap.CreateBitmap(mBitmap, 0, 0, mBitmap.Width, mBitmap.Height, matrix, true);
                    return myBitmap;
                }

                return null;
            }

            //終了時処理
            protected override void OnPostExecute(Bitmap bitmap) {
                if (bitmap != null) {
                    MemoryStream stream = new MemoryStream();

                    //画像の圧縮（圧縮率５０）
                    bitmap.Compress(Bitmap.CompressFormat.Jpeg, 50, stream);
                    byte[] bitmapData = stream.ToArray();

                    //画像データをUI側に渡す
                    MessagingCenter.Send<byte[]>(bitmapData, "ImageSelected");

                    //メモリ上からデータを破棄
                    bitmap.Recycle();
                    GC.Collect();
                }
            }
        }
    }
}

