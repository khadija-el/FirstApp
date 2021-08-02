using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FistApi.Models;

using Microsoft.AspNetCore.Authorization;

namespace FistApi.Controllers
{
    //[Authorize]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ProfilsController  : SuperController<Profil>
    {
        public ProfilsController(FistApiContext context): base(context) { }
    }
}
