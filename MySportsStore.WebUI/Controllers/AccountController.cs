using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MySportsStore.IBLL;
using MySportsStore.WebUI.Models;
using Ninject;
using System.Web.Security;
using MySportsStore.Model;
using System.Text;
using WebMatrix.WebData;

namespace MySportsStore.WebUI.Controllers
{
    public class AccountController : BaseController
    {
        //
        // GET: /Account/

        //注入自定义服务
        [Inject]
        public IUserInfoService UserInfoService { get; set; }

        public AccountController()
        {
            this.AddDisposableObject(UserInfoService);
        }


        /// <summary>
        ///  显示注册页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Register()
        {
            return View();
        }

        //实现功能，注册
        [HttpPost]
        public ActionResult Register(RegisterViewModel model)
        {

            if (ModelState.IsValid)
            {
                
                int isExists =UserInfoService.LoadEntities(p => p.UserEmail == model.UserEmail).Count();
                if (isExists > 0)
                {
                    ModelState.AddModelError("", "邮箱已经注册了");
                    return View();
                }
               
                //加盐，对密码加密
                string salt = Guid.NewGuid().ToString();
                
                byte[] passwordAndSaltBytes = System.Text.Encoding.UTF8.GetBytes(model.UserPwd + salt);
                byte[] hashBytes = new System.Security.Cryptography.SHA256Managed().ComputeHash(passwordAndSaltBytes);

                string hashString = Convert.ToBase64String(hashBytes);
                //用户注册，默认就是一般用户类型
                UserInfoService.AddEntity(new UserInfo { UserName=model.UserName,UserType="60",UserEmail=model.UserEmail,UserPwd=hashString,Salt=salt,UserRank=0,RegisterTime=DateTime.Now});

                return RedirectToAction("List", "Product");

            }
            //验证不通过返回注册页面
            return View();
        }
        public ActionResult Login()
        {
            return View();
        }


        

        //实现登录
        [HttpPost] //该注解表示只接收Post数据
        //[ValidateAntiForgeryToken]//该注解可以防止跨站攻击
        [ActionName("Login")]//该注解可以更改路由中Action名称
        public ActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                //用户输入服务端验证,此处处理验证不通过的提示代码 本例略过             
                return View();
            }

            int isExist = UserInfoService.LoadEntities(p => p.UserName == model.UserName).Count();
            if (isExist == 0)
            {
                ModelState.AddModelError("", "账号不存在");
            }

            var select2 = UserInfoService.LoadEntities(p => p.UserName == model.UserName)
                .Select(m => new { m.UserPwd, m.UserType,m.Salt }).FirstOrDefault();

            string salt_string = select2.Salt.ToString();
            byte[] passwordAndSaltBytes = System.Text.Encoding.UTF8.GetBytes(model.Password + salt_string);
            byte[] hashBytes = new System.Security.Cryptography.SHA256Managed().ComputeHash(passwordAndSaltBytes);
            string hashString = Convert.ToBase64String(hashBytes);

            //usertype用户类型可以相应的做一些处理，转换成相应的权限也可以

            if (hashString == select2.UserPwd)
            {
                // user login successfully 
                //用户登陆核心代码
                FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(
                     1,
                     model.UserName,
                     DateTime.Now,
                     DateTime.Now.AddHours(240),//记住密码的时间
                     model.RememberMe,//是否保存cookie 记住密码
                     select2.UserType, //获取的用户权限列表 用逗号分割的字符串
                     "/"
                     );
                string encryptedTickt = FormsAuthentication.Encrypt(authTicket);//FormsAuthentication.Encrypt 加密票据
                //把加密后的Ticket 存储在Response Cookie中                
                HttpCookie authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTickt);
                //客户端js不需要读取到这个Cookie，所以最好设置HttpOnly=True，防止浏览器攻击窃取、伪造Cookie
                authCookie.HttpOnly = true;
                Response.Cookies.Add(authCookie);

                Response.Redirect("/Home", true);
            }
            else
            {
                ModelState.AddModelError("", "密码不正确");
            } 

            return View();
        }


        public ActionResult Index()
        {
            return View();
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            WebSecurity.Logout();

            return RedirectToAction("Index", "Home");
        }


        #region Status Codes
        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
        #endregion
    }
}
