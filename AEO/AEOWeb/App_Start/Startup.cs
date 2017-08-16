using AEOWebapi.Controllers.Infrastructure;
using Core.Infrastructure;
using Microsoft.Owin;
using Microsoft.Owin.Security.Infrastructure;
using Microsoft.Owin.Security.OAuth;
using Microsoft.Owin.Logging;
using Owin;
using System;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

[assembly: OwinStartup(typeof(AEOWeb.Startup))]
namespace AEOWeb
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            MvcHandler.DisableMvcResponseHeader = true;

            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            EngineContext.Initialize(false);

            WebApiAuth(app);
        }

        public void WebApiAuth(IAppBuilder app)
        {
            //将用户管理器配置为对每个请求使用单个实例
            app.CreatePerOwinContext<WebapiUserManager>(WebapiUserManager.Create);
            app.SetLoggerFactory();
            var PublicClientId = "self";
            var OAuthOptions = new OAuthAuthorizationServerOptions
            {
                TokenEndpointPath = new PathString("/Token"),
                Provider = new WebapiOAuthProvider(PublicClientId),
                RefreshTokenProvider = new AuthenticationTokenProvider()
                {
                    OnCreate = context=> {
                        context.SetToken(context.SerializeTicket());
                    },
                    OnReceive = context => {
                        context.DeserializeTicket(context.Token);
                    },
                },
                AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(20),
                // Note: Remove the following line before you deploy to production:
                AllowInsecureHttp = true
            };
            // Enable the application to use bearer tokens to authenticate users
            app.UseOAuthBearerTokens(OAuthOptions);
        }
    }
}
