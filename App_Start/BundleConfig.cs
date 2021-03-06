﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

public class BundleConfig
{
    // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
    public static void RegisterBundles(BundleCollection bundles)
    {
        //bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
        //            "~/Scripts/jquery-{version}.js"));

        //bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
        //            "~/Scripts/jquery.validate*"));

        //bundles.Add(new ScriptBundle("~/bundles/ajax").Include(
        //            "~/Scripts/jquery.unobtrusive*"));


        //bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
        //           "~/Scripts/jquery-ui-{version}.js"));


        // Use the development version of Modernizr to develop with and learn from. Then, when you're
        // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
        //bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
        //            "~/Scripts/modernizr-*"));

        //bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
        //          "~/Scripts/bootstrap.js",
        //          "~/Scripts/bootstrap.min.js",
        //          "~/Scripts/respond.js"));

        //bundles.Add(new StyleBundle("~/Content/css").Include(
        //    "~/Content/myCss/weathers.css",
        //    "~/Content/bootstrap.css",
        //          "~/Content/bootstrap.min.css",
        //          "~/Content/landing-page.css",
        //          "~/Content/site.css",
        //          "~/Content/font-awesome/css/font-awesome.min.css"));

        bundles.Add(new StyleBundle("~/Content/css").Include(
                  "~/Content/Site.css"));



        // Set EnableOptimizations to false for debugging. For more information,
        // visit http://go.microsoft.com/fwlink/?LinkId=301862
        BundleTable.EnableOptimizations = true;
    }
}

