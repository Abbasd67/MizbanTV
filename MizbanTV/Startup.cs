using Microsoft.Owin;
using Owin;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

[assembly: OwinStartupAttribute(typeof(MizbanTV.Startup))]
namespace MizbanTV
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
    public class CustomRequiredAttribute : RequiredAttribute, IClientValidatable
    {
        public CustomRequiredAttribute()
        {
            this.ErrorMessage = "وارد کردن {0} اجباری است.";
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            yield return new ModelClientValidationRule
            {
                ErrorMessage = this.ErrorMessage,
                ValidationType = "required"
            };
        }
    }
}
