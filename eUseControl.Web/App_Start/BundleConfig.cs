using System.Web.Optimization;

namespace eUseControl.Web.App_Start
{
    public static class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/bundles/main/css").Include("~/Content/Site.css", new CssRewriteUrlTransform()));

            bundles.Add(new StyleBundle("~/bundles/style/css").Include("~/Assets/css/style.css", new CssRewriteUrlTransform()));

            bundles.Add(new StyleBundle("~/bundles/lightbox/css").Include("~/Assets/lib/lightbox/css/lightbox.min.css", new CssRewriteUrlTransform()));

            bundles.Add(new StyleBundle("~/bundles/owl/carousel/css").Include("~/Assets/lib/owlcarousel/assets/owl.carousel.css", new CssRewriteUrlTransform()));

            bundles.Add(new StyleBundle("~/bundles/owl/carousel/min/css").Include("~/Assets/lib/owlcarousel/assets/owl.carousel.min.css", new CssRewriteUrlTransform()));

            bundles.Add(new StyleBundle("~/bundles/owl/theme/default/css").Include("~/Assets/lib/owlcarousel/assets/owl.theme.default.css", new CssRewriteUrlTransform()));

            bundles.Add(new StyleBundle("~/bundles/owl/theme/default/min/css").Include("~/Assets/lib/owlcarousel/assets/owl.theme.default.min.css", new CssRewriteUrlTransform()));

            bundles.Add(new StyleBundle("~/bundles/owl/theme/green/css").Include("~/Assets/lib/owlcarousel/assets/owl.theme.green.css", new CssRewriteUrlTransform()));

            bundles.Add(new StyleBundle("~/bundles/owl/theme/green/min/css").Include("~/Assets/lib/owlcarousel/assets/owl.theme.green.min.css", new CssRewriteUrlTransform()));

            bundles.Add(new StyleBundle("~/bundles/login/css").Include("~/Assets/css/login.css", new CssRewriteUrlTransform()));

            bundles.Add(new StyleBundle("~/bundles/payment/css").Include("~/Assets/css/payment.css", new CssRewriteUrlTransform()));

            bundles.Add(new StyleBundle("~/bundles/chat/css").Include("~/Assets/css/chat.css", new CssRewriteUrlTransform()));

            bundles.Add(new StyleBundle("~/bundles/product-ui/css").Include("~/Assets/css/product-ui.css", new CssRewriteUrlTransform()));

            bundles.Add(new StyleBundle("~/bundles/legal/css").Include("~/Assets/css/legal.css", new CssRewriteUrlTransform()));

            bundles.Add(new StyleBundle("~/bundles/form-errors/css").Include("~/Assets/css/form-errors.css", new CssRewriteUrlTransform()));

            bundles.Add(new StyleBundle("~/bundles/layout/css").Include("~/Assets/css/layout.css", new CssRewriteUrlTransform()));

            bundles.Add(new StyleBundle("~/bundles/order-confirmation/css").Include("~/Assets/css/order-confirmation.css", new CssRewriteUrlTransform()));

            bundles.Add(new StyleBundle("~/bundles/order-failure/css").Include("~/Assets/css/order-failure.css", new CssRewriteUrlTransform()));

            bundles.Add(new StyleBundle("~/bundles/user-products/css").Include("~/Assets/css/user-products.css", new CssRewriteUrlTransform()));

            bundles.Add(new StyleBundle("~/bundles/change-password/css").Include("~/Assets/css/change-password.css", new CssRewriteUrlTransform()));

            bundles.Add(new StyleBundle("~/bundles/account-profile/css").Include("~/Assets/css/account-profile.css", new CssRewriteUrlTransform()));

            bundles.Add(new StyleBundle("~/bundles/customer-orders/css").Include("~/Assets/css/customer-orders.css", new CssRewriteUrlTransform()));

            bundles.Add(new StyleBundle("~/bundles/profile-settings/css").Include("~/Assets/css/profile-settings.css", new CssRewriteUrlTransform()));

            bundles.Add(new StyleBundle("~/bundles/success-message/css").Include("~/Assets/css/success-message.css", new CssRewriteUrlTransform()));

            bundles.Add(new ScriptBundle("~/bundles/account-settings/js").Include("~/Assets/js/account-settings.js"));

            bundles.Add(new ScriptBundle("~/bundles/cart-actions/js").Include("~/Assets/js/cart-actions.js"));

            bundles.Add(new ScriptBundle("~/bundles/animations/js").Include("~/Assets/js/animations.js"));

            bundles.Add(new ScriptBundle("~/bundles/main/js").Include("~/Assets/js/main.js"));

            bundles.Add(new ScriptBundle("~/bundles/easing/js").Include("~/Assets/lib/easing/easing.js"));

            bundles.Add(new ScriptBundle("~/bundles/easing/min/js").Include("~/Assets/lib/easing/easing.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/lightbox/js").Include("~/Assets/lib/lightbox/js/lightbox.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/owl/carousel/js").Include("~/Assets/lib/owlcarousel/owl.carousel.js"));

            bundles.Add(new ScriptBundle("~/bundles/owl/carousel/min/js").Include("~/Assets/lib/owlcarousel/owl.carousel.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/waypoints/js").Include("~/Assets/lib/waypoints/waypoints.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/payment/js").Include("~/Assets/js/payment.js"));
        }
    }
}