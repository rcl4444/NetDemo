using AEOPoco;
using Core;
using Core.Fakes;
using AEOService.Interface;
using System;
using System.Linq;
using System.Web;

namespace AEOWeb.Infrastructure
{
    /// <summary>
    /// Work context for web application
    /// </summary>
    public partial class WebWorkContext : IWorkContext
    {
        #region Const
        private const string CustomerCookieName = "AEO.User";
        #endregion

        #region Fields
        private readonly HttpContextBase _httpContext;
        private readonly IAuthenticationService _authenticationService;
        private AbstractAccount _cachedUser;
        #endregion

        #region Ctor
        public WebWorkContext(HttpContextBase httpContext, IAuthenticationService authenticationService)
        {
            this._httpContext = httpContext;
            this._authenticationService = authenticationService;
        }
        #endregion

        #region Utilities
        protected virtual HttpCookie GetCustomerCookie()
        {
            if (_httpContext == null || _httpContext.Request == null)
                return null;
            return _httpContext.Request.Cookies[CustomerCookieName];
        }

        protected virtual void SetCustomerCookie(string cookieValue)
        {
            if (_httpContext != null && _httpContext.Response != null)
            {
                var cookie = new HttpCookie(CustomerCookieName);
                cookie.HttpOnly = false;
                cookie.Value = Convert.ToBase64String(System.Text.Encoding.Default.GetBytes(cookieValue));
                int cookieExpires = 24;//默认记录一天
                cookie.Expires = DateTime.Now.AddHours(cookieExpires);
                _httpContext.Response.Cookies.Remove(CustomerCookieName);
                _httpContext.Response.Cookies.Add(cookie);
            }
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the current user
        /// </summary>
        public virtual AbstractAccount CurrentUser
        {
            get
            {
                if (_cachedUser != null)
                    return _cachedUser;
                AbstractAccount user = null;
                if (_httpContext == null || _httpContext is FakeHttpContext)
                {
                    user = null;
                }
                if (user == null)
                {
                    var userByCookie = _authenticationService.GetAuthenticatedCustomer();
                    if (userByCookie != null)
                        user = userByCookie;
                }
                if (user != null)
                {
                    SetCustomerCookie("{'username':'" + user.AccountName + "','id':'" + user.Id + "'}");
                    _cachedUser = user;
                }
                return _cachedUser;
            }
            set
            {
                SetCustomerCookie("{'username':'"+ value.AccountName + "','id':'"+ value.Id + "'}");
                _cachedUser = value;
            }
        }

        /// <summary>
        /// Get or set value indicating whether we're in admin area
        /// </summary>
        public virtual bool IsAdmin { get; set; }
        #endregion
    }
}
