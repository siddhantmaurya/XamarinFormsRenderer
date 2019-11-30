using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

using Android.App;
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

[assembly: ExportRenderer(typeof(RoundedButton), typeof(RoundButtonRenderer))]
namespace XamarinFormsRenderers.Droid.Renderers
{
    public class RoundButtonRenderer : ButtonRenderer
    {
        ButtonDrawable _backgroundDrawable;
        Drawable _defaultDrawable;
        bool _drawableEnabled;

        RoundButtonRenderer(Context context) : base(context)
        {
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_backgroundDrawable != null)
                {
                    _backgroundDrawable.Dispose();
                    _backgroundDrawable = null;
                }
            }

            base.Dispose(disposing);
        }


        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Button> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null && _drawableEnabled)
            {
                _drawableEnabled = false;
                _backgroundDrawable.Reset();
                _backgroundDrawable = null;
            }
            UpdateDrawable();
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (_drawableEnabled &&
                (e.PropertyName == VisualElement.BackgroundColorProperty.PropertyName || e.PropertyName == Xamarin.Forms.Button.BorderColorProperty.PropertyName || e.PropertyName == Xamarin.Forms.Button.BorderRadiusProperty.PropertyName ||
                e.PropertyName == Xamarin.Forms.Button.BorderWidthProperty.PropertyName))
            {
                _backgroundDrawable.Reset();
                Control.Invalidate();
            }

            base.OnElementPropertyChanged(sender, e);
        }

        private void UpdateDrawable()
        {
            if (Element.BackgroundColor == Color.Default)
            {
                if (!_drawableEnabled)
                    return;

                if (_defaultDrawable != null)
                    Control.SetBackground(_defaultDrawable);

                _drawableEnabled = false;
            }
            else
            {
                if (_backgroundDrawable == null)
                    _backgroundDrawable = new ButtonDrawable();

                _backgroundDrawable.Button = Element;

                if (_drawableEnabled)
                    return;

                if (_defaultDrawable == null)
                    _defaultDrawable = Control.Background;

                Control.SetBackground(_backgroundDrawable.GetDrawable());
                _drawableEnabled = true;
            }

            Control.Invalidate();
        }
    }


    public class ButtonDrawable : IDisposable
    {
        object _backgroundDrawable;

        PropertyInfo ButtonProperty;
        public Xamarin.Forms.Button Button
        {
            get
            {
                return (Xamarin.Forms.Button)ButtonProperty.GetMethod.Invoke(_backgroundDrawable, null);
            }
            set
            {
                ButtonProperty.SetMethod.Invoke(_backgroundDrawable, new object[] { value });
            }
        }

        public ButtonDrawable()
        {
            _backgroundDrawable = typeof(Xamarin.Forms.Platform.Android.ButtonRenderer).Assembly.CreateInstance("Xamarin.Forms.Platform.Android.ButtonDrawable");
            this.ResetMethod = _backgroundDrawable.GetType().GetMethod("Reset", BindingFlags.Instance | BindingFlags.Public);
            this.DisposeMethod = _backgroundDrawable.GetType().GetMethod("Dispose", BindingFlags.Instance | BindingFlags.Public);
            this.ButtonProperty = _backgroundDrawable.GetType().GetProperty("Button", BindingFlags.Instance | BindingFlags.Public);
        }

        MethodInfo ResetMethod;
        public void Reset()
        {
            ResetMethod.Invoke(_backgroundDrawable, null);
        }

        MethodInfo DisposeMethod;
        public void Dispose()
        {
            DisposeMethod.Invoke(_backgroundDrawable, null);
        }

        public Android.Graphics.Drawables.Drawable GetDrawable()
        {
            return _backgroundDrawable as Android.Graphics.Drawables.Drawable;
        }
    }
}