﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using Ext.Net.MVC;
using Ext.Net;
using ExtHardwareMaintenance.Models;

namespace ExtHardwareMaintenance.Controllers
{
    [DirectController]
    public class MainController : System.Web.Mvc.Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Home()
        {
            return View();
        }

        [OutputCache(Duration = 3600)]
        public ActionResult GetExamplesNodes(string node)
        {
            if (node == "Root")
            {
                return this.Content(ExamplesModel.GetExamplesNodes().ToJson());
            }

            return new HttpStatusCodeResult((int)HttpStatusCode.BadRequest);
        }

        [DirectMethod]
        [OutputCache(Duration = 86400, VaryByParam = "theme")]
        public DirectResult GetThemeUrl(string theme, bool reload)
        {
            Theme temp = (Theme)Enum.Parse(typeof(Theme), theme);

            this.Session["Ext.Net.Theme"] = temp;

            if (reload)
            {
                X.Reload();
                return this.Direct();
            }

            return this.Direct(MvcResourceManager.GetThemeUrl(temp));
        }

        [DirectMethod]
        [OutputCache(Duration = 86400, VaryByParam = "name")]
        public DirectResult GetHashCode(string name)
        {
            return this.Direct(Math.Abs(name.ToLower().GetHashCode()));
        }

        [DirectMethod]
        public DirectResult ChangeScriptMode(bool debug)
        {
            if (debug)
            {
                this.Session["Ext.Net.ScriptMode"] = Ext.Net.ScriptMode.Debug;
                this.Session["Ext.Net.SourceFormatting"] = true;
            }
            else
            {
                this.Session["Ext.Net.ScriptMode"] = Ext.Net.ScriptMode.Release;
                this.Session["Ext.Net.SourceFormatting"] = false;
            }

            Response.Redirect("");

            return this.Direct();
        }
    }
}
