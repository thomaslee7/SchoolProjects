using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Graphics;

namespace FitConnectApp.Services
{
    public class FontManager
    {
        public static string ROOT = "fonts/";
        public static string FONTAWESOME = ROOT + "fontawesome-webfont.ttf";

        public static Typeface getTypeface(Context context, String font)
        {
            return Typeface.CreateFromAsset(context.Assets, font);
        }
        public static void markAsIconContainer(View v, Typeface typeface)
        {
            if (v is ViewGroup) {
                ViewGroup vg = (ViewGroup)v;
                for (int i = 0; i < vg.ChildCount; i++)
                {
                    View child = vg.GetChildAt(i);
                    markAsIconContainer(child, typeface);
                }
            } else if (v is TextView) {
                ((TextView)v).SetTypeface(typeface, TypefaceStyle.Normal);
            }
        }
    }

}