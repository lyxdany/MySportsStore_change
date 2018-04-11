
using System.Web.Mvc;
using MySportsStore.WebUI.Models;

namespace MySportsStore.WebUI.Extension
{
    public class CartModelBinder : IModelBinder
    {
        //静态参数指定为cart
        private const string sessionKey = "Cart";
        //从session中获取cart
        //根据前台传递的id值获取对象
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            Cart cart = (Cart)controllerContext.HttpContext.Session[sessionKey];
            if (cart == null)
            {
                cart = new Cart();
                controllerContext.HttpContext.Session[sessionKey] = cart;
            }
            return cart;
        }
    }
}