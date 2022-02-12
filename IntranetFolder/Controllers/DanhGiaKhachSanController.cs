using IntranetFolder.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IntranetFolder.Controllers
{
    public class DanhGiaKhachSanController : BaseController
    {
        private readonly IDanhGiaKhachSanService _danhGiaKhachSanService;

        public DanhGiaKhachSanController(IDanhGiaKhachSanService danhGiaKhachSanService)
        {
            _danhGiaKhachSanService = danhGiaKhachSanService;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}