using Android.App;

namespace AndroidBluetoothLE
{
    public static class DialogView
    {
        private static Dialog _currentDialog;

        public static void ShowDialog(string message, Activity currentActivity)
        {
            currentActivity.RunOnUiThread(() => LocalShowDialog(message, currentActivity));
        }

        public static void CloseDialog(Activity currentActivity)
        {
            currentActivity.RunOnUiThread(() =>
            {
                if (_currentDialog != null && _currentDialog.IsShowing)
                {
                    _currentDialog.Dismiss();
                }
            });
            
        }

        private static void LocalShowDialog(string message, Activity currentActivity)
        {
            CloseDialog(currentActivity);

            var builder = new AlertDialog.Builder(currentActivity);
            builder.SetMessage(message);

            _currentDialog = builder.Create();
            _currentDialog.Show();
        }
    }
}