using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SMS.DTO;
using SMS.WebUI.Core;

namespace SMS.WebUI.Controllers
{
    public class BaseController : Controller
    {
        public UserDTO CurrentUser
        {
            get
            {
                var userDTOJson = HttpContext.User.Claims.FirstOrDefault(z => z.Type == "UserDTO").Value;
                return SMSConvert.SMSJsonDesirializeUserDTO(userDTOJson);
            }
        }
    }
}
