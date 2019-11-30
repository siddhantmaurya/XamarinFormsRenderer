using Android.Content;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using XamarinFormsRenderers.CustomRendererControls;
using XamarinFormsRenderers.Droid.Renderers;

[assembly:ExportRenderer(typeof(RoundedEntry),typeof(RoundedEntryRenderer))]
namespace XamarinFormsRenderers.Droid.Renderers
{
    public class RoundedEntryRenderer : EntryRenderer
    {
        public RoundedEntryRenderer(Context context) : base(context)
        {
        }
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);
            if (Control != null)
            {
                var gradientDrawable = new GradientDrawable();
                gradientDrawable.SetCornerRadius(60f);
                gradientDrawable.SetStroke(5, Android.Graphics.Color.LightSeaGreen);
                gradientDrawable.SetColor(Android.Graphics.Color.LightGray);

                Control.SetBackground(gradientDrawable);
            }
        }
    }
}