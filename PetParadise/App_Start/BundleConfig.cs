using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace PetParadise.App_Start
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                "~/Scripts/jquery.unobtrusive*",
                "~/Scripts/jquery.validate*"));


            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/uikit").Include(
                "~/Scripts/uikit.js",
                "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                 "~/Content/uikit.css",
                 "~/Content/bootstrap.css",
                 "~/Content/Site.css",
                 "~/Content/fontawesome.css"));

            // My custom scripts

            bundles.Add(new ScriptBundle("~/bundles/helpers").Include(
                "~/Scripts/access.js",
                "~/Scripts/ValidationHandler/Validator.js",
                "~/Scripts/ValidationHandler/InvalidObject.js"));

            //the following creates bundles in debug mode;
            BundleTable.EnableOptimizations = true;
        }
    }
}