using System.Web;
using System.Web.Optimization;

namespace SystemSetup
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            //--Javascript file--
            bundles.IgnoreList.Clear();

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(                        
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/dataTables").Include(
                        "~/Scripts/jquery.dataTables.js",
                        "~/Scripts/dataTables.fixedColumns.js",
                        "~/Scripts/fnStandingRedraw.js",
                        "~/Scripts/iseiQ.dataTables.js"));

            bundles.Add(new ScriptBundle("~/bundles/input").Include(
                        "~/Scripts/jquery.numeric.js",
                        "~/Scripts/iseiQ.numeric.js",
                        "~/Scripts/datepicker-ja.js",
                        "~/Scripts/iseiQ.datepicker.js",
                        //"~/Scripts/jquery.min.js",
                        "~/Scripts/jquery.bxslider.min.js",
                         "~/Scripts/zxcvbn.js",
                        "~/Scripts/iseiQ.Utility.js"));

            bundles.Add(new ScriptBundle("~/bundles/jquery/bootstrapPlugins").Include(
                        "~/Scripts/bootstrap.min.js",
                        "~/Scripts/plugins/datepicker/bootstrap-datepicker.js",
                        "~/Scripts/plugins/datepicker/locales/bootstrap-datepicker.ja.js",
                        "~/Scripts/plugins/bootstrap-dialog.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/loadingPlugin").Include(
                       "~/Scripts/pace.js"));

            bundles.Add(new ScriptBundle("~/bundles/common").Include(
                        "~/Scripts/slidebars.js",
                        "~/Scripts/thickbox.js",
                        "~/Scripts/iseiQ.CmnStringUtil.js",
                        "~/Scripts/bootstrap-typeahead.js",
                        "~/Scripts/jquery.mockjax.js",
                        "~/Scripts/kickstart.js",
                        "~/Scripts/iseiQ.CmnEventUtil.js",
                        "~/Scripts/jquery.cookie.js"));

            bundles.Add(new ScriptBundle("~/bundles/estimateRegister").Include(
                       "~/Scripts/iseiQ.EstimateRegister.js"));

            bundles.Add(new ScriptBundle("~/bundles/contractRegister").Include(
                       "~/Scripts/iseiQ.ContractSES.js",
                       "~/Scripts/iseiQ.ContractStaff.js",
                       "~/Scripts/iseiQ.ContractContract.js",
                       "~/Scripts/iseiQ.ContractDelegation.js",
                       "~/Scripts/iseiQ.ContractPackage.js",
                       "~/Scripts/iseiQ.ContractProductSales.js",
                       "~/Scripts/iseiQ.ContractMaintain.js",
                       "~/Scripts/iseiQ.ContractProductSalesMonth.js"));

            bundles.Add(new ScriptBundle("~/bundles/estimateView").Include(
                       "~/Scripts/iseiQ.EstimateView.js"));

            bundles.Add(new ScriptBundle("~/bundles/jquery/plugins/jstreeJS").Include("~/Scripts/plugins/jstree.js"));

            bundles.Add(new ScriptBundle("~/bundles/jquery/plugins/dragOn").Include(
                "~/Scripts/plugins/dragOn.js"
                , "~/Scripts/plugins/barOn.js"));

            //--CSS--
            bundles.Add(new StyleBundle("~/Content/themes/iseiQ/css").Include(
                "~/Content/themes/iseiQ/default.style.css",
                "~/Content/themes/iseiQ/example-styles.css",
                "~/Content/themes/iseiQ/iseiQ.css",
                "~/Content/themes/iseiQ/PasswordReissue.css",
                "~/Content/themes/iseiQ/slidebars.css"));

            bundles.Add(new StyleBundle("~/Content/common").Include(
                "~/Content/thickbox.css",
                "~/Content/jquery-ui.css",
                "~/Content/bootstrap.css",
                "~/Content/jqx.base.css",
                "~/Content/jquery.bxslider.css",
                "~/Content/jquery.dataTables.css",
                "~/Content/datepicker.css",
                "~/Content/cm-form.css",
                "~/Content/cm-layout.css",
                "~/Content/jquery.treeview.css"));

            bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
                "~/Content/themes/base/jquery.ui.core.css",
                "~/Content/themes/base/jquery.ui.resizable.css",
                "~/Content/themes/base/jquery.ui.selectable.css",
                "~/Content/themes/base/jquery.ui.accordion.css",
                "~/Content/themes/base/jquery.ui.autocomplete.css",
                "~/Content/themes/base/jquery.ui.button.css",
                "~/Content/themes/base/jquery.ui.dialog.css",
                "~/Content/themes/base/jquery.ui.slider.css",
                "~/Content/themes/base/jquery.ui.tabs.css",
                "~/Content/themes/base/jquery.ui.datepicker.css",
                "~/Content/themes/base/jquery.ui.progressbar.css",
                "~/Content/themes/base/jquery.ui.theme.css"));

            bundles.Add(new StyleBundle("~/Content/fontawesome/css/icon").Include(
                "~/Content/fontawesome/css/font-awesome.css"));

            bundles.Add(new StyleBundle("~/Content/jstree").Include(
                "~/Content/js-tree.css"));
        }
    }
}